namespace WpieSerwisInz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventName = c.String(),
                        EventInformation = c.String(),
                        EventStatus = c.Int(nullable: false),
                        Lat = c.String(),
                        Long = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        LastModificationDate = c.DateTime(nullable: false),
                        Author_Id = c.String(maxLength: 128),
                        EventType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .ForeignKey("dbo.EventTypes", t => t.EventType_Id)
                .Index(t => t.Author_Id)
                .Index(t => t.EventType_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        LayerView_Id = c.Int(),
                        Layer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LayerViews", t => t.LayerView_Id)
                .ForeignKey("dbo.LayerViews", t => t.Layer_Id)
                .Index(t => t.LayerView_Id)
                .Index(t => t.Layer_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LayerViews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LayerName = c.String(),
                        LayerInformation = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LayerElements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ElementName = c.String(),
                        Information = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EventTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventName = c.String(),
                        EventInformation = c.String(),
                        AssignedImage = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JunctionModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Information = c.String(),
                        IsAccepted = c.Boolean(nullable: false),
                        Lat = c.String(),
                        Long = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        LastModificationDate = c.DateTime(nullable: false),
                        Author_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .Index(t => t.Author_Id);
            
            CreateTable(
                "dbo.JunctionXmls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SerializedXmlModel = c.String(),
                        AdditionalInformation = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        LastModificationDate = c.DateTime(nullable: false),
                        Author_Id = c.String(maxLength: 128),
                        Junctiom_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .ForeignKey("dbo.JunctionModels", t => t.Junctiom_Id)
                .Index(t => t.Author_Id)
                .Index(t => t.Junctiom_Id);
            
            CreateTable(
                "dbo.LayerElementEventModels",
                c => new
                    {
                        LayerElement_Id = c.Int(nullable: false),
                        EventModel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LayerElement_Id, t.EventModel_Id })
                .ForeignKey("dbo.LayerElements", t => t.LayerElement_Id, cascadeDelete: true)
                .ForeignKey("dbo.EventModels", t => t.EventModel_Id, cascadeDelete: true)
                .Index(t => t.LayerElement_Id)
                .Index(t => t.EventModel_Id);
            
            CreateTable(
                "dbo.LayerElementLayerViews",
                c => new
                    {
                        LayerElement_Id = c.Int(nullable: false),
                        LayerView_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LayerElement_Id, t.LayerView_Id })
                .ForeignKey("dbo.LayerElements", t => t.LayerElement_Id, cascadeDelete: true)
                .ForeignKey("dbo.LayerViews", t => t.LayerView_Id, cascadeDelete: true)
                .Index(t => t.LayerElement_Id)
                .Index(t => t.LayerView_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JunctionXmls", "Junctiom_Id", "dbo.JunctionModels");
            DropForeignKey("dbo.JunctionXmls", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.JunctionModels", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.EventModels", "EventType_Id", "dbo.EventTypes");
            DropForeignKey("dbo.EventModels", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Layer_Id", "dbo.LayerViews");
            DropForeignKey("dbo.AspNetUsers", "LayerView_Id", "dbo.LayerViews");
            DropForeignKey("dbo.LayerElementLayerViews", "LayerView_Id", "dbo.LayerViews");
            DropForeignKey("dbo.LayerElementLayerViews", "LayerElement_Id", "dbo.LayerElements");
            DropForeignKey("dbo.LayerElementEventModels", "EventModel_Id", "dbo.EventModels");
            DropForeignKey("dbo.LayerElementEventModels", "LayerElement_Id", "dbo.LayerElements");
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.JunctionXmls", new[] { "Junctiom_Id" });
            DropIndex("dbo.JunctionXmls", new[] { "Author_Id" });
            DropIndex("dbo.JunctionModels", new[] { "Author_Id" });
            DropIndex("dbo.EventModels", new[] { "EventType_Id" });
            DropIndex("dbo.EventModels", new[] { "Author_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Layer_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "LayerView_Id" });
            DropIndex("dbo.LayerElementLayerViews", new[] { "LayerView_Id" });
            DropIndex("dbo.LayerElementLayerViews", new[] { "LayerElement_Id" });
            DropIndex("dbo.LayerElementEventModels", new[] { "EventModel_Id" });
            DropIndex("dbo.LayerElementEventModels", new[] { "LayerElement_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropTable("dbo.LayerElementLayerViews");
            DropTable("dbo.LayerElementEventModels");
            DropTable("dbo.JunctionXmls");
            DropTable("dbo.JunctionModels");
            DropTable("dbo.EventTypes");
            DropTable("dbo.LayerElements");
            DropTable("dbo.LayerViews");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.EventModels");
        }
    }
}
