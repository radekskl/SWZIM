using Microsoft.SharePoint.Client.UserProfiles;
using SWZIM_WEBWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWZIM_WEBWeb.Controllers
{
    public class ProfileController : Controller
    {
        private SWZIM_dbEntities db = new SWZIM_dbEntities();

        // GET: Profile
        [SharePointContextFilter]
        public ActionResult Index()
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    // Get the people manager instance and load current properties.
                    PeopleManager peopleManager = new PeopleManager(clientContext);
                    PersonProperties personProperties = peopleManager.GetMyProperties();
                    clientContext.Load(personProperties);
                    clientContext.ExecuteQuery();
                    ProfileDataModel model = new ProfileDataModel();
                    //fill desired properties
                    model.FirstName = personProperties.UserProfileProperties["FirstName"];
                    model.LastName = personProperties.UserProfileProperties["LastName"];
                    model.PersonalSpace = personProperties.UserProfileProperties["PersonalSpace"];
                    model.UserName = personProperties.UserProfileProperties["UserName"];
                    model.PictureUrl = string.Format("https://outlook.office365.com/api/beta/Users('{0}')/UserPhoto/$value", model.UserName);
                    model.PreferredName = personProperties.UserProfileProperties["PreferredName"];

                    Users user = db.Users.Find(ViewBag.UserId);
                    if (user != null)
                    {
                        if (user.Groups != null)
                        {
                            model.UserGroup = user.Groups;
                        }
                    }
                    return View(model);
                }
            }

            return RedirectToAction("Error", "Home", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }
    }
}