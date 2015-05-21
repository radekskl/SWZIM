using System.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WpieSerwisInz.Models;
using WpieSerwisInz.Models.BO;
using WpieSerwisInz.Models.BO.Dictionary;

namespace WpieSerwisInz.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WpieSerwisInz.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(WpieSerwisInz.Models.ApplicationDbContext context)
        {
            ApplicationDbContext DbContext = new ApplicationDbContext();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            IdentityManager rolesManager = new IdentityManager();

            if (!rolesManager.RoleExists(RoleType.Admin))
            {
                rolesManager.CreateRole(RoleType.Admin);
            }

            if (!rolesManager.RoleExists(RoleType.Developer))
            {
                rolesManager.CreateRole(RoleType.Developer);
            }

            if (!rolesManager.RoleExists(RoleType.Confirmed))
            {
                rolesManager.CreateRole(RoleType.Confirmed);
            }

            if (!rolesManager.RoleExists(RoleType.Unconfirmed))
            {
                rolesManager.CreateRole(RoleType.Unconfirmed);
            }

            var userAdmin = new ApplicationUser()
            {
                UserName = "TestAdmin",
                CreateDate = DateTime.Now,
                ModificationDate = DateTime.Now
            };

            manager.Create(userAdmin, "lolek1");
            rolesManager.AddUserToRole(ExtendedUserRoles.GetUserIdByName(userAdmin.UserName),
                RoleType.Admin);

            var user = new ApplicationUser()
            {
                UserName = "TestUser",
                CreateDate = DateTime.Now,
                ModificationDate = DateTime.Now
            };

            manager.Create(user, "lolek1");
            rolesManager.AddUserToRole(ExtendedUserRoles.GetUserIdByName(user.UserName),
                RoleType.Confirmed);

            EventType ev1 = new EventType()
            {
                AssignedImage = "AccidentImage",
                EventName = "Accident",
                EventInformation = "Event for road accidents"
            };

            if (!DbContext.EventTypeDbSet.Any(x => x.EventName == ev1.EventName))
            {
                DbContext.EventTypeDbSet.Add(ev1);
            }

            EventType ev2 = new EventType()
            {
                AssignedImage = "WorkImage",
                EventName = "Work on road",
                EventInformation = "Event for work on road"
            };

            if (!DbContext.EventTypeDbSet.Any(x => x.EventName == ev1.EventName))
            {
                DbContext.EventTypeDbSet.Add(ev2);
            }

            EventType ev3 = new EventType()
            {
                AssignedImage = "PublicEImage",
                EventName = "Public Event",
                EventInformation = "Event for public Event"
            };

            if (!DbContext.EventTypeDbSet.Any(x => x.EventName == ev1.EventName))
            {
                DbContext.EventTypeDbSet.Add(ev3);
            }

            EventType ev4 = new EventType()
            {
            AssignedImage = "OtherImage",
            EventName = "Other",
            EventInformation = "Event for public Event"
            };

            if (!DbContext.EventTypeDbSet.Any(x => x.EventName == ev1.EventName))
            {
                DbContext.EventTypeDbSet.Add(ev4);
            }
            DbContext.SaveChanges();
        }
    }
}
