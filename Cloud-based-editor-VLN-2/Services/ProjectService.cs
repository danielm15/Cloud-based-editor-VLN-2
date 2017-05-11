using Cloud_based_editor_VLN_2.Models.Entities;
using Cloud_based_editor_VLN_2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cloud_based_editor_VLN_2.Models;

namespace Cloud_based_editor_VLN_2.Services {
    public class ProjectService : BaseService {

        public ProjectService(IAppDataContext context) : base(context) {

        }

        // Fetch all projects owned by a particular User
        public List<Project> GetProjectsByUserID(int UserID) {
            var projects = (from p in _db.Projects
                            join up in _db.UserProjects on p.ID equals up.ProjectID
                            join au in _db.AppUsers on up.AppUserID equals au.ID
                            where UserID == up.AppUserID
                            select p).ToList();
            return projects;
        }

        // Fetch a single project by ProjectID
        public Project GetProjectByID(int ProjectID) {
            var project = (from p in _db.Projects
                           where ProjectID == p.ID
                           select p).SingleOrDefault();
            return project;
        }

        public bool AddProject(Project newProject) {
            _db.Projects.Add(newProject);
            _db.SaveChanges();
            UserProjects newLink = new UserProjects();
            var newProjectID = (from p in _db.Projects
                                orderby p.ID descending
                                select p.ID).FirstOrDefault();
            newLink.AppUserID = newProject.OwnerID;
            newLink.ProjectID = newProjectID;
            _db.UserProjects.Add(newLink);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateProject(Project projectToUpdate) {
            _db.Entry(projectToUpdate).State = System.Data.Entity.EntityState.Modified;
            try {
                _db.SaveChanges();
            }
            catch {
                return false;
            }
            return true;
        }

        public bool DeleteProject(int projectID) {

            var documents = _db.Documents.Where(item => item.ProjectID == projectID);
            
            foreach (var document in documents) {
                _db.Documents.Remove(document);
            }

            var userProjectConnections = _db.UserProjects.Where(item => item.ProjectID == projectID);

            foreach (var userProjectConnection in userProjectConnections) {
                _db.UserProjects.Remove(userProjectConnection);
            }

            var project = _db.Projects.Where(item => item.ID == projectID).Single();
            _db.Projects.Remove(project);
            _db.SaveChanges();
            return true;
        }

        public bool AddInvitation(Invitation newInvitaion) {
            _db.Invitations.Add(newInvitaion);

            return _db.SaveChanges() == 1;
        }

        public List<Invitation> GetUserInvitations(int userID) {
            var invitaions = (from inv in _db.Invitations
                              where inv.AppUserID == userID
                              select inv).ToList();

            return invitaions;
        }

        /*public bool DeleteInvitaion(Invitation invitation) {
            _db.Invitations.Remove(invitation);

            return _db.SaveChanges() == 1;
        }*/

        public bool ContainsInvitation(Invitation invitation) {
            var result = (from inv in _db.Invitations
                          where inv.AppUserID == invitation.AppUserID
                          && inv.ProjectID == invitation.ProjectID
                          select inv).SingleOrDefault();

            return !(result == null);
        }

        public bool HasUserProject(UserProjects userProject) {
            var result = (from up in _db.UserProjects
                          where up.AppUserID == userProject.AppUserID
                          && up.ProjectID == userProject.ProjectID
                          select up).SingleOrDefault();

            return !(result == null);
        }

        public bool AddUserToProject(UserProjects newUserProject) {
            _db.UserProjects.Add(newUserProject);

            return _db.SaveChanges() == 1;
        }

        public bool RemoveInvite(Invitation invite) {
            var invToRemove = (from inv in _db.Invitations
                               where inv.AppUserID == invite.AppUserID
                               && inv.ProjectID == invite.ProjectID
                               select inv).SingleOrDefault();

            _db.Invitations.Remove(invToRemove);

            return _db.SaveChanges() == 1;
        }
    }
}