using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

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
    }
}