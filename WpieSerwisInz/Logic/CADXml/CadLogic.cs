using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.DynamicData.ModelProviders;
using System.Web.Services.Description;
using System.Xml.Serialization;
using Microsoft.Ajax.Utilities;
using WpieSerwisInz.Models.BO.CADXml;
using WpieSerwisInz.Models.BO.CADXml.CadWrapper;
using WpieSerwisInz.Models.BO.Wrapper;

namespace WpieSerwisInz.Logic.CADXml
{
    public class CadLogic
    {
        #region SerializeDeserialize

        public topology DeserializeXmlTopology(string xmlTopology)
        {
            topology top = new topology();
            XmlSerializer serializer = new XmlSerializer(typeof(topology));
            using (TextReader reader = new StringReader(xmlTopology))
            {
                top = (topology)serializer.Deserialize(reader);
            }
            return top;
        }

        public string SerializeXmlTopology(topology topologyBegin)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(topology));
            string wynik = String.Empty;
            using (TextWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, topologyBegin);
                wynik = writer.ToString();
            }
            return wynik;
        }

        #endregion


        public topology GetTopology(JunctionXmlWrapper wrapperXml)
        {
            topology topol = new topology();
            topol.Odcinek = new List<topologyOdcinek>();
            topol.Znak = new List<topologyZnak>();
            topol.SygnalizacjaSwietlna = new List<topologySygnalizacjaSwietlna>();
            foreach (Znak znak in wrapperXml.ListaZnakow)
            {
                topol.Znak.Add(ConvertZnakToTopology(znak));
            }
            foreach (Odcinek odcinek in wrapperXml.ListaOdcinkow)
            {
                topol.Odcinek.Add(ConvertOdcinekToTopology(odcinek));
            }
            foreach (Sygnalizacja sygnal in wrapperXml.ListaSygnal)
            {
                topol.SygnalizacjaSwietlna.Add(ConvertSygnalToTopology(sygnal));
            }
            return topol;
        }

        public JunctionXmlWrapper GetTopologyWrapper(topology topologyXml)
        {
            JunctionXmlWrapper topol = new JunctionXmlWrapper();
            topol.ListaOdcinkow = new List<Odcinek>();
            topol.ListaZnakow = new List<Znak>();
            topol.ListaSygnal = new List<Sygnalizacja>();
            foreach (topologyZnak znak in topologyXml.Znak)
            {
                topol.ListaZnakow.Add(ConvertTopologyToZnak(znak));
            }
            foreach (topologyOdcinek odcinek in topologyXml.Odcinek)
            {
                topol.ListaOdcinkow.Add(ConvertTopologyToOdcinek(odcinek));
            }
            foreach (topologySygnalizacjaSwietlna sygnal in topologyXml.SygnalizacjaSwietlna)
            {
                topol.ListaSygnal.Add(ConvertTopologyToSygnal(sygnal));
            }
            return topol;
        }
        #region Sygnal

        public Sygnalizacja ConvertTopologyToSygnal(topologySygnalizacjaSwietlna topolSygn)
        {
            Sygnalizacja sygnal = new Sygnalizacja();
            sygnal.Signcode = topolSygn.signcode;
            sygnal.Nazwa = topolSygn.nazwa;
            sygnal.Id = topolSygn.id;
            sygnal.Lat = ConvertToGMaps(ConvertDegreeAngleToDouble(Decimal.Parse(topolSygn.latitude_degrees),
                Decimal.Parse(topolSygn.latitude_minutes), Decimal.Parse(topolSygn.latitude_seconds), Decimal.Parse(topolSygn.latitude_miliseconds)).ToString());
            sygnal.Long = ConvertToGMaps(ConvertDegreeAngleToDouble(Decimal.Parse(topolSygn.longitude_degrees),
                Decimal.Parse(topolSygn.longitude_minutes), Decimal.Parse(topolSygn.longitude_seconds), Decimal.Parse(topolSygn.longitude_miliseconds)).ToString());
            return sygnal;
        }

        public topologySygnalizacjaSwietlna ConvertSygnalToTopology(Sygnalizacja sygnal)
        {
            topologySygnalizacjaSwietlna topolSygnal = new topologySygnalizacjaSwietlna();

            topolSygnal.signcode = sygnal.Signcode;
            topolSygnal.nazwa = sygnal.Nazwa;
            topolSygnal.id = sygnal.Id;
            //Lat
            topolSygnal.latitude_degrees = ExtractDegrees(Decimal.Parse(ConvertToDecimal(sygnal.Lat))).ToString();
            topolSygnal.latitude_minutes = ExtractMinutes(Decimal.Parse(ConvertToDecimal(sygnal.Lat))).ToString();
            string sec = "0";
            string milisec = "0";
            ExtractSeconds(Decimal.Parse(ConvertToDecimal(sygnal.Lat)), out sec,
                out milisec);
            topolSygnal.latitude_seconds = sec;
            topolSygnal.latitude_miliseconds = milisec;
            //Long
            topolSygnal.longitude_degrees = ExtractDegrees(Decimal.Parse(ConvertToDecimal(sygnal.Long))).ToString();
            topolSygnal.longitude_minutes = ExtractMinutes(Decimal.Parse(ConvertToDecimal(sygnal.Long))).ToString();
            sec = "0";
            milisec = "0";
            ExtractSeconds(Decimal.Parse(ConvertToDecimal(sygnal.Long)), out sec,
                out milisec);
            topolSygnal.longitude_seconds = sec;
            topolSygnal.longitude_miliseconds = milisec;

            return topolSygnal;
        }

        #endregion


        #region Znak

        public Znak ConvertTopologyToZnak(topologyZnak topolZnak)
        {
            Znak znak = new Znak();
            znak.Signcode = topolZnak.signcode;
            znak.Nazwa = topolZnak.nazwa;
            znak.Id = topolZnak.id;
            znak.Lat = ConvertToGMaps(ConvertDegreeAngleToDouble(Decimal.Parse(topolZnak.latitude_degrees),
                Decimal.Parse(topolZnak.latitude_minutes), Decimal.Parse(topolZnak.latitude_seconds), Decimal.Parse(topolZnak.latitude_miliseconds)));
            znak.Long = ConvertToGMaps(ConvertDegreeAngleToDouble(Decimal.Parse(topolZnak.longitude_degrees),
               Decimal.Parse(topolZnak.longitude_minutes), Decimal.Parse(topolZnak.longitude_seconds), Decimal.Parse(topolZnak.longitude_miliseconds)));
            return znak;
        }

        public topologyZnak ConvertZnakToTopology(Znak znak)
        {
            topologyZnak topolSygnal = new topologyZnak();

            topolSygnal.signcode = znak.Signcode;
            topolSygnal.nazwa = znak.Nazwa;
            topolSygnal.id = znak.Id;
            //Lat
            topolSygnal.latitude_degrees = ExtractDegrees(Decimal.Parse(ConvertToDecimal(znak.Lat))).ToString();
            topolSygnal.latitude_minutes = ExtractMinutes(Decimal.Parse(ConvertToDecimal(znak.Lat))).ToString();
            string sec = "0";
            string milisec = "0";
            ExtractSeconds(Decimal.Parse(ConvertToDecimal(znak.Lat)), out sec,
                out milisec);
            topolSygnal.latitude_seconds = sec;
            topolSygnal.latitude_miliseconds = milisec;
            //Long
            topolSygnal.longitude_degrees = ExtractDegrees(Decimal.Parse(ConvertToDecimal(znak.Long))).ToString();
            topolSygnal.longitude_minutes = ExtractMinutes(Decimal.Parse(ConvertToDecimal(znak.Long))).ToString();

            sec = "0";
            milisec = "0";
            ExtractSeconds(Decimal.Parse(ConvertToDecimal(znak.Long)), out sec,
                out milisec);

            topolSygnal.longitude_seconds = sec;
            topolSygnal.longitude_miliseconds = milisec;

            return topolSygnal;
        }

        #endregion

        #region Odcinek

        public Odcinek ConvertTopologyToOdcinek(topologyOdcinek topolOdcinek)
        {
            Odcinek odcinek = new Odcinek();
            odcinek.Signcode = topolOdcinek.signcode;
            odcinek.Id = topolOdcinek.id;
            odcinek.LatStart = ConvertToGMaps(ConvertDegreeAngleToDouble(Decimal.Parse(topolOdcinek.latitude_degrees_begin),
                Decimal.Parse(topolOdcinek.latitude_minutes_begin), Decimal.Parse(topolOdcinek.latitude_seconds_begin), Decimal.Parse(topolOdcinek.latitude_miliseconds_begin)).ToString());

            odcinek.LongStart = ConvertToGMaps(ConvertDegreeAngleToDouble(Decimal.Parse(topolOdcinek.longitude_degrees_begin),
               Decimal.Parse(topolOdcinek.longitude_minutes_begin), Decimal.Parse(topolOdcinek.longitude_seconds_begin), Decimal.Parse(topolOdcinek.longitude_miliseconds_begin)).ToString());

            odcinek.LatEnd = ConvertToGMaps(ConvertDegreeAngleToDouble(Decimal.Parse(topolOdcinek.latitude_degrees_end),
                Decimal.Parse(topolOdcinek.latitude_minutes_end), Decimal.Parse(topolOdcinek.latitude_seconds_end), Decimal.Parse(topolOdcinek.latitude_miliseconds_end)).ToString());

            odcinek.LongEnd = ConvertToGMaps(ConvertDegreeAngleToDouble(Decimal.Parse(topolOdcinek.longitude_degrees_end),
                Decimal.Parse(topolOdcinek.longitude_minutes_end), Decimal.Parse(topolOdcinek.longitude_seconds_end), Decimal.Parse(topolOdcinek.longitude_miliseconds_end)).ToString());

            return odcinek;
        }

        public topologyOdcinek ConvertOdcinekToTopology(Odcinek odcinek)
        {
            topologyOdcinek tpoloOdcinek = new topologyOdcinek();

            tpoloOdcinek.signcode = odcinek.Signcode;
            tpoloOdcinek.id = odcinek.Id;
            //Lat Start
            tpoloOdcinek.latitude_degrees_begin = ExtractDegrees(Decimal.Parse(ConvertToDecimal(odcinek.LatStart))).ToString();
            tpoloOdcinek.latitude_minutes_begin = ExtractMinutes(Decimal.Parse(ConvertToDecimal(odcinek.LatStart))).ToString();


            string sec = "0";
            string milisec = "0";
            ExtractSeconds(Decimal.Parse(ConvertToDecimal(odcinek.LatStart)), out sec,
                out milisec);
            tpoloOdcinek.latitude_seconds_begin = sec;
            tpoloOdcinek.latitude_miliseconds_begin = milisec;

            //Lat End
            tpoloOdcinek.latitude_degrees_end = ExtractDegrees(Decimal.Parse(ConvertToDecimal(odcinek.LatEnd))).ToString();
            tpoloOdcinek.latitude_minutes_end = ExtractMinutes(Decimal.Parse(ConvertToDecimal(odcinek.LatEnd))).ToString();


            sec = "0";
            milisec = "0";
            ExtractSeconds(Decimal.Parse(ConvertToDecimal(odcinek.LatEnd)), out sec,
                out milisec);

            tpoloOdcinek.latitude_seconds_end = sec;
            tpoloOdcinek.latitude_miliseconds_end = milisec;

            //Long Start
            tpoloOdcinek.longitude_degrees_begin = ExtractDegrees(Decimal.Parse(ConvertToDecimal(odcinek.LongStart))).ToString();
            tpoloOdcinek.longitude_minutes_begin = ExtractMinutes(Decimal.Parse(ConvertToDecimal(odcinek.LongStart))).ToString();

            sec = "0";
            milisec = "0";
            ExtractSeconds(Decimal.Parse(ConvertToDecimal(odcinek.LongStart)), out sec,
                out milisec);

            tpoloOdcinek.longitude_seconds_begin = sec;
            tpoloOdcinek.longitude_miliseconds_begin = milisec;

            //Long End
            tpoloOdcinek.longitude_degrees_end = ExtractDegrees(Decimal.Parse(ConvertToDecimal(odcinek.LongEnd))).ToString();
            tpoloOdcinek.longitude_minutes_end = ExtractMinutes(Decimal.Parse(ConvertToDecimal(odcinek.LongEnd))).ToString();

            sec = "0";
            milisec = "0";
            ExtractSeconds(Decimal.Parse(ConvertToDecimal(odcinek.LongEnd)), out sec,
                out milisec);
            tpoloOdcinek.longitude_seconds_end = sec;
            tpoloOdcinek.longitude_miliseconds_end = milisec;


            return tpoloOdcinek;
        }

        #endregion

        #region Helpers

        public string ConvertDegreeAngleToDouble(decimal degrees, decimal minutes, decimal seconds, decimal miliseconds)
        {
            decimal secondsDec = Decimal.Parse(String.Format("{0},{1}", seconds, miliseconds));
            return String.Format("{0:0.############}", degrees + (minutes / 60) + (secondsDec / 3600));
        }

        public int ExtractDegrees(decimal value)
        {
            return (int)value;
        }

        public int ExtractMinutes(decimal value)
        {
            value = Math.Abs(value);
            return (int)((value - ExtractDegrees(value)) * 60);
        }

        public void ExtractSeconds(decimal value, out string seconds, out string miliseconds)
        {
            value = Math.Abs(value);
            decimal minutes = (value - ExtractDegrees(value)) * 60;

            decimal secondsDoub = (minutes - ExtractMinutes(value)) * 60;

            var s = secondsDoub.ToString().Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            seconds = s.First();
            miliseconds = s.Last();
            // return (int)Math.Round((minutes - ExtractMinutes(value)) * 60);
        }

        public string ConvertToDecimal(string value)
        {
            string result;
            result = value.Replace('.', ',');
            return result;
        }

        public string ConvertToGMaps(string value)
        {
            string result;
            result = value.Replace(',', '.');
            return result;
        }

        #endregion
    }
}