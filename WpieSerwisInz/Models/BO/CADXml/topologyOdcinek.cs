using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WpieSerwisInz.Models.BO.CADXml
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class topologyOdcinek
    {

        /// <remarks/>
        public string signcode { get; set; }


        /// <remarks/>
        public string latitude_degrees_begin
        {
            get;
            set;
        }


        /// <remarks/>
        public string latitude_minutes_begin
        {
            get;
            set;
        }


        /// <remarks/>
        public string latitude_seconds_begin
        {
            get;
            set;
        }

        /// <remarks/>
        public string latitude_miliseconds_begin
        {
            get;
            set;
        }


        /// <remarks/>
        public string longitude_degrees_begin
        {
            get;
            set;
        }


        /// <remarks/>
        public string longitude_minutes_begin
        {
            get;
            set;
        }

        /// <remarks/>
        public string longitude_seconds_begin
        {
            get;
            set;
        }

        /// <remarks/>
        public string longitude_miliseconds_begin
        {
            get;
            set;
        }


        /// <remarks/>
        public string latitude_degrees_end
        {
            get;
            set;
        }

        /// <remarks/>
        public string latitude_minutes_end
        {
            get;
            set;
        }

        /// <remarks/>
        public string latitude_seconds_end
        {
            get;
            set;
        }

        /// <remarks/>
        public string latitude_miliseconds_end
        {
            get;
            set;
        }


        /// <remarks/>
        public string longitude_degrees_end
        {
            get;
            set;
        }


        /// <remarks/>
        public string longitude_minutes_end
        {
            get;
            set;
        }


        /// <remarks/>
        public string longitude_seconds_end
        {
            get;
            set;
        }

        /// <remarks/>
        public string longitude_miliseconds_end
        {
            get;
            set;
        }


        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id { get; set; }

    }

}