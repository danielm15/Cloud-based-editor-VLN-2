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

      
 
    
        [HttpPost]
        public ActionResult SaveFile(int updateDocumentID, string contentData) {

            Document updatedocument = _service.GetDocumentByID(updateDocumentID);
            updatedocument.Content = contentData;
            _service.UpdateDocument(updatedocument);
            Console.WriteLine("komst i data");
            return Json(new {  success = true});
       
        }
    }
}