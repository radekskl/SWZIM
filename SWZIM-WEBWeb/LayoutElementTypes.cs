
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
    
public partial class LayoutElementTypes
{

    public LayoutElementTypes()
    {

        this.LayoutElements = new HashSet<LayoutElements>();

    }


    public int Id { get; set; }

    public string Name { get; set; }

    public Nullable<int> MarkerIconId { get; set; }



    public virtual ICollection<LayoutElements> LayoutElements { get; set; }

    public virtual MarkerIcons MarkerIcons { get; set; }

}

}
