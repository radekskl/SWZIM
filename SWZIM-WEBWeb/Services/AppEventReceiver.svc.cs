using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.EventReceivers;

namespace SWZIM_WEBWeb.Services
{
    public class AppEventReceiver : IRemoteEventService
    {
        /// <summary>
        /// Handles app events that occur after the app is installed or upgraded, or when app is being uninstalled.
        /// </summary>
        /// <param name="properties">Holds information about the app event.</param>
        /// <returns>Holds information returned from the app event.</returns>
        public SPRemoteEventResult ProcessEvent(SPRemoteEventProperties properties)
        {
            SPRemoteEventResult result = new SPRemoteEventResult();

            using (ClientContext clientContext = TokenHelper.CreateAppEventClientContext(properties, useAppWeb: false))
            {
                switch(properties.EventType){
                    case SPRemoteEventType.AppInstalled:
                        CreateGroups(clientContext);
                        break;

                    default:
                        break;
                }

                
            }

            return result;
        }

        /// <summary>
        /// This method is a required placeholder, but is not used by app events.
        /// </summary>
        /// <param name="properties">Unused.</param>
        public void ProcessOneWayEvent(SPRemoteEventProperties properties)
        {
            throw new NotImplementedException();
        }

        public bool CreateGroups(ClientContext ctx)
        {
            try
            {
                if (ctx != null)
                {
                    User spUser = ctx.Web.CurrentUser;
                    ctx.Load(ctx.Web);
                    ctx.Load(spUser, user => user.Title, user => user.Id, user => user.Email);
                    ctx.ExecuteQuery();

                    // Create Group
                    var groupCreationInfo = new GroupCreationInformation();
                    groupCreationInfo.Title = "aaa";
                    groupCreationInfo.Description = "This is a new Collaboration Group";
                    Group oGroup = ctx.Web.SiteGroups.Add(groupCreationInfo);
                    ctx.Load(oGroup);
                    ctx.ExecuteQuery();
                    oGroup.Owner = spUser;
                    oGroup.Update();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }
}
