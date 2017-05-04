namespace Cloud_based_editor_VLN_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedBy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "CreatedBy", c => c.String(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "CreatedBy");
        }
    }
}
