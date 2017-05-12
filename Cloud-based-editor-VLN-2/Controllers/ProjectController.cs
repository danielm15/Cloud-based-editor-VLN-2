using Cloud_based_editor_VLN_2.Services;
using System;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using Cloud_based_editor_VLN_2.Models.Entities;
using Cloud_based_editor_VLN_2.Models.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cloud_based_editor_VLN_2.Controllers {

    public class ProjectController : ParentController {

        private string _currentUserEmail;
        private int _currentUserID;

        private ProjectService _service = new ProjectService(null);
        private AppUserService _userService = new AppUserService(null);
        private DocumentService _documentService = new DocumentService(null);

        #region Index
        /// <summary>
        /// This function calls the Index view which takes in the current userID and
        /// the projects the user is affiliated in and lists them.
        /// </summary>
        /// <returns>Index view for projects</returns>
        public ActionResult Index() {

            _currentUserEmail = User.Identity.GetUserName();
            _currentUserID = _service.getUserID(_currentUserEmail);
            var model = new ProjectViewModel() {
                CurrUserID = _currentUserID,
                Projects = _service.GetProjectsByUserID(_currentUserID)
            };
            return View(model);
        }
        #endregion

        #region AddPrj Get and Set
        /// <summary>
        /// Takes in the ownerID and sends it to the model which then lists up the 
        /// modal window for Addproject
        /// </summary>
        /// <param name="ownerID"></param>
        /// <returns>partial view to the addproject(modal window for partial view)</returns>
        [HttpGet]
        public ActionResult AddProject(int? ownerID) {
            var newP = new Project() {
                OwnerID = ownerID ?? default(int)
            };
            return PartialView("AddProject", newP);
        }

        /// <summary>
        /// Takes in the project we are about to create and adds it to the project db
        /// it also makes the beginning document for each file depending on what kind of file it is
        /// </summary>
        /// <param name="item"></param>
        /// <returns>injects the html for the new project to the projects</returns>
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

                ViewBag.UserName = User.Identity.GetUserName();

                var html = RenderRazorViewToString("AddProjectContainerForAdd", item);

                return Json(html);
            }

            return Json(new { success = false });
        }
        #endregion

        #region PopulateList
        // Gets the users which match  the special searchstring
        public ActionResult PopulateList(string searchString) {

            var userList = _userService.getLimitedUserList(searchString);

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var jsonUsers = serializer.Serialize(userList);
            return Json(new { userList = userList }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete get and set
        /// <summary>
        /// Checks if you can delete the project(if you have the authority)
        /// if the Json returns true he will call DeleteProjectConfirm(are you shure you want to delete?)
        /// and if false we call DeleteNoPermission(you are not admin)
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns>Json object</returns>
        public ActionResult DeleteProjectVal(int? projectID) {

            var currentUserID = _service.getUserID(User.Identity.GetUserName());

            if (projectID.HasValue) {
                var ID = projectID ?? default(int);
                var projectToDelete = _service.GetProjectByID(ID);
                if (projectToDelete.OwnerID == currentUserID) return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        // Takes in the projectID and gets the correspondent project from that and sends 
        // it to the deleteprojectconfirm partial view and sends in the project which 
        // checks if you are shure you want to delete the project.
        public ActionResult DeleteProjectConfirm(int? ProjectID) {
            var prj = _service.GetProjectByID(ProjectID ?? default(int));
            return PartialView("_DeleteProjectConfirm", prj);
        }

        // Takes in the projectID and gets the correspondent project from that and sends
        // it to the deletenopermission partial view and sends in the project, which 
        // tells you that you don´t have permission to delete because you are not admin
        public ActionResult DeleteNoPermission(int? ProjectID) {
            var prj = _service.GetProjectByID(ProjectID ?? default(int));
            return PartialView("_DeleteNoPermission", prj);
        }


        /// <summary>
        /// is called when the user has pressed delete in confirmdelete view
        /// if the project id makes sense we delete it and return Json(true)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Json object</returns>
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
        /// <summary>
        /// Is called when the user presses rename button in the options menu
        /// takes in the projectID and finds the corresponding project and sends that project
        /// to the renameproject partial view.
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns>partial view to the _renameProject view</returns>
        public ActionResult _RenameProject(int? ProjectID) {
            var p = new Project();
            var prj = _service.GetProjectByID(ProjectID ?? default(int));
            return PartialView("_RenameProject", prj);
        }

        /// <summary>
        /// takes in the project and checks if you are admin, if you are not admin you get a nopermission view
        /// if you are admin you can rename the project successfully
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Json object</returns>
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
        /// <summary>
        /// Is called when the user presses the invite button
        /// Takes in the projectID and finds the project according to that
        /// calls the _inviteUser partial view and sends in the project with it.
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns>Inviteuser partial view</returns>
        public ActionResult InviteUser(int? ProjectID) {
            var p = new Project();
            var someTest = new ProjectViewModel();
            var prj = _service.GetProjectByID(ProjectID ?? default(int));
            return PartialView("_InviteUser", prj);
        }

        /// <summary>
        /// Adds invitation to a project from a user to the database if
        /// he has not already received an invite or is already a collaborator,
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="userName"></param>
        /// <returns>Json object</returns>
        [HttpPost]
        public ActionResult Invite(int projectID, string userName) {

            var fromUserName = _service.GetProjectByID(projectID).AppUser.UserName;
            var userID = _service.getUserID(userName);

            if (userID == 0) return Json(new { success = "userNotFound", name = userName, projectID = projectID });

	        var inv = new Invitation();
            inv.fromUserName = fromUserName;
            inv.AppUserID = userID;
            inv.ProjectID = projectID;

            var project = new UserProjects();
            project.AppUserID = userID;
            project.ProjectID = projectID;

            if (_service.ContainsInvitation(inv)) {
                return Json(new { success = "hasInvite", name = userName, projectID = projectID });
            }
            else if (_service.HasUserProject(project)) {
                return Json(new { success = "hasProject", name = userName, projectID = projectID });
            }
            else {
                _service.AddInvitation(inv);
                return Json(new { success = true, name = userName, projectID = projectID });
            }

        }
        /// <summary>
        /// Gets all the invites the user has not responded to (accepted or declined)
        /// </summary>
        /// <returns>Json object</returns>
        [HttpGet]
        public ActionResult GetInvites() {
            var userID = _service.getUserID(User.Identity.GetUserName());

            var invites = _service.GetUserInvitations(userID);

            var projects = new List<Project>();

            foreach (var item in invites) projects.Add(_service.GetProjectByID(item.ProjectID));
	        if (projects.Count == 0) return Json(new { success = false }, JsonRequestBehavior.AllowGet);

	        var settings = new JsonSerializerSettings {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
            var serializer = JsonSerializer.Create(settings);
            var projectsResult = JsonConvert.SerializeObject(projects);
            var invitesResult = JsonConvert.SerializeObject(invites);

            return Json(new { projectsResult, invitesResult }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the project to the users project list if he accepts an invite
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns>Json object</returns>
        [HttpPost]
        public ActionResult AcceptProject(int projectID) {

            var userID = _service.getUserID(User.Identity.GetUserName());

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

        /// <summary>
        /// Removes the invite from users invites if declines it
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns>Json object</returns>
        [HttpPost]
        public ActionResult DeclineProject(int projectID) {

            var userID = _service.getUserID(User.Identity.GetUserName());

            var invite = new Invitation();
            invite.AppUserID = userID;
            invite.ProjectID = projectID;

            if (_service.RemoveInvite(invite)) return Json(new { success = true });
	        return Json(new { success = false });
        }

        /// <summary>
        /// Gets the html for the project container so the the accepted project
        /// can be added to the project page without refreshing the whole site
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns>Json object</returns>
        public ActionResult AddInvitedProject(int projectID) {

            var project = _service.GetProjectByID(projectID);
            var html = RenderRazorViewToString("AddProjectContainer", project);

            return Json(html, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region AbandonProject
        /// <summary>
        /// This is called when the abandon project button is pushed
        /// Takes in the projectID and finds project and userID and validates that
        /// if Admin is the only one in the project the project can be deleted
        /// If the admin is not the only one in the project he has to choose one of the collaborators to be admin before abandoning the prj
        /// if the user trying to delete is not admin he can be removed
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns>returns Json object</returns>
        public ActionResult AbandonPrj(int? ProjectID) {

            var p = new Project();
            var prj = _service.GetProjectByID(ProjectID ?? default(int));
            var userID = _service.getUserID(User.Identity.GetUserName());
            prj.AppUser.ID = userID;

            if (prj.AppUser.ID == prj.OwnerID && _service.HowManyUsersAreInTheProject(prj.ID) > 1) return Json(new { message = "Admin++" }, JsonRequestBehavior.AllowGet);
	        if (prj.AppUser.ID == prj.OwnerID && _service.HowManyUsersAreInTheProject(prj.ID) == 1) return Json(new { message = "Admin-" }, JsonRequestBehavior.AllowGet);
	        else return Json(new { message = "notAdmin" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This is called if the user is not the admin and he has pressed confirm abandonprj
        /// this function deletes the user and project connection from the db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <returns>redirect</returns>
        [HttpPost]
        public ActionResult AbandonPrj(int? id, int? userID) {
            if (id.HasValue && userID.HasValue) {
                _service.AbandonProject(id ?? default(int), userID ?? default(int));
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// The user is the admin and is the only one in the project, so we ask him if he wants to delete the prj
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns>partial view to _AdminAbandonProject</returns>
        public ActionResult AbandonPrjAdmin(int? ProjectID) {
            var prj = _service.GetProjectByID(ProjectID ?? default(int));
            return PartialView("_AdminAbandonProject", prj);
        }

        /// <summary>
        /// The user is not admin and can be removed from the project
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns>partial view to _AbandonPrjConfirm</returns>
        public ActionResult AbandonPrjNormal(int? ProjectID) {
            var prj = _service.GetProjectByID(ProjectID ?? default(int));
            ViewBag.CurrentUserID = _service.getUserID(User.Identity.GetUserName());
            return PartialView("_AbandonPrjConfirm", prj);
        }
        #endregion

        #region ListCollaborators
        /// <summary>
        /// This is called when the user presses list collaborators
        /// this lists all the collaborators in the project
        /// if you are admin of the project you get two buttons delete user and make admin
        /// if you are not admin you just get to see who is admin and all the users in the project.
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public ActionResult ListCollaborators(int? ProjectID) {

            if (ProjectID.HasValue) {
                var userProject = _service.GetProjectByID(ProjectID ?? default(int));
                var allUsers = _userService.getAllUsersInProject(userProject);

                ViewBag.projectName = userProject.Name;
                ViewBag.Owner = userProject.AppUser.UserName;
                ViewBag.User = User.Identity.GetUserName();
                ViewBag.ProjectID = userProject.ID;

                return PartialView("_Listcollaborators", allUsers);
            }

	        return Json(false);
        }
        #endregion

        #region makeAdmin
        /// <summary>
        /// This is used when the admin presses make admin button
        /// this function takes in id and userID and finds the username from that
        /// then changes the owner name and returns true, if return is false then there is an error message.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MakeAdmin(int? id, int? userID) {
            if (id.HasValue && userID.HasValue) {
                var currentUserID = _service.getUserID(User.Identity.GetUserName());
                _service.changeOwner(id ?? default(int), userID ?? default(int));
                return Json(new { sucess = true });
            }
            return Json(new { sucess = false });
        }
        #endregion

        #region make new document on instanzation
        /// <summary>
        /// This function makes a file when you have made a new project the name,type and content
        /// varies after which project type we are working with, (f.e. if this is a c++ file then
        /// we make a new file containing the name main and type cpp
        /// </summary>
        /// <param name="projectType"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="content"></param>
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

        // Gets the number of invitations user has
        public ActionResult GetNotificationsCount() {
            var userID = _service.getUserID(User.Identity.GetUserName());

            var count = _service.GetUserInvitations(userID).Count;

            return Json(new { count = count }, JsonRequestBehavior.AllowGet);
        }
    }
}
