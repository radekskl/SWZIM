//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SWZIM_WEBWeb
{
    using System;
    using System.Collections.Generic;
    
    public partial class Users
    {
        public Users()
        {
            this.Notifications = new HashSet<Notifications>();
            this.Events = new HashSet<Events>();
            this.LayoutElements = new HashSet<LayoutElements>();
        }
    
        public int ID { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int GroupId { get; set; }
    
        public virtual ICollection<Notifications> Notifications { get; set; }
        public virtual Groups Groups { get; set; }
        public virtual ICollection<Events> Events { get; set; }
        public virtual ICollection<LayoutElements> LayoutElements { get; set; }
    }
}
