using Microsoft.SharePoint.Client.UserProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWZIM_WEBWeb.Controllers
{
    public class ProfileController : Controller
    {
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
                    return View(personProperties.UserProfileProperties);
                }
            }

            return RedirectToAction("Error", "Home", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }
    }
}