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

        public static string Test(HttpPostedFileBase file)
        {
            string result = "";

            Dictionary<string, string> idToName = new Dictionary<string, string>(); // pozniej za value bedzie obiekt

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
                    result += reader.LocalName + Environment.NewLine;
                    if (reader.HasAttributes)
                    {
                        while (reader.MoveToNextAttribute())
                        {
                            result += String.Format(" {0}={1}", reader.Name, reader.Value) + Environment.NewLine;
                            if (reader.Name.Equals("base_Class"))
                                result += String.Format("Nazwa : {0}", idToName[reader.Value]) + Environment.NewLine;
                        }
                        reader.MoveToElement();
                    }
                }


            }

            return result;
        }
    }
}