namespace Cloud_based_editor_VLN_2.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class NameProject : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Documents", new[] { "ProjectID" });
            AlterColumn("dbo.Documents", "Name", c => c.String(maxLength: 400, nullable: false));
            AlterColumn("dbo.Projects", "Name", c => c.String(nullable: false));
            CreateIndex("dbo.Documents", new[] { "Name", "ProjectID" }, unique: true, name: "NameProject");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Documents", "NameProject");
            AlterColumn("dbo.Projects", "Name", c => c.String());
            AlterColumn("dbo.Documents", "Name", c => c.String());
            CreateIndex("dbo.Documents", "ProjectID");
        }
    }
}
