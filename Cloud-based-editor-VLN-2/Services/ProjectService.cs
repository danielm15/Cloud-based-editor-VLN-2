using Cloud_based_editor_VLN_2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Services {
    public class ProjectService : BaseService {

        // Fetch all projects owned by a particular User
        public List<ProjectViewModel> GetProjectsByUserID(int UserID) {
            /*var projects = (from up in _db.UserProjects
                            )*/
            return null;
        }

        // Fetch a single project by ProjectID
        public ProjectViewModel GetProjectByID(int ProjectID) {
            // TODO
            return null;
        }
    }

}