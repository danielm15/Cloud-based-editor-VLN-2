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
            _db.SetModified(projectToUpdate);
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
            
            foreach (var document in documents.ToList()) {
                _db.Documents.Remove(document);
            }

            var userProjectConnections = _db.UserProjects.Where(item => item.ProjectID == projectID);

            foreach (var userProjectConnection in userProjectConnections.ToList()) {
                _db.UserProjects.Remove(userProjectConnection);
            }

            var project = _db.Projects.Where(item => item.ID == projectID).Single();
            _db.Projects.Remove(project);
            _db.SaveChanges();
            return true;
        }
     }
}