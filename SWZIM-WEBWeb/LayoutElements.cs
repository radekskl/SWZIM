
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
    
public partial class LayoutElements
{

    public LayoutElements()
    {

        this.LayoutElements1 = new HashSet<LayoutElements>();

        this.LayoutElements2 = new HashSet<LayoutElements>();

    }


    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Longitude { get; set; }

    public decimal Latitude { get; set; }

    public int LayoutElementTypeId { get; set; }

    public int UserId { get; set; }

    public int LayersId { get; set; }



    public virtual LayoutElementTypes LayoutElementTypes { get; set; }

    public virtual Users Users { get; set; }

    public virtual ICollection<LayoutElements> LayoutElements1 { get; set; }

    public virtual ICollection<LayoutElements> LayoutElements2 { get; set; }

    public virtual Layers Layers { get; set; }

}

}
