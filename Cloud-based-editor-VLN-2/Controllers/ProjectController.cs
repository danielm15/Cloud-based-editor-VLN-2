﻿using Cloud_based_editor_VLN_2.Services;
using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cloud_based_editor_VLN_2.Models.Entities;
using Cloud_based_editor_VLN_2.Models.ViewModels;

namespace Cloud_based_editor_VLN_2.Controllers {
    public class ProjectController : Controller {
        private string _currentUserEmail;
        private int _currentUserID;
        private ProjectService _service = new ProjectService();

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

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddProject(Project item) {

            if (ModelState.IsValid) {
                item.DateCreated = DateTime.Now;
                _service.AddProject(item);
                
                return RedirectToAction("Index");
            }

            return View(item);
        }
    }
}