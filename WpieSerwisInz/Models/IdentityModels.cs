using System;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using WpieSerwisInz.Models.BO;
using Microsoft.AspNet.Identity;
using WpieSerwisInz.Models.BO.Dictionary;

namespace WpieSerwisInz.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ModificationDate { get; set; }

        public virtual LayerView Layer { get; set; }
        public virtual ICollection<JunctionModel> Junction { get; set; }
        public virtual ICollection<JunctionXml> JunctionXml { get; set; }
        public virtual ICollection<EventModel> EventModels { get; set; }

        public ApplicationUser()
        {
            ModificationDate = DateTime.Now;
        }
    }

    public class IdentityManager
    {

        public bool RoleExists(string name)
        {
            var rm = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(new ApplicationDbContext()));
            return rm.RoleExists(name);
        }

        public bool CreateRole(string name)
        {
            var rm = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var idResult = rm.Create(new IdentityRole(name));
            return idResult.Succeeded;
        }

        public bool CreateUser(ApplicationUser user, string password)
        {
            var um = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var idResult = um.Create(user, password);
            return idResult.Succeeded;
        }

        public bool UserIsInRole(string userId, string roleName)
        {
            var um = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(new ApplicationDbContext()));
            return um.IsInRole(userId, roleName);
        }

        public bool AddUserToRole(string userId, string roleName)
        {
            var um = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ExtendedUserRoles.ClearUserUnconfirmedRole(userId);
            var idResult = um.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }

        public void ClearUserRoles(string userId)
        {
            var um = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = um.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();
            currentRoles.AddRange(user.Roles);
            foreach (var role in currentRoles)
            {
                um.RemoveFromRole(userId, role.Role.Name);
            }
        }
    }

    public static class ExtendedUserRoles
    {

        public static string GetUserIdByName(string userName)
        {
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUser appUser = UserManager.FindByName(userName);
            return appUser.Id;
        }

        public static void RegisterUserAsUnconfirmed(string userName)
        {
            IdentityManager im = new IdentityManager();
            im.AddUserToRole(GetUserIdByName(userName), RoleType.Unconfirmed);
        }

        public static string GetRoleById(string id)
        {
            var rm = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(new ApplicationDbContext()));
            return rm.FindById(id).Name;
        }

        public static void ClearUserUnconfirmedRole(string userId)
        {
            var um = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = um.FindById(userId);
            um.RemoveFromRole(userId, RoleType.Unconfirmed);
        }

        public static void ActivateUser(string userId)
        {
            IdentityManager im = new IdentityManager();
            ClearUserUnconfirmedRole(userId);
            var um = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = um.FindById(userId);
            im.AddUserToRole(GetUserIdByName(user.UserName), RoleType.Confirmed);
        }
    }

}