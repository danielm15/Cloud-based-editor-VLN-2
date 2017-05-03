using Cloud_based_editor_VLN_2.Models.ViewModels;
using Cloud_based_editor_VLN_2.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cloud_based_editor_VLN_2.Controllers {
    public class DocumentController : Controller {
        //private string _currentUserEmail;
        //private int _currentUserID;
        private DocumentService _service = new DocumentService();

        // GET: Document
        public ActionResult Index(int? projectID) {
            //_currentUserEmail = User.Identity.GetUserName();
            //_currentUserID = _service.getUserID(_currentUserEmail);
            if (projectID.HasValue) {
                int id = projectID ?? default(int);
                DocumentViewModel model = new DocumentViewModel();
                model.Documents = _service.GetDocumentsByProjectID(id);
                return View(model);
            }

            return HttpNotFound();
        }
    }
}