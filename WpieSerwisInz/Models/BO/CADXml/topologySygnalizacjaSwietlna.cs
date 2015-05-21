using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WpieSerwisInz.Models.BO.CADXml
{
    // <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class topologySygnalizacjaSwietlna
    {

        /// <remarks/>
        public string signcode
        {
            get;
            set;
        }

        /// <remarks/>
        public string nazwa
        {
            get;
            set;
        }

        /// <remarks/>
        public string latitude_degrees
        {
            get;
            set;
        }

        /// <remarks/>
        public string latitude_minutes
        {
            get;
            set;
        }

        /// <remarks/>
        public string latitude_seconds
        {
            get;
            set;
        }


        /// <remarks/>
        public string latitude_miliseconds
        {
            get;
            set;
        }

        /// <remarks/>
        public string longitude_degrees
        {
            get;
            set;
        }

        /// <remarks/>
        public string longitude_minutes
        {
            get;
            set;
        }

        /// <remarks/>
        public string longitude_seconds
        {
            get;
            set;
        }

        /// <remarks/>
        public string longitude_miliseconds
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id { get; set; }
    }
}