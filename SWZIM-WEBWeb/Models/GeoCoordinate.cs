using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SWZIM_WEBWeb.Models
{
    public class GeoCoordinate : IQueryable<GeoCoordinate>
    {
        private readonly double latitude;
        private readonly double longitude;

        public double Latitude { get { return latitude; } }
        public double Longitude { get { return longitude; } }

        public GeoCoordinate(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", Latitude, Longitude);
        }

        public override bool Equals(Object other)
        {
            return other is GeoCoordinate && Equals((GeoCoordinate)other);
        }

        public bool Equals(GeoCoordinate other)
        {
            return Latitude == other.Latitude && Longitude == other.Longitude;
        }

        public override int GetHashCode()
        {
            return Latitude.GetHashCode() ^ Longitude.GetHashCode();
        }
    }
}