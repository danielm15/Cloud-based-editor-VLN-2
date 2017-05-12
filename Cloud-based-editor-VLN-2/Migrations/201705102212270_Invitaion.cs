namespace Cloud_based_editor_VLN_2.Migrations {
	using System.Data.Entity.Migrations;

	public partial class Invitaion : DbMigration {
		public override void Up() {
			CreateTable(
					"dbo.Invitations",
					c => new
					{
						ID = c.Int(nullable: false, identity: true),
						AppUserID = c.Int(nullable: false),
						ProjectID = c.Int(nullable: false),
					})
				.PrimaryKey(t => t.ID)
				.ForeignKey("dbo.AppUsers", t => t.AppUserID, cascadeDelete: false)
				.ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: false)
				.Index(t => t.AppUserID)
				.Index(t => t.ProjectID);

			AlterColumn("dbo.Documents", "Name", c => c.String(maxLength: 400, nullable: false));
		}

		public override void Down() {
			DropForeignKey("dbo.Invitations", "ProjectID", "dbo.Projects");
			DropForeignKey("dbo.Invitations", "AppUserID", "dbo.AppUsers");
			DropIndex("dbo.Invitations", new[] { "ProjectID" });
			DropIndex("dbo.Invitations", new[] { "AppUserID" });
			AlterColumn("dbo.Projects", "ProjectType", c => c.String());
			DropTable("dbo.Invitations");
		}
	}
}
