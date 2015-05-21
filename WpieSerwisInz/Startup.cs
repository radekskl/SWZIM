
// <author>Wojciech Piechocinski</author>
// <date>28/01/2015 10:00:00 AM</date>

using Microsoft.Owin;
using Owin;
using WpieSerwisInz.Models;

[assembly: OwinStartupAttribute(typeof(WpieSerwisInz.Startup))]
namespace WpieSerwisInz
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ApplicationDbContext db = new ApplicationDbContext();
            db.Database.CreateIfNotExists();
        }
    }
}
