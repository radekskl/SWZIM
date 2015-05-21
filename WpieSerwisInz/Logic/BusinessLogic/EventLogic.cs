using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WpieSerwisInz.Models;
using WpieSerwisInz.Models.BO;
using WpieSerwisInz.Models.BO.Core.CheckboxListBo;

namespace WpieSerwisInz.Logic.BusinessLogic
{
    public static class EventLogic
    {

        static EventLogic()
        {
            DbContext = new ApplicationDbContext();
        }

        public static ApplicationDbContext DbContext { get; set; }

        public static List<SelectListItem> GetListType()
        {
                var evTypes = DbContext.EventTypeDbSet.ToList();
                List<SelectListItem> dictType = new List<SelectListItem>();
                bool selected = true;
                foreach (EventType eType in evTypes)
                {
                    dictType.Add(new SelectListItem()
                    {
                        Selected = selected,
                        Value = eType.Id.ToString(),
                        Text = eType.EventName
                    });
                    if (selected)
                    {
                        selected = false;
                    }
                }
                return dictType;
        }

        public static List<EventModel> GetEventForSelected(List<LayerModel> layerModels)
        {
                var listEvents = DbContext.EventModelDbSet.ToList();
                var returnList = new List<EventModel>();
                foreach (EventModel em in listEvents)
                {
                    if (layerModels.Where(x => em.LayerElements.Any(z => z.Id == x.Id)).Any())
                    {
                        returnList.Add((EventModel)em);
                    }
                }
                return returnList;
        }

        
    }
}