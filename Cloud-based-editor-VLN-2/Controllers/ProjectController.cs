using Cloud_based_editor_VLN_2.Services;
using System;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using Cloud_based_editor_VLN_2.Models.Entities;
using Cloud_based_editor_VLN_2.Models.ViewModels;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Web.Services;
using System.Web.Script.Services;
using System.Collections.Generic;

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
            var model = new ProjectViewModel() {
                CurrUserID = _currentUserID,
                Projects = _service.GetProjectsByUserID(_currentUserID)
            };
            return View(model);
        }
        #region AddPrj Get and Set
        [HttpGet]
        public ActionResult AddProject(int? ownerID) {
            var newP = new Project() {
                OwnerID = ownerID ?? default(int)
            };
            return PartialView("AddProject", newP);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddProject(Project item) {
            string name = "", type = "", content = "";
            if (ModelState.IsValid) {
                item.DateCreated = DateTime.Now;
                item.Name = item.Name.Replace(' ', '_');
                _service.AddProject(item);

                var doc = new Document();
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

            var userList = _userService.getLimitedUserList(searchString);

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var jsonUsers = serializer.Serialize(userList);
            return Json(new { userList = userList }, JsonRequestBehavior.AllowGet);
        }
        #region Delete get and set
        public ActionResult DeleteProjectVal(int? projectID) {

            var currentUserID = _service.getUserID(User.Identity.GetUserName());

            if (projectID.HasValue) {
                var ID = projectID ?? default(int);
                var projectToDelete = _service.GetProjectByID(ID);
                if (projectToDelete.OwnerID == currentUserID) return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult DeleteProjectConfirm(int? ProjectID) {
            var prj = _service.GetProjectByID(ProjectID ?? default(int));
            return PartialView("_DeleteProjectConfirm", prj);
        }

        public ActionResult DeleteNoPermission(int? ProjectID) {
            var prj = _service.GetProjectByID(ProjectID ?? default(int));
            return PartialView("_DeleteNoPermission", prj);
        }



        [HttpPost]
        public ActionResult DeleteProject(int? id) {

            if (id.HasValue) {
                var projectdID = id ?? default(int);
                _service.DeleteProject(projectdID);

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
        #endregion

        #region RenamePrj Get and Post
        public ActionResult _RenameProject(int? ProjectID) {
            var p = new Project();
            var prj = _service.GetProjectByID(ProjectID ?? default(int));
            return PartialView("_RenameProject", prj);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult _RenameProject(Project item) {


            var projectToUpdate = _service.GetProjectByID(item.ID);
            projectToUpdate.Name = item.Name.Replace(' ', '_');

            if (projectToUpdate.AppUser.UserName != User.Identity.GetUserName()) return Json(new { success = false, message = "noPermission", name = projectToUpdate.Name, projectID = projectToUpdate.ID });
	        if (_service.UpdateProject(projectToUpdate)) return Json(new { success = true, name = projectToUpdate.Name, projectID = projectToUpdate.ID });

	        return Json(new { success = false });

        }
        #endregion

        #region Invite Get and Post
        public ActionResult InviteUser(int? ProjectID) {
            var p = new Project();
            var someTest = new ProjectViewModel();
            var prj = _service.GetProjectByID(ProjectID ?? default(int));
            return PartialView("_InviteUser", prj);
        }

        [HttpPost]
        public ActionResult InviteUser(FormCollection collection) {

            var projectID = int.Parse(collection["ID"]);
            var userName = collection["inputUser"];

            var userID = _service.getUserID(userName);

            if (userID == 0) {
                return Json(new { sucess = false, message = "userNotFound", name = userName, projectID = projectID });
            }
            else {
                var projectToAddTo = _service.GetProjectByID(projectID);
                var userProjects = _service.GetProjectsByUserID(userID);

                if (userProjects.Contains(projectToAddTo)) {
                    return Json(new { sucess = false, message = "userAlreadyInProject", name = userName, project = projectToAddTo.Name, projectID = projectID });
                }
                else {
                    var userProject = new UserProjects();
                    userProject.ProjectID = projectToAddTo.ID;
                    userProject.AppUserID = userID;

                    _userService.addUserToProject(userProject);
                    return Json(new { success = true, name = userName, project = projectToAddTo.Name, projectID = projectID });
                }
            }
        }

        [HttpPost]
        public ActionResult Invite(int projectID, string userName) {
 

            int userID = _service.getUserID(userName);

            if(userID == 0) {
                return Json(new { success = "userNotFound", name = userName, projectID = projectID });
            }

            Invitation inv = new Invitation();
            inv.AppUserID = userID;
            inv.ProjectID = projectID;

            UserProjects project = new UserProjects();
            project.AppUserID = userID;
            project.ProjectID = projectID;

            if (_service.ContainsInvitation(inv)) {
                return Json(new { success = "hasInvite", name = userName, projectID = projectID });
            }
            else if(_service.HasUserProject(project)) {
                return Json(new { success = "hasProject", name = userName, projectID = projectID });
            }
            else {
                _service.AddInvitation(inv);
                return Json(new { success = true, name = userName, projectID = projectID });
            }

        }

        [HttpGet]
        public ActionResult GetInvites() {
            int userID = _service.getUserID(User.Identity.GetUserName());

            var invites = _service.GetUserInvitations(userID);

            var projects = new List<Project>();
            
            foreach(Invitation item in invites) {
                projects.Add(_service.GetProjectByID(item.ProjectID));
            }

            /*var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(invites);*/
            //var result = new JavaScriptSerializer().Serialize(projects);

            JsonSerializerSettings settings = new JsonSerializerSettings {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
            var serializer = JsonSerializer.Create(settings);
            var result = JsonConvert.SerializeObject(projects);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult AcceptProject(int projectID) {

            int userID = _service.getUserID(User.Identity.GetUserName());

            var newUserProject = new UserProjects();
            newUserProject.AppUserID = userID;
            newUserProject.ProjectID = projectID;

            var invite = new Invitation();
            invite.AppUserID = userID;
            invite.ProjectID = projectID;

            if (_service.AddUserToProject(newUserProject) && _service.RemoveInvite(invite)) {
                return Json(new { success = true });

            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult DeclineProject(int projectID) {

            int userID = _service.getUserID(User.Identity.GetUserName());

            var invite = new Invitation();
            invite.AppUserID = userID;
            invite.ProjectID = projectID;

            if (_service.RemoveInvite(invite)) {
                return Json(new { success = true });

            }
            return Json(new { success = false });
        }
        #endregion

        #region AbandonProject
        public ActionResult AbandonPrj(int? ProjectID) {

            var p = new Project();
            var prj = _service.GetProjectByID(ProjectID ?? default(int));
            var userID = _service.getUserID(User.Identity.GetUserName());
            prj.AppUser.ID = userID;

            if (prj.AppUser.ID == prj.OwnerID && _service.HowManyUsersAreInTheProject(prj.ID) > 1) {
                return Json(new { message = "Admin++" }, JsonRequestBehavior.AllowGet);

                /// TODO:: Tell him he cannot remove him self from the project || make someone else admin and remove him from the project
            }
            if (prj.AppUser.ID == prj.OwnerID && _service.HowManyUsersAreInTheProject(prj.ID) == 1) {
                return Json(new { message = "Admin-" }, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(new { message = "notAdmin" }, JsonRequestBehavior.AllowGet);
            }
        }
        


        [HttpPost]
        public ActionResult AbandonPrj(int? id, int? userID) {
            if(id.HasValue && userID.HasValue) {
                _service.AbandonProject(id?? default(int), userID?? default(int));
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public ActionResult AbandonPrjAdmin(int? ProjectID) {
            var prj = _service.GetProjectByID(ProjectID ?? default(int));
            return PartialView("_AdminAbandonProject", prj);
        }

        public ActionResult AbandonPrjNormal(int? ProjectID) {
            var prj = _service.GetProjectByID(ProjectID ?? default(int));
            ViewBag.CurrentUserID = _service.getUserID(User.Identity.GetUserName());
            return PartialView("_AbandonPrjConfirm", prj);
        }
        #endregion

        #region ListCollaborators
        public ActionResult ListCollaborators(int? ProjectID) {

            if (ProjectID.HasValue) {
                Project userProject = _service.GetProjectByID(ProjectID ?? default(int));
                List<AppUser> allUsers = _userService.getAllUsersInProject(userProject);

                ViewBag.projectName = userProject.Name;
                ViewBag.Owner = userProject.AppUser.UserName;
                ViewBag.User = User.Identity.GetUserName();
                ViewBag.ProjectID = userProject.ID;

                return PartialView("_Listcollaborators", allUsers);
            }

            return View();
        }
        #endregion

        [HttpPost]
        public ActionResult MakeAdmin(int? id, int? userID) {
            if (id.HasValue && userID.HasValue) {
                int currentUserID = _service.getUserID(User.Identity.GetUserName());
                _service.changeOwner(id?? default(int), userID?? default(int));
            }
            return RedirectToAction("Index");
        }

    }
}