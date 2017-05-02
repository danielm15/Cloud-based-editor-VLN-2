using Cloud_based_editor_VLN_2.Models.Entities;
using Cloud_based_editor_VLN_2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Services {
    public class ProjectService : BaseService {

        // Fetch all projects owned by a particular User
        public List<Project> GetProjectsByUserID(int UserID) {
            var projects = (from p in _db.Projects
                            join up in _db.UserProjects on p.ID equals up.ProjectID
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
    }
}