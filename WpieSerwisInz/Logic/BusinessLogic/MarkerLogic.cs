using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WpieSerwisInz.Models.BO;

namespace WpieSerwisInz.Logic.BusinessLogic
{
    public static class MarkerLogic
    {
        public static List<Marker> GetMarkerList(List<JunctionModel> models)
        {
            List<Marker> markers = new List<Marker>();
            foreach (JunctionModel md in models)
            {
                Marker mark = new Marker();
                mark.IsEvent = false;
                mark.Title = md.Name;
                mark.Information = md.Information;
                mark.Lat = md.Lat;
                mark.LastModificationDate = md.LastModificationDate;
                mark.Long = md.Long;
                mark.Id = md.Id;
                markers.Add(mark);
            }
            return markers;
        }

        public static List<Marker> GetMarkerList(List<EventModel> models)
        {
            List<Marker> markers = new List<Marker>();
            foreach (EventModel md in models)
            {
                Marker mark = new Marker();
                mark.IsEvent = true;
                mark.Title = md.EventName;
                mark.Information = md.EventInformation;
                mark.Lat = md.Lat;
                mark.Long = md.Long;
                mark.LastModificationDate = md.LastModificationDate;
                mark.Id = md.Id;
                mark.EventType = md.EventTypes.Id;
                mark.AssignedImage = md.EventTypes.AssignedImage;
                markers.Add(mark);
            }
            return markers;
        }
    }
}