using System.Data.Entity;
using Cloud_based_editor_VLN_2.Models;
using Cloud_based_editor_VLN_2.Models.Entities;
using System.Data.Entity.Infrastructure;
using System.Data.Common;

namespace Cloud_based_editor_VLN_2.Tests {
	
    /// <summary>
    /// This is an example of how we'd create a fake database by implementing the 
    /// same interface that the BookeStoreEntities class implements.
    /// </summary>
    public class MockDataContext : IAppDataContext {
	    /// <summary>
	    /// Sets up the fake database.
	    /// </summary>
	    ///
		public MockDataContext() {
            // We're setting our DbSets to be InMemoryDbSets rather than using SQL Server.
            AppUsers = new InMemoryDbSet<AppUser>();
            UserProjects = new InMemoryDbSet<UserProjects>();
            Projects = new InMemoryDbSet<Project>();
            Documents = new InMemoryDbSet<Document>();
			Invitations = new InMemoryDbSet<Invitation>();
		    SaveSuccess = true;
		}

        public IDbSet<AppUser> AppUsers { get; set; }

        public IDbSet<UserProjects> UserProjects { get; set; }

        public IDbSet<Project> Projects { get; set; }

        public IDbSet<Document> Documents { get; set; }

	    public IDbSet<Invitation> Invitations { get; set; }

	    public bool SaveSuccess { get; set; }

		public void SetModified(object entity) { }

        public int SaveChanges() {

	        if (!SaveSuccess) {
		        throw new DbUpdateException();
	        }
	        return 1;
        }
    }
}
