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
    using System.ComponentModel;
    
    public partial class MarkerIcons
    {
        public MarkerIcons()
        {
            this.EventTypes = new HashSet<EventTypes>();
            this.LayoutElementTypes = new HashSet<LayoutElementTypes>();
        }

        public int Id { get; set; }
        [DisplayName("Nazwa")]
        public string Name { get; set; }
        [DisplayName("Adres URL")]
        public string Url { get; set; }
    
        public virtual ICollection<EventTypes> EventTypes { get; set; }
        public virtual ICollection<LayoutElementTypes> LayoutElementTypes { get; set; }
    }
}
