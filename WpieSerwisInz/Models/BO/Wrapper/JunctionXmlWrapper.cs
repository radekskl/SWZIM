using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WpieSerwisInz.Models.BO.CADXml.CadWrapper;

namespace WpieSerwisInz.Models.BO.Wrapper
{
    public class JunctionXmlWrapper
    {
        public List<Odcinek> ListaOdcinkow { get; set; }

        public List<Znak> ListaZnakow { get; set; }

        public List<Sygnalizacja> ListaSygnal { get; set; }

        public JunctionXml Xml { get; set; }

        public bool IsUploadFile { get; set; }

        public JunctionXmlWrapper()
        {
            ListaOdcinkow = new List<Odcinek>();
            ListaZnakow = new List<Znak>();
            ListaSygnal = new List<Sygnalizacja>();
        }
    }
}