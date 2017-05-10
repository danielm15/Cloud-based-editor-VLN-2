using Cloud_based_editor_VLN_2.Services;
using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cloud_based_editor_VLN_2.Models.Entities;
using Cloud_based_editor_VLN_2.Models.ViewModels;
using System.IO;

namespace Cloud_based_editor_VLN_2.Controllers {
    public class ProjectController : Controller {
        private string _currentUserEmail;
        private int _currentUserID;
        private ProjectService _service = new ProjectService();
        private AppUserService _userService = new AppUserService();
        private DocumentService _documentService = new DocumentService();

        private void InstanceCorrectFile(string projectType, ref string name, ref string type, ref string content) {
            if(projectType == "HTML") {
                name = "Index";
                type = ".html";
//                content = @"<!DOCTYPE html>
//<html>
//<head>
//<title> Page Title </title>  
//</head> 
//<body> 
//<p> Hello world! </p> 
//</body>
//</html> ";
            }
            else if (projectType == "C++") {
                name = "main";
                type = ".cpp";
            }
            else if (projectType == "Python") {
                name = "app";
                type = ".py"
            }
            else if (projectType == "C#") {
                name = "project";
                type = ".cs";
            }
            else if (projectType == "Javascript") {
                name = "project";
                type = ".js";
            }
            else if (projectType == "Java") {
                name = "project";
                type = ".java";
            }
            else if (projectType == "C") {
                name = "main";
                type = ".c";
            }
            else if (projectType == "Php") {
                name = "project";
                type = ".php";
            }
            else if (projectType == "Node.js") {
                name = "project";
                type = ".js";
            }
            else if (projectType == "Ruby") {
                name = "project";
                type = ".rb";
            }

        }
        
        // GET: ProjectsOverview
        public ActionResult Index() {
            _currentUserEmail = User.Identity.GetUserName();
            _currentUserID = _service.getUserID(_currentUserEmail);
            ProjectViewModel model = new ProjectViewModel() {
                CurrUserID = _currentUserID,
                Projects = _service.GetProjectsByUserID(_currentUserID)
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult AddProject(int? ownerID) {
            Project newP = new Project() {
                OwnerID = (ownerID ?? default(int))
            };
            return PartialView("AddProject", newP);             
        }

        public ActionResult DeleteProjectVal(int? projectID) {

            int currentUserID = _service.getUserID(User.Identity.GetUserName());

            if (projectID.HasValue) {
                int ID = projectID ?? default(int);
                Project projectToDelete = _service.GetProjectByID(ID);
                if (projectToDelete.OwnerID == currentUserID) {  
                    return Json(new { success = true}, JsonRequestBehavior.AllowGet);
                }
                
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PopulateList(string searchString) {

            IEnumerable<AppUser> userList = _userService.getLimitedUserList(searchString);

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonUsers = serializer.Serialize(userList);
            return Json(new { userList = userList}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteProjectConfirm(int? ProjectID) {
            Project prj = _service.GetProjectByID(ProjectID ?? default(int));
            return PartialView("_DeleteProjectConfirm", prj);
        }

        public ActionResult DeleteNoPermission(int? ProjectID) {
            Project prj = _service.GetProjectByID(ProjectID ?? default(int));
            return PartialView("_DeleteNoPermission", prj);
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddProject(Project item) {
            string name = "", type ="", content ="";
            if (ModelState.IsValid) {
                item.DateCreated = DateTime.Now;
                _service.AddProject(item);

                Document doc = new Document();
                doc.ProjectID = item.ID;
                doc.DateCreated = item.DateCreated;
                doc.CreatedBy = _service.GetUserNameByUserID(item.OwnerID);
                doc.LastUpdatedBy = doc.CreatedBy;
                InstanceCorrectFile(item.ProjectType, ref name, ref type, ref content);
                doc.Name = name;
                doc.Type = type;
                doc.Content = content;

                    //doc.CreatedBy = _service._db.AppUsers.
                _documentService.AddDocument(doc);

                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult _RenameProject(int? ProjectID) {
            Project p = new Project();
            Project prj= _service.GetProjectByID(ProjectID ?? default(int));
            return  PartialView("_RenameProject", prj);
        }

        [HttpPost]
        public ActionResult DeleteProject(int? id) {

            if (id.HasValue) {
                int projectdID = id ?? default(int);
                _service.DeleteProject(projectdID);

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult _RenameProject(Project item) {
            if (ModelState.IsValid) {
                Project test = _service.GetProjectByID(item.ID);
                test.Name = item.Name;
                _service._db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult InviteUser(int? ProjectID) {
            Project p = new Project();
            ProjectViewModel someTest = new ProjectViewModel(); 
            Project prj = _service.GetProjectByID(ProjectID ?? default(int));
            return PartialView("_InviteUser", prj);
        }

        [HttpPost]
        public ActionResult InviteUser(FormCollection collection) {

            int projectID = int.Parse(collection["ID"]);
            string userName = collection["inputUser"];

            int userID = _service.getUserID(userName);

            if (userID == 0) {
                return Json(new { sucess = false, message = "userNotFound", name = userName, projectID = projectID });
            } else {
                Project projectToAddTo = _service.GetProjectByID(projectID);
                List<Project> userProjects = _service.GetProjectsByUserID(userID);

                if (userProjects.Contains(projectToAddTo)) {
                    return Json(new { sucess = false, message = "userAlreadyInProject", name = userName, project = projectToAddTo.Name, projectID = projectID});
                } else {
                    UserProjects userProject = new UserProjects();
                    userProject.ProjectID = projectToAddTo.ID;
                    userProject.AppUserID = userID;

                    _userService.addUserToProject(userProject);
                    return Json(new { success = true, name = userName, project = projectToAddTo.Name, projectID = projectID });
                }
            }
        }
    }
}