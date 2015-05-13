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

            if (filterContext.HttpContext.Request.QueryString["SPHostUrl"] == null)
            {
                var spHostUrl = SharePointContext.GetSPHostUrl(filterContext.HttpContext.Request, filterContext.HttpContext);
                if (spHostUrl != null)
                    SharePointContextProvider.BookmarkifyUrl(filterContext, spHostUrl.AbsoluteUri);
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
                    clientContext.Load(spUser, user => user.Title, user => user.Id);
                    clientContext.ExecuteQuery();

                    viewBag.UserName = spUser.Title;
                    viewBag.UserId = spUser.Id;
                }
            }
        }
    }
}
