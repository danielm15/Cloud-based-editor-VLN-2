namespace Cloud_based_editor_VLN_2.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class LastUpdatedBy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "LastUpdatedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "LastUpdatedBy");
        }
    }
}
