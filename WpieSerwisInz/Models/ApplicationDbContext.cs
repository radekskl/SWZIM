using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using WpieSerwisInz.Models.BO;

namespace WpieSerwisInz.Models
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DatabaseConnection")
        {
            Database.SetInitializer<ApplicationDbContext>(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }

        public DbSet<EventModel> EventModelDbSet { get; set; }
        public DbSet<EventType> EventTypeDbSet { get; set; }
        public DbSet<JunctionModel> JunctionModelDbSet { get; set; }
        public DbSet<JunctionXml> JunctionXmlDbSet { get; set; }
        public DbSet<LayerElement> LayerElementDbSet { get; set; }
        public DbSet<LayerView> LayerViewDbSet { get; set; }
    }

}