namespace Cloud_based_editor_VLN_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Invitaion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invitations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AppUserID = c.Int(nullable: false),
                        ProjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AppUsers", t => t.AppUserID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.AppUserID)
                .Index(t => t.ProjectID);
            
            AlterColumn("dbo.Projects", "ProjectType", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invitations", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.Invitations", "AppUserID", "dbo.AppUsers");
            DropIndex("dbo.Invitations", new[] { "ProjectID" });
            DropIndex("dbo.Invitations", new[] { "AppUserID" });
            AlterColumn("dbo.Projects", "ProjectType", c => c.String());
            DropTable("dbo.Invitations");
        }
    }
}
