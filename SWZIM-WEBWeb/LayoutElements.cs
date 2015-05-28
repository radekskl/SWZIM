
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
    
public partial class LayoutElements
{

    public LayoutElements()
    {

        this.LayoutElements1 = new HashSet<LayoutElements>();

        this.LayoutElements2 = new HashSet<LayoutElements>();

    }


    public int Id { get; set; }

    [DisplayName("Nazwa")]
    public string Name { get; set; }

    [DisplayName("Opis")]
    public string Description { get; set; }

    [DisplayName("Dl. geo")]
    public decimal Longitude { get; set; }

    [DisplayName("Szer.geo.")]
    public decimal Latitude { get; set; }

    [DisplayName("Typ")]
    public int LayoutElementTypeId { get; set; }

    [DisplayName("Użytkownik")]
    public int UserId { get; set; }

    [DisplayName("Warstwa")]
    public int LayersId { get; set; }



    public virtual LayoutElementTypes LayoutElementTypes { get; set; }

    public virtual Users Users { get; set; }

    public virtual ICollection<LayoutElements> LayoutElements1 { get; set; }

    public virtual ICollection<LayoutElements> LayoutElements2 { get; set; }

    public virtual Layers Layers { get; set; }

}

}
