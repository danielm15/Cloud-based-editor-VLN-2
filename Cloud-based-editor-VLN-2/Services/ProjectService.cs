using Cloud_based_editor_VLN_2.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using Cloud_based_editor_VLN_2.Models;

namespace Cloud_based_editor_VLN_2.Services {
    public class ProjectService : BaseService {

        public ProjectService(IAppDataContext context) : base(context) {

        }

        // Fetch all projects owned by a particular User
        public List<Project> GetProjectsByUserID(int UserID) {
            var projects = (from p in Db.Projects
                            join up in Db.UserProjects on p.ID equals up.ProjectID
                            join au in Db.AppUsers on up.AppUserID equals au.ID
                            where UserID == up.AppUserID
                            select p).ToList();
            return projects;
        }

        // Fetch a single project by ProjectID
        public Project GetProjectByID(int ProjectID) {
            var project = (from p in Db.Projects
                           where ProjectID == p.ID
                           select p).SingleOrDefault();
            return project;
        }

        public bool AddProject(Project newProject) {
            Db.Projects.Add(newProject);
            Db.SaveChanges();
            UserProjects newLink = new UserProjects();
            var newProjectID = (from p in Db.Projects
                                orderby p.ID descending
                                select p.ID).FirstOrDefault();
            newLink.AppUserID = newProject.OwnerID;
            newLink.ProjectID = newProjectID;
            Db.UserProjects.Add(newLink);
            Db.SaveChanges();
            return true;
        }

        public bool UpdateProject(Project projectToUpdate) {
            Db.SetModified(projectToUpdate);
            try {
                Db.SaveChanges();
            }
            catch {
                return false;
            }
            return true;
        }

        public bool DeleteProject(int projectID) {

            var documents = Db.Documents.Where(item => item.ProjectID == projectID);
            
            foreach (var document in documents.ToList()) {
                Db.Documents.Remove(document);
            }

            var userProjectConnections = Db.UserProjects.Where(item => item.ProjectID == projectID);

            foreach (var userProjectConnection in userProjectConnections.ToList()) {
                Db.UserProjects.Remove(userProjectConnection);
            }

            var project = Db.Projects.Where(item => item.ID == projectID).Single();
            Db.Projects.Remove(project);
            Db.SaveChanges();
            return true;
        }

        public bool AbandonProject(int prjID, int usrID) {
            //var project = _db.UserProjects.Where(item => item.ProjectID == prjID && item.AppUser.ID == usrID).SingleOrDefault();
            var project = (from item in Db.UserProjects
                           where item.ProjectID == prjID && item.AppUserID == usrID
                           select item).SingleOrDefault();
            Db.UserProjects.Remove(project);
            Db.SaveChanges();
            return true;
        }

        public int HowManyUsersAreInTheProject(int prjID) {
            IEnumerable<UserProjects> userList = (from item in Db.UserProjects
													where item.ProjectID == prjID
													select item).ToList();
            return userList.Count();
            //int number = _db.UserProjects.Where(item => item.ProjectID == prjID).All
            //return number;
        }
     }
}