using Cloud_based_editor_VLN_2.Models.Entities;
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
                model.Doc = _service.GetDocumentByID(documentByID);
                return View(model);
                
            }

            return HttpNotFound();
        }

        public ActionResult GetContent(int? documentID) {

            if (documentID.HasValue) {
                int documentByID = documentID ?? default(int);
                string content = _service.GetDocumentByID(documentByID).Content;
                return Json(content, JsonRequestBehavior.AllowGet);
            }

            return View();
        }
    
        [HttpPost]
        public ActionResult SaveFile(int? updateDocumentID, string contentData) {

            if (updateDocumentID.HasValue) {
                int updateDocumentIDSend = updateDocumentID ?? default(int); 
                Document updateDocument = _service.GetDocumentByID(updateDocumentIDSend);
                updateDocument.Content = contentData;
                if (_service.UpdateDocument(updateDocument)) {
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });

        }
    }
}