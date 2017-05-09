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

        // GET: ProjectsOverview
        public ActionResult Index() {
            _currentUserEmail = User.Identity.GetUserName();
            _currentUserID = _service.getUserID(_currentUserEmail);
            ProjectViewModel model = new ProjectViewModel();
            model.CurrUserID = _currentUserID;
            model.Projects = _service.GetProjectsByUserID(_currentUserID);

            return View(model);
        }

        [HttpGet]
        public ActionResult AddProject(int? ownerID) {
            Project newP = new Project();
            newP.OwnerID = (ownerID ?? default(int));
            return View(newP);             
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

        public ActionResult DeleteProjectConfirm(int? ProjectID) {
            Project prj = _service.GetProjectByID(ProjectID ?? default(int));
            return PartialView("_DeleteProjectConfirm", prj);
        }

        public ActionResult DeleteNoPermission(int? ProjectID) {
            Project prj = _service.GetProjectByID(ProjectID ?? default(int));
            return PartialView("_DeleteNoPermission", prj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProject(Project item) {

            if (ModelState.IsValid) {
                item.DateCreated = DateTime.Now;
                _service.AddProject(item);
                
                return RedirectToAction("Index", new { projectID = item.ID });
            }

            return View(item);
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
            Project prj = _service.GetProjectByID(ProjectID ?? default(int));
            return PartialView("_InviteUser", prj);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult InviteUser(Project item) {
            if (ModelState.IsValid) {
                Project test = _service.GetProjectByID(item.ID);
                int userID = _service.getUserID(item.Name);

                UserProjects p = new UserProjects();
                p.ProjectID = test.ID;
                p.AppUserID = userID;

                _userService.addUserToProject(p);

                //_service._db.UserProjects.Add(p);
                //_service._db.SaveChanges();

                return RedirectToAction("Index", new { projectID = item.ID});
            }
            return View();
        }
    }
}