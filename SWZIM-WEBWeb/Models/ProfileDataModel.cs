using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SWZIM_WEBWeb.Models
{
    public class ProfileDataModel
    {
        public string PreferredName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalSpace { get; set; }
        public string UserName { get; set; }
        public string PictureUrl { get; set; }
        public List<Groups> UserGroup { get; set; }

        public Users User { get; set; }


        public ProfileDataModel() { }
    }
}