﻿using Cloud_based_editor_VLN_2.Models.Entities;
using Cloud_based_editor_VLN_2.Models.ViewModels;
using Cloud_based_editor_VLN_2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Cloud_based_editor_VLN_2.Controllers {

    public class EditorController : Controller {

        private DocumentService _service = new DocumentService();

        // GET: Editor
        public ActionResult Index(int? projectID, int? documentID) {

            if(projectID.HasValue && documentID.HasValue) {
                int projectByID = projectID ?? default(int);
                int documentByID = documentID ?? default(int);
                DocumentViewModel model = new DocumentViewModel();
                model.CurrProjectID = projectByID;
                model.Documents = _service.GetDocumentsByProjectID(projectByID);
                return View(model);
                
            }

            return HttpNotFound();
        }
    }
}