namespace Cloud_based_editor_VLN_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Invitaion : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "ProjectType", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "ProjectType", c => c.String());
        }
    }
}
