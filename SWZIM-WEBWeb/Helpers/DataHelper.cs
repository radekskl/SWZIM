﻿using SWZIM_WEBWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Data.Entity;

namespace SWZIM_WEBWeb.Helpers
{
    public class DataHelper
    {
        public static Stream GetXMLStream(HttpPostedFileBase file)
        {
            using (Stream inputStream = file.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                memoryStream = new MemoryStream();
                inputStream.CopyTo(memoryStream);
                memoryStream.Position = 0;
                return memoryStream;
            }
        }

        public static Tuple<List<XMIHelperModel.ProfilCADModel>,string> ParseXMI(HttpPostedFileBase file)
        {
            try
            {
                Dictionary<string, string> idToName = new Dictionary<string, string>();
                Dictionary<string, XMIHelperModel.LatLong> coordinates = new Dictionary<string, XMIHelperModel.LatLong>();
                List<XMIHelperModel.ProfilCADModel> list = new List<XMIHelperModel.ProfilCADModel>();

                Stream stream = GetXMLStream(file);

                XmlTextReader reader = new XmlTextReader(stream);
                string result = "";
                while (reader.Read())
                {
                    if (reader.Prefix.Equals("uml") && reader.LocalName.Equals("Model"))
                    {
                        result += reader.GetAttribute("xmi:id") + "|";
                    }
                    if (reader.Name.Equals("packageImport"))
                    {
                        result += reader.GetAttribute("xmi:id");
                    }

                    if (reader.Name.Equals("packagedElement"))
                    {
                        var xmiId = reader.GetAttribute("xmi:id");
                        var xmiName = reader.GetAttribute("name");
                        idToName.Add(xmiId, xmiName);
                    }
                    if (reader.Prefix.Equals("ProfilCAD"))
                    {
                        if (reader.LocalName.Equals("Lokalizacja")) // kordyty do obiektow
                        {
                            var xmiId = reader.GetAttribute("xmi:id");
                            decimal lat;
                            bool res = decimal.TryParse(reader.GetAttribute("szerokoscGeograficzna"), out lat);
                            if (res == false)
                            {
                                lat = 0;
                            }
                            decimal log;
                            bool res2 = decimal.TryParse(reader.GetAttribute("dlugoscGeograficzna"), out log);
                            if (res2 == false)
                            {
                                log = 0;
                            }
                            // walidacja (do sprawdzenia)
                            if (lat > 90)
                                lat = 90;
                            if (lat < -90)
                                lat = -90;
                            if (log > 180)
                                log = 180;
                            if (log < -180)
                                log = -180;
                            coordinates.Add(xmiId, new XMIHelperModel.LatLong() { Latitude = lat, Longitude = log });
                        }
                        XMIHelperModel.ProfilCADModel mdl = new XMIHelperModel.ProfilCADModel();
                        mdl.ClassName = reader.LocalName;
                        //result += reader.LocalName + "\n";
                        if (reader.HasAttributes)
                        {
                            while (reader.MoveToNextAttribute())
                            {
                                if (reader.Name.Equals("base_Class"))
                                {
                                    //result += String.Format("Nazwa : {0}", idToName[reader.Value]) + "\n";
                                    mdl.Name = idToName[reader.Value];
                                }
                                mdl.Attributes.Add(reader.Name, reader.Value);
                            }
                            reader.MoveToElement();
                        }
                        list.Add(mdl);

                    }


                }

                foreach (var item in list)
                {
                    var attr = item.Attributes.FirstOrDefault(x => x.Key == "lokalizacja");
                    if (!attr.Equals(default(KeyValuePair<string, string>)))
                    {
                        if (attr.Value.Contains(" "))
                        {
                            foreach (string coordiate in attr.Value.Split(' '))
                            {
                                item.Coordinates.Add(coordinates[coordiate]);
                            }
                        }
                        else
                        {
                            item.Coordinates.Add(coordinates[attr.Value]);
                        }
                    }
                    // podzial na wydarzenia i elementy modelu
                    if (item.ClassName.Equals("Wypadek"))
                        item.Type = 1; // event
                    else
                        item.Type = 0; // layoutElement
                }
                Tuple<List<XMIHelperModel.ProfilCADModel>, string> tuple = new Tuple<List<XMIHelperModel.ProfilCADModel>, string>(list, result);
                return tuple;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Debug here! " + ex);
                return null;
            }
        }

