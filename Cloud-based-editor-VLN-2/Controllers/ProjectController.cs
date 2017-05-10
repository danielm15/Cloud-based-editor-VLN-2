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

        private ProjectService _service = new ProjectService(null);
        private AppUserService _userService = new AppUserService(null);
        private DocumentService _documentService = new DocumentService(null);

        #region make new document on instanzation
        private void InstanceCorrectFile(string projectType, ref string name, ref string type, ref string content) {
            if (projectType == "HTML") {
                name = "Index";
                type = ".html";
                content = @"<!DOCTYPE html>
<html>
<head>
<title> Page Title </title>  
</head> 
<body> 
<p> Hello world! </p> 
</body>
</html> ";
                return;
            }
            else if (projectType == "C++") {
                name = "main";
                type = ".cpp";
                content = @"#include <iostream>
using namespace std;

int main() {
    cout << ""Hello world"" << endl;
    
    return 0;
}";
                return;
            }
            else if (projectType == "Python") {
                name = "app";
                type = ".py";
                content = @"print ""Hello World"" ";
            }
            else if (projectType == "C#") {
                name = "project";
                type = ".cs";
                content = @"public class project
{
   public static void Main()
   {
      System.Console.WriteLine(""Hello, World!"");
   }
        }";
            }
            else if (projectType == "Javascript") {
                name = "project";
                type = ".js";
                content = @"console.log(""Hello World"" ";
            }
            else if (projectType == "Java") {
                name = "project";
                type = ".java";
                content = @"public class project{
    public static void main(string[] args){
        System.out.println(""Hello World"");
    }
        }";
            }
            else if (projectType == "C") {
                name = "main";
                type = ".c";
                content = @"#include<stdio.h>

main()
{
    printf(""Hello World"");
}
            ";
            }
            else if (projectType == "Php") {
                name = "project";
                type = ".php";
                content = @"<!DOCTYPE html>
<html>
<body>

<?php
echo ""Hello World!"";
?>

</ body >
</ html > ";
            }
            else if (projectType == "Node.js") {
                name = "project";
                type = ".js";
                content = @"console.log(""Hello World"")";
            }
            else if (projectType == "Ruby") {
                name = "project";
                type = ".rb";
                content = @"puts ""Hello World""  ";
            }
        }
        #endregion

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
        #region AddPrj Get and Set
        [HttpGet]
        public ActionResult AddProject(int? ownerID) {
            Project newP = new Project() {
                OwnerID = (ownerID ?? default(int))
            };
            return PartialView("AddProject", newP);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddProject(Project item) {
            string name = "", type = "", content = "";
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
        #endregion

        public ActionResult PopulateList(string searchString) {

            IEnumerable<AppUser> userList = _userService.getLimitedUserList(searchString);

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonUsers = serializer.Serialize(userList);
            return Json(new { userList = userList }, JsonRequestBehavior.AllowGet);
        }
        #region Delete get and set
        public ActionResult DeleteProjectVal(int? projectID) {

            int currentUserID = _service.getUserID(User.Identity.GetUserName());

            if (projectID.HasValue) {
                int ID = projectID ?? default(int);
                Project projectToDelete = _service.GetProjectByID(ID);
                if (projectToDelete.OwnerID == currentUserID) {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }

            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
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
        public ActionResult DeleteProject(int? id) {

            if (id.HasValue) {
                int projectdID = id ?? default(int);
                _service.DeleteProject(projectdID);

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
        #endregion

        #region RenamePrj Get and Post
        public ActionResult _RenameProject(int? ProjectID) {
            Project p = new Project();
            Project prj = _service.GetProjectByID(ProjectID ?? default(int));
            return PartialView("_RenameProject", prj);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult _RenameProject(Project item) {


            Project projectToUpdate = _service.GetProjectByID(item.ID);
            projectToUpdate.Name = item.Name;

            if (projectToUpdate.AppUser.UserName != User.Identity.GetUserName()) {
                return Json(new { success = false, message = "noPermission", name = projectToUpdate.Name, projectID = projectToUpdate.ID });
            }
            if (_service.UpdateProject(projectToUpdate)) {
                return Json(new { success = true, name = projectToUpdate.Name, projectID = projectToUpdate.ID });
            }

            return Json(new { success = false });

        }
        #endregion

        #region Invite Get and Post
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
            }
            else {
                Project projectToAddTo = _service.GetProjectByID(projectID);
                List<Project> userProjects = _service.GetProjectsByUserID(userID);

                if (userProjects.Contains(projectToAddTo)) {
                    return Json(new { sucess = false, message = "userAlreadyInProject", name = userName, project = projectToAddTo.Name, projectID = projectID });
                }
                else {
                    UserProjects userProject = new UserProjects();
                    userProject.ProjectID = projectToAddTo.ID;
                    userProject.AppUserID = userID;

                    _userService.addUserToProject(userProject);
                    return Json(new { success = true, name = userName, project = projectToAddTo.Name, projectID = projectID });
                }
            }
        }
        #endregion

        #region AbandonProject
        public ActionResult AbandonPrj(int? ProjectID) {

            Project p = new Project();
            Project prj = _service.GetProjectByID(ProjectID ?? default(int));
            int userID = _service.getUserID(User.Identity.GetUserName());
            prj.AppUser.ID = userID;
            return PartialView("_AbandonPrjConfirm", prj);

        }

        [HttpPost]
        public ActionResult AbandonPrj(int id, int userID) {
            if(ModelState.IsValid) {
                _service.AbandonProject(id, userID);
                return RedirectToAction("Index");
            }
            return View();
        }
        #endregion

    }
}