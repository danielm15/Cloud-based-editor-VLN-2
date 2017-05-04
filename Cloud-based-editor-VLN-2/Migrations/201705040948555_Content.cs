namespace Cloud_based_editor_VLN_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Content : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "Content", c => c.String());
            AlterColumn("dbo.Documents", "DateCreated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Projects", "DateCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "DateCreated", c => c.String());
            AlterColumn("dbo.Documents", "DateCreated", c => c.String());
            DropColumn("dbo.Documents", "Content");
        }
    }
}
