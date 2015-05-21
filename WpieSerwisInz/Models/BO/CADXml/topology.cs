using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WpieSerwisInz.Models.BO.CADXml
{

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class topology
    {

        private List<topologyZnak> znakField;

        private List<topologyOdcinek> odcinekField;

        private List<topologySygnalizacjaSwietlna> sygnalizacjaSwietlnaField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Znak")]
        public List<topologyZnak> Znak
        {
            get
            {
                return this.znakField;
            }
            set
            {
                this.znakField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Odcinek")]
        public List<topologyOdcinek> Odcinek
        {
            get
            {
                return this.odcinekField;
            }
            set
            {
                this.odcinekField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SygnalizacjaSwietlna")]
        public List<topologySygnalizacjaSwietlna> SygnalizacjaSwietlna
        {
            get
            {
                return this.sygnalizacjaSwietlnaField;
            }
            set
            {
                this.sygnalizacjaSwietlnaField = value;
            }
        }
    }
}
