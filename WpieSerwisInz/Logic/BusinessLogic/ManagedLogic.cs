using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WpieSerwisInz.Models.BO;

namespace WpieSerwisInz.Logic.BusinessLogic
{
    public class ManagedLogic
    {
        public List<InfoObject> GetInfoObjects(List<EventModel> models)
        {
            List<InfoObject> listObj = new List<InfoObject>();
            foreach (EventModel evM in models.OrderByDescending(x => x.LastModificationDate).Take(10))
            {
                InfoObject obj = new InfoObject()
                {
                    Id = evM.Id,
                    Name = evM.EventName,
                    Information = evM.EventInformation,
                    LastModificationDate = evM.LastModificationDate,
                    Action = "Details",
                    Controller = "Event",
                    Type = "Event"
                };
                listObj.Add(obj);
            }
            return listObj;
        }

        public List<InfoObject> GetInfoObjects(List<JunctionModel> models)
        {
            List<InfoObject> listObj = new List<InfoObject>();
            foreach (JunctionModel evM in models.OrderByDescending(x => x.LastModificationDate).Take(10))
            {
                InfoObject obj = new InfoObject()
                {
                    Id = evM.Id,
                    Name = evM.Name,
                    Information = evM.Information,
                    LastModificationDate = evM.LastModificationDate,
                    Action = "Details",
                    Controller = "Junction",
                    Type = "Junction"
                };
                listObj.Add(obj);
            }
            return listObj;
        }

        public List<InfoObject> GetInfoObjects(List<JunctionXml> models)
        {
            List<InfoObject> listObj = new List<InfoObject>();
            foreach (JunctionXml evM in models.OrderByDescending(x => x.LastModificationDate).Take(10))
            {
                InfoObject obj = new InfoObject()
                {
                    Id = evM.Id,
                    Name = "Xml - "+evM.Junctiom.Name,
                    Information = evM.AdditionalInformation,
                    LastModificationDate = evM.LastModificationDate,
                    Action = "Details",
                    Controller = "JunctionXml",
                    Type = "JunctionXml"
                };
                listObj.Add(obj);
            }
            return listObj;
        }
    }
}