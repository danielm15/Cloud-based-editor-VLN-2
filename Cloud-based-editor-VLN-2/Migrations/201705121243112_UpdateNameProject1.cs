namespace Cloud_based_editor_VLN_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateNameProject1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Documents", "NameProject");
            AlterColumn("dbo.Documents", "Name", c => c.String(maxLength: 300));
            AlterColumn("dbo.Documents", "Type", c => c.String(maxLength: 100));
            CreateIndex("dbo.Documents", new[] { "Name", "Type", "ProjectID" }, unique: true, name: "NameProject");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Documents", "NameProject");
            AlterColumn("dbo.Documents", "Type", c => c.String(maxLength: 400));
            AlterColumn("dbo.Documents", "Name", c => c.String(maxLength: 400));
            CreateIndex("dbo.Documents", new[] { "Name", "Type", "ProjectID" }, unique: true, name: "NameProject");
        }
    }
}
