namespace Cloud_based_editor_VLN_2.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ProjectType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "ProjectType", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "ProjectType");
        }
    }
}
