using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Cloud_based_editor_VLN_2.Models.Entities;

namespace Cloud_based_editor_VLN_2.Models {
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager) {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public interface IAppDataContext {
        IDbSet<AppUser> AppUsers { get; set; }

        IDbSet<UserProjects> UserProjects { get; set; }

        IDbSet<Project> Projects { get; set; }

        IDbSet<Document> Documents { get; set; }

        int SaveChanges();

        void SetModified(object entity);
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IAppDataContext {
        public IDbSet<AppUser> AppUsers { get; set; }

        public IDbSet<UserProjects> UserProjects { get; set; }

        public IDbSet<Project> Projects { get; set; }

        public IDbSet<Document> Documents { get; set; }

        public void SetModified(object entity) {
            Entry(entity).State = EntityState.Modified;
        }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false) {
        }

        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            Database.SetInitializer<ApplicationDbContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}