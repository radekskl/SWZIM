using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SWZIM_WEBWeb.Helpers
{
    public class GroupsHelper
    {
        public static bool CreateGroups(ClientContext ctx)
        {
            try
            {
                if (ctx != null)
                {
                    User spUser = ctx.Web.CurrentUser;
                    ctx.Load(ctx.Web);
                    ctx.Load(ctx.Web.SiteGroups);
                    ctx.Load(spUser, user => user.Title, user => user.Id, user => user.Email);
                    ctx.ExecuteQuery();

                    ctx.Web.BreakRoleInheritance(false, false);
                    ctx.ExecuteQuery();
                    Group ownerGroup = default(Group); Group memberGroup = default(Group); Group visitorGroup = default(Group);

                    foreach (var groupItem in Consts.Enumerations.Groups)
	                {
		                //if (!(ctx.Web.SiteGroups.Count(x => x.Title == groupItem.Title) == 0)){
                            // Create Group
                            var groupCreationInfo = new GroupCreationInformation();
                            groupCreationInfo.Title = groupItem.Title;
                            groupCreationInfo.Description = groupItem.Description;
                            Group oGroup = ctx.Web.SiteGroups.Add(groupCreationInfo);
                            ctx.Load(oGroup);
                            ctx.ExecuteQuery();
                            oGroup.Owner = spUser;
                            oGroup.Update();
                        //}
	                }
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