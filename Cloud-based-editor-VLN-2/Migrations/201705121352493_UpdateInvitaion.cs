namespace Cloud_based_editor_VLN_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateInvitaion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invitations", "fromUserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invitations", "fromUserName");
        }
    }
}
