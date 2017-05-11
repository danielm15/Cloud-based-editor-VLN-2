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

        public bool AddInvitation(Invitation newInvitaion) {
            Db.Invitations.Add(newInvitaion);

            return Db.SaveChanges() == 1;
        }

        public List<Invitation> GetUserInvitations(int userID) {
            var invitaions = (from inv in Db.Invitations
                              where inv.AppUserID == userID
                              select inv).ToList();

            return invitaions;
        }

        /*public bool DeleteInvitaion(Invitation invitation) {
            _db.Invitations.Remove(invitation);

            return _db.SaveChanges() == 1;
        }*/

        public bool ContainsInvitation(Invitation invitation) {
            var result = (from inv in Db.Invitations
                          where inv.AppUserID == invitation.AppUserID
                          && inv.ProjectID == invitation.ProjectID
                          select inv).SingleOrDefault();

            return !(result == null);
        }

        public bool HasUserProject(UserProjects userProject) {
            var result = (from up in Db.UserProjects
                          where up.AppUserID == userProject.AppUserID
                          && up.ProjectID == userProject.ProjectID
                          select up).SingleOrDefault();

            return !(result == null);
        }

        public bool AddUserToProject(UserProjects newUserProject) {
            Db.UserProjects.Add(newUserProject);

            return Db.SaveChanges() == 1;
        }

        public bool RemoveInvite(Invitation invite) {
            var invToRemove = (from inv in Db.Invitations
                               where inv.AppUserID == invite.AppUserID
                               && inv.ProjectID == invite.ProjectID
                               select inv).SingleOrDefault();

            Db.Invitations.Remove(invToRemove);

            return Db.SaveChanges() == 1;
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

        public bool changeOwner(int prjID, int usrID) {

            var project = Db.Projects.Where(item => item.ID == prjID).Single();
            var newUser = Db.AppUsers.Where(item => item.ID == usrID).Single();
            project.OwnerID = usrID;
            project.AppUser = newUser;
            Db.SetModified(project);
            try {
                Db.SaveChanges();
            }
            catch {
                return false;
            }
            return true;
        }
     }
}