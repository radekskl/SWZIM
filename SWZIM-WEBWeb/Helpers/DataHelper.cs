using SWZIM_WEBWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

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

        public static List<XMIHelperModel.ProfilCADModel> ParseXMI(HttpPostedFileBase file)
        {
            try
            {
                Dictionary<string, string> idToName = new Dictionary<string, string>();
                Dictionary<string, XMIHelperModel.LatLong> coordinates = new Dictionary<string, XMIHelperModel.LatLong>();
                List<XMIHelperModel.ProfilCADModel> list = new List<XMIHelperModel.ProfilCADModel>();

                Stream stream = GetXMLStream(file);

                XmlTextReader reader = new XmlTextReader(stream);
                while (reader.Read())
                {
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
                        else
                        {
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


                }
           
            foreach (var item in list)
            {
                var attr = item.Attributes.FirstOrDefault(x => x.Key == "lokalizacja");
                if (!attr.Equals(default(KeyValuePair<string,string>)))
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

            return list;
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
                if (item.Coordinates.Count() > 0 && !item.Coordinates.Any(x => x.Equals(default(XMIHelperModel.LatLong)))){
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


    }
}