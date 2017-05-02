namespace Cloud_based_editor_VLN_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppUsers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Documents",
                c => new {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    Type = c.String(),
                    DateCreated = c.DateTime(nullable: true),
                    ProjectID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: false)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OwnerID = c.Int(nullable: false),
                        Name = c.String(),
                        DateCreated = c.DateTime(nullable: true),
                        StartUpFileID = c.Int(nullable: true),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AppUsers", t => t.OwnerID, cascadeDelete: false)
                .ForeignKey("dbo.Documents", t => t.StartUpFileID)
                .Index(t => t.OwnerID)
                .Index(t => t.StartUpFileID);
            
            CreateTable(
                "dbo.UserProjects",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AppUserID = c.Int(nullable: false),
                        ProjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AppUsers", t => t.AppUserID, cascadeDelete: false)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: false)
                .Index(t => t.AppUserID)
                .Index(t => t.ProjectID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProjects", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.UserProjects", "AppUserID", "dbo.AppUsers");
            DropForeignKey("dbo.Documents", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.Projects", "StartUpFileID", "dbo.Documents");
            DropForeignKey("dbo.Projects", "OwnerID", "dbo.AppUsers");
            DropIndex("dbo.UserProjects", new[] { "ProjectID" });
            DropIndex("dbo.UserProjects", new[] { "AppUserID" });
            DropIndex("dbo.Projects", new[] { "StartUpFileID" });
            DropIndex("dbo.Projects", new[] { "OwnerID" });
            DropIndex("dbo.Documents", new[] { "ProjectID" });
            DropTable("dbo.UserProjects");
            DropTable("dbo.Projects");
            DropTable("dbo.Documents");
            DropTable("dbo.AppUsers");
        }
    }
}