        public static List<LayoutElements> RefactorCADProfileToLayoutElements(List<XMIHelperModel.ProfilCADModel> input, int layerId, int userId)
        {
            List<LayoutElements> list = new List<LayoutElements>();
            foreach (var item in input)
            {
                LayoutElements le = new LayoutElements();
                le.LayersId = layerId;
                le.LayoutElementTypeId = LayoutElementsHelper.GetLEType(item.ClassName);
                le.Name = item.Name;
                if (item.Coordinates.Count() > 0 && !item.Coordinates.Any(x => x.Equals(default(XMIHelperModel.LatLong))))
                {
                    le.Longitude = item.Coordinates.First().Longitude; //TODO: co z drogami, gdzie jest 2 koordynaty?
                    le.Latitude = item.Coordinates.First().Latitude;
                }

                le.Description = "Zaimportowany z pliku XMI";
                le.UserId = userId;
                foreach (var itx in item.Attributes)
                {
                    le.LayoutElementAttributes.Add(new LayoutElementAttributes() { Name = itx.Key, Value = itx.Value });
                }

                //if (item.Coordinates.Count > 1 && !item.Coordinates.Any(x => x.Equals(default(XMIHelperModel.LatLong)))) // jesli ma wiecej niz 1 wsp. to tworzymy jego pod elementy
                //{
                //    for (int i = 0; i < item.Coordinates.Count; i++)
                //    {
                //        LayoutElements lex = new LayoutElements();
                //        lex.LayersId = layerId;
                //        lex.LayoutElementTypeId = 4; // Punkt
                //        lex.Name = item.Name + "[#" + i + "]";
                //        lex.Longitude = item.Coordinates[i].Longitude; //TODO: co z drogami, gdzie jest 2 koordynaty?
                //        lex.Latitude = item.Coordinates[i].Latitude;
                //        le.Description = "Należy do: " + le.Name;
                //        le.UserId = userId;
                //        foreach (var itx in item.Attributes)
                //        {
                //            lex.LayoutElementAttributes.Add(new LayoutElementAttributes() { Name = itx.Key, Value = itx.Value });
                //        }
                //        le.LayoutElements1.Add(lex);
                //    }
                //}

                list.Add(le);

            }
            return list;
        }

        public static List<Events> RefactorCADProfileToEvents(List<XMIHelperModel.ProfilCADModel> input, int userId)
        {
            List<Events> list = new List<Events>();
            foreach (var item in input)
            {
                if (item.Coordinates.Count() > 0 && !item.Coordinates.Any(x => x.Equals(default(XMIHelperModel.LatLong)))) // jezeli nie maja kordynatow to nie wpisujemy ich
                {
                    Events e = new Events();
                    e.AddedBy = userId;
                    e.CreatedAt = DateTime.Now;
                    e.Description = "Zaimportowane z pliku XMI";
                    e.EventTypeId = EventsHelper.GetEventType(item.ClassName);
                    e.Latitude = item.Coordinates.First().Latitude;
                    e.Longitude = item.Coordinates.First().Longitude;
                    e.Name = item.Name;
                    e.Status = 0;
                    list.Add(e);
                }
            }
            return list;
        }

        public static string GetXMIDocument(string path)
        {
            string result = "";

            using (StreamReader reader = new StreamReader(path))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }

        public static string GetPackageElementList(Dictionary<string, string> input)
        {
            string result = "";

            foreach (var item in input)
            {
                result += GetPackageElement(item) + Environment.NewLine;
            }

            return result;
        }

