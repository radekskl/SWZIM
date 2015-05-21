using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WpieSerwisInz.Models;

namespace WpieSerwisInz.Logic.BusinessLogic
{
    public static class UserLogic
    {
        public static bool IsLayerForUser(string id)
        {
            using (ApplicationDbContext DbContext = new ApplicationDbContext())
            {
                var users = DbContext.Users.ToList();
                var user = users.FirstOrDefault(x => x.Id == id);
                return (user != null && user.Layer != null);
            }
        }
    }
}