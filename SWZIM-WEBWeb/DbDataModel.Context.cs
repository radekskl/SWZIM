﻿

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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class SWZIM_dbEntities : DbContext
{
    public SWZIM_dbEntities()
        : base("name=SWZIM_dbEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<Notifications> Notifications { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    public virtual DbSet<Groups> Groups { get; set; }

    public virtual DbSet<MarkerIcons> MarkerIcons { get; set; }

    public virtual DbSet<Layers> Layers { get; set; }

    public virtual DbSet<Events> Events { get; set; }

    public virtual DbSet<EventTypes> EventTypes { get; set; }

    public virtual DbSet<LayoutElementTypes> LayoutElementTypes { get; set; }

    public virtual DbSet<LayoutElements> LayoutElements { get; set; }

}

}

