using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using WpieSerwisInz.Logic.BusinessLogic;
using WpieSerwisInz.Models;
using WpieSerwisInz.Models.BO;
using WpieSerwisInz.Models.BO.Core.CheckboxListBo;
using WpieSerwisInz.Models.BO.Dictionary;
using WpieSerwisInz.Models.BO.Wrapper;

namespace WpieSerwisInz.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public HomeController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
            DbContext = new ApplicationDbContext();
        }

        private int homePageSize = 15;

        public UserManager<ApplicationUser> UserManager { get; private set; }
        public ApplicationDbContext DbContext { get; set; }

        public ActionResult Index(int? page)
        {
            DbContext = new ApplicationDbContext();
            MainPageWrapper model = GetDefaultMainPageContext(null);
            int pageSize = homePageSize;
            int pageNumber = (page ?? 1);
            model.MarkerListShow = model.MarkerList.ToPagedList(pageNumber, pageSize);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Index(string submit, MainPageWrapper model, int? page)
        {
            DbContext = new ApplicationDbContext();
            EventController evt = new EventController();
            JunctionController junc = new JunctionController();
            switch (submit)
            {
                case "Event":
                    evt.AddEvent(model.EventModel, User.Identity.GetUserId());
                    break;
                case "Cross":
                    junc.AddCross(model.JunctionModel, User.Identity.GetUserId());
                    break;
            }

            MainPageWrapper wrapper = GetDefaultMainPageContext(model);

            int pageSize = homePageSize;
            int pageNumber = (page ?? 1);

            wrapper.MarkerListShow = wrapper.MarkerList.ToPagedList(pageNumber, pageSize);

            return View(wrapper);
        }

        private MainPageWrapper GetDefaultMainPageContext(MainPageWrapper mainPageWrapper)
        {
            MainPageWrapper home = new MainPageWrapper();
            List<JunctionModel> junctions = DbContext.JunctionModelDbSet.ToList(); //Tutaj trzeba dodac ogarniczenia (Prawo/Lewo/Gora/Dol)      
            List<EventModel> events = new List<EventModel>();
            List<Marker> markers = new List<Marker>();

            if (User.Identity.IsAuthenticated &&
              UserLogic.IsLayerForUser(User.Identity.GetUserId()))
            {
                if (mainPageWrapper == null || mainPageWrapper.PostedService == null)
                {
                    LayerViewWrapper layer =
                        LayerLogic.GetLayerViewMainPage(UserManager.FindById(User.Identity.GetUserId()).Layer.Id);
                    home.SelectedElement = layer.SelectedElement;
                    home.AvailibleElement = layer.AvailibleElement;
                    home.PostedService = layer.PostedService;
                }
                else
                {
                    string id = User.Identity.GetUserId();
                    var user = DbContext.Users.FirstOrDefault(x => x.Id == id);
                    home = LayerLogic.GetPostedService(mainPageWrapper, user.Layer.Id);
                }
                events = EventLogic.GetEventForSelected(home.SelectedElement);
            }
            else
            {
                events = DbContext.EventModelDbSet.OrderByDescending(x => x.LastModificationDate).ToList();
                home.SelectedElement = new List<LayerModel>();
                home.AvailibleElement = new List<LayerModel>();
                home.PostedService = new PostedService();
            }
            home.EventModel = new EventWrapper();
            home.EventModel.SelectedElement = new List<LayerModel>();
            home.EventModel.AvailibleElement = LayerLogic.GetListLayer(DbContext.LayerElementDbSet.ToList(), false);
            home.EventModel.PostedService = new PostedService();
            markers.AddRange(MarkerLogic.GetMarkerList(events));
            if (User.IsInRole(RoleType.Developer) || User.IsInRole(RoleType.Admin))
            {
                markers.AddRange(MarkerLogic.GetMarkerList(junctions));
            }
            home.MarkerList = markers;
            return home;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}