namespace Cloud_based_editor_VLN_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateNameProject : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Documents", "NameProject");
            AlterColumn("dbo.Documents", "Type", c => c.String(maxLength: 400));
            CreateIndex("dbo.Documents", new[] { "Name", "Type", "ProjectID" }, unique: true, name: "NameProject");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Documents", "NameProject");
            AlterColumn("dbo.Documents", "Type", c => c.String());
            CreateIndex("dbo.Documents", new[] { "Name", "ProjectID" }, unique: true, name: "NameProject");
        }
    }
}
