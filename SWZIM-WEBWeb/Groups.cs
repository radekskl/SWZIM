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
    
    public partial class Groups
    {
        public Groups()
        {
            this.Users = new HashSet<Users>();
            this.Layers = new HashSet<Layers>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Users> Users { get; set; }
        public virtual ICollection<Layers> Layers { get; set; }
    }
}