        public static Dictionary<string, string> GetPackageElementDict(int layerId)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            using (var db = new SWZIM_dbEntities())
            {
                foreach (var item in db.LayoutElements.Where(le => le.LayersId == layerId))
                {
                    var baseClass = item.LayoutElementAttributes.Where(lea => lea.Name.Equals("base_Class")).FirstOrDefault();
                    if (baseClass != null && !dict.ContainsKey(baseClass.Value))
                        dict.Add(baseClass.Value, item.Name);
                    else
                        dict.Add(GenerateBaseClassId(), item.Name);

                    //dodanie packageElementow odnosnie lokalizacji
                    var lokal = item.LayoutElementAttributes.Where(lea => lea.Name.Equals("lokalizacja")).FirstOrDefault();
                    //if (lokal != null && !dict.ContainsKey(lokal.Value))
                     //   dict.Add(lokal.Value, item.Name + "Poczatek");

                    var koniec = item.LayoutElementAttributes.Where(lea => lea.Name.Equals("koniecDrogi")).FirstOrDefault();
                    //if (koniec != null && !dict.ContainsKey(koniec.Value))
                    //    dict.Add(koniec.Value, item.Name + "Koniec");
                }
                foreach (var item in db.Layers.Find(layerId).Events)
                {
                    dict.Add(GenerateBaseClassId(), item.Name);
                }
            }

            return dict;
        }

        private static string GetPackageElement(KeyValuePair<string, string> input)
        {
            return @"<packagedElement xmi:type=""uml:Class"" xmi:id="""
                    + input.Key +
                    @""" name=""" + input.Value + @"""/>";
        }



        public static string GenerateBaseClassId()
        {
            return "_pZ" + RandomHelper.RandomString(20);
        }

        public static Dictionary<string, LayoutElements> GetProfilContentDict(Dictionary<string, string> key, int layerId)
        {
            Dictionary<string, LayoutElements> dict = new Dictionary<string, LayoutElements>();

            using (var db = new SWZIM_dbEntities())
            {
                foreach (var item in key.Keys)
                {
                    var fromDB = db.LayoutElements.Include(le => le.LayoutElementAttributes).Include(le => le.LayoutElementTypes).
                        Where(le => le.LayersId == layerId && le.LayoutElementAttributes.
                            Any(lea => lea.Value.Equals(item) && lea.Name.Equals("base_Class"))).FirstOrDefault();
                    if (fromDB != null && !dict.ContainsKey(item))
                        dict.Add(item, fromDB);
                }
            }

            return dict;
        }

        public static string GetProfilContentList(Dictionary<string, LayoutElements> input)
        {
            string result = "";

            foreach (var item in input)
            {
                if (item.Value.LayoutElementTypeId == 21) // lokalizacja
                    result += GetCoordinatesForProfilContent(item.Value, input) + Environment.NewLine;
                else
                    result += GetProfilContent(item.Value) + Environment.NewLine;
            }

            return result;
        }

        private static string GetProfilContent(LayoutElements input)
        {
            string attr = "";
            foreach (var item in input.LayoutElementAttributes)
            {
                attr += item.Name + @"=""" + item.Value + @""" ";
            }
            return @"<ProfilCAD:" + input.LayoutElementTypes.Name + " " + attr + @"/>";
        }

        private static string GetCoordinatesForProfilContent(LayoutElements input, Dictionary<string, LayoutElements> dict)
        {
            string attr = "";
            foreach (var item in input.LayoutElementAttributes.Where(lea => !lea.Name.Equals("szerokoscGeograficzna") && !lea.Name.Equals("dlugoscGeograficzna")))
            {
                attr += item.Name + @"=""" + item.Value + @""" ";
            }
            var xmiId = input.LayoutElementAttributes.Where(lea => lea.Name.Equals("xmi:id")).FirstOrDefault();
            if (xmiId != null)
            {
                var elem = dict.Values.Where(le => le.LayoutElementAttributes
                .Any(lea => lea.Value.Equals(xmiId.Value))).FirstOrDefault();
                if (elem != null)
                {
                    attr += @"szerokoscGeograficzna=""" + elem.Latitude + @""" dlugoscGeograficzna=""" + elem.Longitude + @"""";
                }
            }



            return @"<ProfilCAD:" + input.LayoutElementTypes.Name + " " + attr + @"/>";
        }

    }
}