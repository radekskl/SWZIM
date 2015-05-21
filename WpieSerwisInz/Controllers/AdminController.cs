using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using PagedList;
using WebGrease.Css.Extensions;
using WpieSerwisInz.Logic.BusinessLogic;
using WpieSerwisInz.Models;
using WpieSerwisInz.Logic.Helpers;
using WpieSerwisInz.Models.BO;
using WpieSerwisInz.Models.BO.Core.CheckboxListBo;
using WpieSerwisInz.Models.BO.Wrapper;
using System.Data.Entity;

namespace WpieSerwisInz.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private int pageElement = 5;

        public AdminController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AdminController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
            DbContext = new ApplicationDbContext();
            
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }
        public ApplicationDbContext DbContext { get; set; }

        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }
        #region UserAdmin
        public ViewResult Users(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.LoginSortParm = sortOrder == "Login" ? "login_desc" : "Login";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var users = DbContext.Users.ToList();

            var model = new List<EditUserViewModel>();
            foreach (var user in users)
            {

                var u = new EditUserViewModel(user);
                if (user.Roles.Count > 0)
                {
                    string roleName = String.Empty;
                    foreach (IdentityUserRole role in user.Roles)
                    {
                        if (!String.IsNullOrEmpty(roleName))
                        {
                            roleName = roleName + "; ";
                        }
                        roleName = roleName + ExtendedUserRoles.GetRoleById(role.RoleId);
                    }
                    u.Roles = roleName;
                }

                if (user.Layer != null)
                {
                    u.Layer = user.Layer.LayerName;
                }
                model.Add(u);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.LastName != null && s.LastName.ToUpper().Contains(searchString.ToUpper())).ToList();
            }
            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.LastName).ToList();
                    break;
                case "date_desc":
                    model = model.OrderByDescending(s => s.CreateDate).ToList();
                    break;
                case "Date":
                    model = model.OrderBy(s => s.CreateDate).ToList();
                    break;
                case "login_desc":
                    model = model.OrderByDescending(s => s.UserName).ToList();
                    break;
                case "Login":
                    model = model.OrderBy(s => s.UserName).ToList();
                    break;
                default:  // Name ascending 
                    model = model.OrderBy(s => s.LastName).ToList();
                    break;
            }

            int pageSize = pageElement;
            int pageNumber = (page ?? 1);
            return View("UserView/Users", model.ToPagedList(pageNumber, pageSize));
        }

        [AdminOnly]
        public async Task<ActionResult> Activate(string id)
        {
            ExtendedUserRoles.ActivateUser(id);
            return RedirectToAction("Users");
        }

        public ViewResult UserCreate()
        {
            var Db = new ApplicationDbContext();
            RegisterAdminViewModel model = new RegisterAdminViewModel();
            model.RolesList = new SelectUserRolesViewModel(true);
            return View("UserView/UserCreate", model);
        }

        [HttpPost]
        public async Task<ActionResult> UserCreate(RegisterAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var idManager = new IdentityManager();
                var user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.Phone,
                    Email = model.Email
                };
                user.CreateDate = DateTime.Now;
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    foreach (var role in model.RolesList.Roles)
                    {
                        if (role.Selected)
                        {
                            idManager.AddUserToRole(user.Id, role.RoleName);
                        }
                    }
                    return RedirectToAction("Users");
                }
                else
                {
                    AddErrors(result);
                }
            }

            return View("UserView/UserCreate", model);
        }

        [Authorize]
        public ActionResult UserEdit(string id, AccountController.ManageMessageId? Message = null)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.FirstOrDefault(u => u.Id == id);
            var model = new EditUserViewModel(user);
            ViewBag.MessageId = Message;
            return View("UserView/UserEdit", model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserEdit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Db = new ApplicationDbContext();
                var user = Db.Users.FirstOrDefault(u => u.Id == model.Id);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.Phone;
                user.Email = model.Email;
                Db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Users");
            }
            return View("UserView/UserEdit", model);
        }


        // GET: /Admin/Delete/5
        public ViewResult Roles()
        {
            return View();
        }

        [AdminOnly]
        public ActionResult UserDelete(string id = null)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.FirstOrDefault(u => u.Id == id);
            var model = new EditUserViewModel(user);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View("UserView/UserDelete", model);
        }

        [HttpPost, ActionName("UserDelete")]
        [ValidateAntiForgeryToken]
        [AdminOnly]
        public ActionResult DeleteConfirmed(string id)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.FirstOrDefault(u => u.Id == id);
            Db.Users.Remove(user);
            Db.SaveChanges();
            return RedirectToAction("Users");
        }

        [AdminOnly]
        public ActionResult UserRoles(string id)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.FirstOrDefault(u => u.Id == id);
            var model = new SelectUserRolesViewModel(user);
            return View("UserView/UserRoles", model);
        }

        [HttpPost]
        [AdminOnly]
        [ValidateAntiForgeryToken]
        public ActionResult UserRoles(SelectUserRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var idManager = new IdentityManager();
                var Db = new ApplicationDbContext();
                var user = Db.Users.FirstOrDefault(u => u.Id == model.Id);
                idManager.ClearUserRoles(user.Id);
                foreach (var role in model.Roles)
                {
                    if (role.Selected)
                    {
                        idManager.AddUserToRole(user.Id, role.RoleName);
                    }
                }
                return RedirectToAction("Users");
            }
            return View();
        }

        [AdminOnly]
        public ActionResult UserLayer(string id)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.FirstOrDefault(u => u.Id == id);
            var layers = Db.LayerViewDbSet.ToList();
            LayerTypeWrapper layer = LayerLogic.GetLayerWrapper(layers, user);
            return View("UserView/UserLayer", layer);
        }

        [HttpPost]
        [AdminOnly]
        [ValidateAntiForgeryToken]
        public ActionResult UserLayer(LayerTypeWrapper model)
        {
            if (ModelState.IsValid)
            {
                var idManager = new IdentityManager();
                var Db = new ApplicationDbContext();
                var user = Db.Users.FirstOrDefault(u => u.Id == model.UserId);
                var layer = Db.LayerViewDbSet.ToList();
                var selected = layer.FirstOrDefault(x => x.Id == Int32.Parse(model.RadioButtons.SelectedValue));

                if (selected != null)
                {
                    user.Layer = selected;
                    Db.SaveChanges();
                }
                else
                {
                    user.Layer = null;
                    Db.SaveChanges();
                }

                return RedirectToAction("Users");
            }
            return View();
        }


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        #endregion

        #region AdminLayerElement

        public ViewResult LayerElements(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var elements = DbContext.LayerElementDbSet.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                elements = elements.Where(s => s.ElementName != null && s.ElementName.ToUpper().Contains(searchString.ToUpper())).ToList();
            }
            switch (sortOrder)
            {
                case "name_desc":
                    elements = elements.OrderByDescending(s => s.ElementName).ToList();
                    break;
                default:  // Name ascending 
                    elements = elements.OrderBy(s => s.ElementName).ToList();
                    break;
            }

            int pageSize = pageElement;
            int pageNumber = (page ?? 1);
            return View("LayerElement/LayerElements", elements.ToPagedList(pageNumber, pageSize));
        }

        public ViewResult LayerElementCreate()
        {
            return View("LayerElement/LayerElementCreate");
        }

        [HttpPost]
        public async Task<ActionResult> LayerElementCreate(LayerElement model)
        {
            if (ModelState.IsValid)
            {
                DbContext.LayerElementDbSet.Add(model);
                DbContext.SaveChanges();
                return RedirectToAction("LayerElements");
            }

            return View("LayerElement/LayerElementCreate", model);
        }

        [AdminOnly]
        public ActionResult LayerElementDelete(int id)
        {
            var Db = new ApplicationDbContext();
            var layer = Db.LayerElementDbSet.FirstOrDefault(u => u.Id == id);
            if (layer == null)
            {
                return HttpNotFound();
            }
            return View("LayerElement/LayerElementDelete", layer);
        }

        [HttpPost, ActionName("LayerElementDelete")]
        [ValidateAntiForgeryToken]
        [AdminOnly]
        public ActionResult LayerElementDeleteConfirm(int id)
        {
            var Db = new ApplicationDbContext();
            var layer = Db.LayerElementDbSet.FirstOrDefault(u => u.Id == id);
            Db.LayerElementDbSet.Remove(layer);
            Db.SaveChanges();
            return RedirectToAction("LayerElements");
        }
        #region layerElementEditDetails
        public ViewResult LayerElementEdit(int id)
        {
            return View("LayerElement/LayerElementEdit", DbContext.LayerElementDbSet.FirstOrDefault(x => x.Id == id));
        }

        [HttpPost]
        public async Task<ActionResult> LayerElementEdit(LayerElement model)
        {
            if (ModelState.IsValid)
            {
                var elem = DbContext.LayerElementDbSet.FirstOrDefault(x => x.Id == model.Id);
                elem.ElementName = model.ElementName;
                elem.Information = model.Information;

                DbContext.SaveChanges();
                return RedirectToAction("LayerElements");
            }

            return View("LayerElement/LayerElementEdit", model);
        }

        public ViewResult LayerElementDetails(int id)
        {
            return View("LayerElement/LayerElementDetails", DbContext.LayerElementDbSet.FirstOrDefault(x => x.Id == id));
        }

        #endregion

        #endregion

        #region AdminLayersView

        public ViewResult LayerViews(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var elements = DbContext.LayerViewDbSet.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                elements = elements.Where(s => s.LayerName != null && s.LayerName.ToUpper().Contains(searchString.ToUpper())).ToList();
            }
            switch (sortOrder)
            {
                case "name_desc":
                    elements = elements.OrderByDescending(s => s.LayerName).ToList();
                    break;
                default:  // Name ascending 
                    elements = elements.OrderBy(s => s.LayerName).ToList();
                    break;
            }

            int pageSize = pageElement;
            int pageNumber = (page ?? 1);
            return View("LayerView/LayerViews", elements.ToPagedList(pageNumber, pageSize));
        }

        #region layerEditDetails
        public ViewResult LayerViewEdit(int id)
        {
            return View("LayerView/LayerViewEdit", LayerLogic.GetLayerView(id));
        }

        [HttpPost]
        public async Task<ActionResult> LayerViewEdit(LayerViewWrapper model)
        {
            if (ModelState.IsValid)
            {
                var toSave = LayerLogic.GetPostedService(model);

                var lay = DbContext.LayerViewDbSet.FirstOrDefault(x => x.Id == model.Id);
                lay.LayerName = toSave.Name;
                lay.LayerInformation = toSave.Information;
                lay.LayerElements.Clear();
                foreach (LayerModel lmod in toSave.SelectedElement)
                {
                    lay.LayerElements.Add(DbContext.LayerElementDbSet.First(x => x.Id == lmod.Id));
                }
                DbContext.Entry(lay).State = EntityState.Modified;
                DbContext.SaveChanges();
                return RedirectToAction("LayerViews");
            }

            return View("LayerView/LayerViewCreate", model);
        }

        public ViewResult LayerViewDetails(int id)
        {
            return View("LayerView/LayerViewDetails", LayerLogic.GetLayerView(id));
        }

        #endregion
        #region layerCreate

        public ViewResult LayerViewCreate()
        {
            return View("LayerView/LayerViewCreate", LayerLogic.GetLayerView());
        }

        [HttpPost]
        public async Task<ActionResult> LayerViewCreate(LayerViewWrapper model)
        {
            if (ModelState.IsValid)
            {
                var toSave = LayerLogic.GetPostedService(model);
                LayerView layerView = new LayerView()
                {
                    LayerName = toSave.Name,
                    LayerInformation = toSave.Information,
                    LayerElements = new Collection<LayerElement>()
                };
                foreach (LayerModel lmod in toSave.SelectedElement)
                {
                    layerView.LayerElements.Add(DbContext.LayerElementDbSet.First(x => x.Id == lmod.Id));
                }
                DbContext.LayerViewDbSet.Add(layerView);
                DbContext.SaveChanges();
                return RedirectToAction("LayerViews");
            }

            return View("LayerView/LayerViewCreate", model);
        }

        #endregion
        #region layerDelete

        [AdminOnly]
        public ActionResult LayerViewDelete(int id)
        {
            var layer = DbContext.LayerViewDbSet.FirstOrDefault(u => u.Id == id);
            if (layer == null)
            {
                return HttpNotFound();
            }
            return View("LayerView/LayerViewDelete", layer);
        }

        [HttpPost, ActionName("LayerViewDelete")]
        [ValidateAntiForgeryToken]
        [AdminOnly]
        public ActionResult LayerViewDeleteConfirm(int id)
        {
            var layer = DbContext.LayerViewDbSet.FirstOrDefault(u => u.Id == id);

            DbContext.LayerViewDbSet.Remove(layer);
            DbContext.SaveChanges();
            return RedirectToAction("LayerViews");
        }
        #endregion
        #endregion
    }
}
