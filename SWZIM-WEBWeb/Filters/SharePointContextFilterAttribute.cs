using Microsoft.SharePoint.Client;
using System;
using System.Web;
using System.Web.Mvc;

namespace SWZIM_WEBWeb
{
    /// <summary>
    /// SharePoint action filter attribute.
    /// </summary>
    public class SharePointContextFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            Uri redirectUrl;
            switch (SharePointContextProvider.CheckRedirectionStatus(filterContext.HttpContext, out redirectUrl))
            {
                case RedirectionStatus.Ok:
                    GetSPUserDetails(filterContext.HttpContext, filterContext.Controller.ViewBag);
                    return;
                case RedirectionStatus.ShouldRedirect:
                    filterContext.Result = new RedirectResult(redirectUrl.AbsoluteUri);
                    break;
                case RedirectionStatus.CanNotRedirect:
                    filterContext.Result = new ViewResult { ViewName = "Error" };
                    break;
            }
        }

        private void GetSPUserDetails(HttpContextBase httpContextBase, dynamic viewBag)
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(httpContextBase);
            viewBag.SPHostUrl = spContext.SPHostUrl.AbsoluteUri.TrimEnd('/');
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    User spUser = clientContext.Web.CurrentUser;
                    clientContext.Load(spUser, user => user.Title, user => user.Id, user => user.Email);
                    clientContext.ExecuteQuery();

                    viewBag.UserName = spUser.Title;
                    viewBag.UserId = spUser.Id;
                    viewBag.UserEmail = spUser.Email;

                    if (viewBag.UserId != null && viewBag.UserId > 0)
                    {
                        EnsureUserCreated(viewBag.UserId, viewBag.UserName, viewBag.UserEmail);
                    }
                }
            }
        }

        private void EnsureUserCreated(int userId, string userName, string userEmail)
        {
            //TODO: database connection, check and save user
            using (var context = new SWZIM_dbEntities())
            {
                var user = context.Users.Find(userId);
                if (user != null)
                {
                    if (userEmail != user.Email)
                        user.Email = userEmail;
                    if (userName != user.UserName)
                        user.UserName = userName;
                    context.SaveChanges();
                }
                else
                {
                    Users u = new Users();
                    u.ID = userId;
                    u.Email = userEmail;
                    u.UserName = userName;
                    u.GroupId = 1;
                    context.Users.Add(u);
                    context.SaveChanges();
                }
            }
        }

    }
}
