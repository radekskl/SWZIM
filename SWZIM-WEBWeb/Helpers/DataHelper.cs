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

        public static List<XMIHelperModel.ProfilCADModel> Test(HttpPostedFileBase file)
        {
            //string result = "";

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
                        var lat = decimal.Parse(reader.GetAttribute("szerokoscGeograficzna"));
                        var log = decimal.Parse(reader.GetAttribute("dlugoscGeograficzna"));
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
                                if (reader.Name.Equals("base_Class")){
                                    //result += String.Format("Nazwa : {0}", idToName[reader.Value]) + "\n";
                                    mdl.Name = idToName[reader.Value];
                                } 
                                else
                                {
                                    mdl.Attributes.Add(reader.Name, reader.Value);
                                    //result += String.Format(" {0}={1}", reader.Name, reader.Value) + "\n";
                                }
                            }
                            reader.MoveToElement();
                        }
                        list.Add(mdl);
                    }
                    
                }


            }

            return list;
        }
    }
}