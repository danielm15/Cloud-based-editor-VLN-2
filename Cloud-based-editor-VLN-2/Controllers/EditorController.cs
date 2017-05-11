using Cloud_based_editor_VLN_2.Models.Entities;
using Cloud_based_editor_VLN_2.Models.ViewModels;
using Cloud_based_editor_VLN_2.Services;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;


namespace Cloud_based_editor_VLN_2.Controllers {

    public class EditorController : Controller {

        private DocumentService _service = new DocumentService(null);
        private ProjectService _projectService = new ProjectService(null);

        #region checkAuthorization
        private bool checkAuthorization(int projectID) {
            int userID = _service.getUserID(User.Identity.GetUserName());
            List<Project> userProjects = _projectService.GetProjectsByUserID(userID);
            Project currentProject = _projectService.GetProjectByID(projectID);

            if (userProjects.Contains(currentProject)) {
                return true;
            }
            return false;
        }
        #endregion

        #region Index
        /// <summary>
        /// Opens up the chosen document in the code editor
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="documentID"></param>
        /// <returns>string</returns>
        public ActionResult Index(int? projectID, int? documentID) {

            if (projectID.HasValue && documentID.HasValue) {
                ViewBag.DocumentID = documentID ?? default(int);
                int projectByID = projectID ?? default(int);
                int documentByID = documentID ?? default(int);

                if (!checkAuthorization(projectByID)) {
                    return RedirectToAction("Error", "Home");
                }
                DocumentViewModel model = new DocumentViewModel();
                model.CurrProjectID = projectByID;
                model.Documents = _service.GetDocumentsByProjectID(projectByID);
                model.Doc = _service.GetDocumentByID(documentByID);
                return View(model);

            }

            return HttpNotFound();
        }
        #endregion

        #region GetContent
        /// <summary>
        /// Gets document by ID from database
        /// </summary>
        /// <param name="documentID"></param>
        /// <returns>string</returns>
        public ActionResult GetContent(int? documentID) {

            if (documentID.HasValue) {
                int documentByID = documentID ?? default(int);
                string content = _service.GetDocumentByID(documentByID).Content;
                return Json(content, JsonRequestBehavior.AllowGet);
            }

	        return Json(new { success = false });
        }
        #endregion

        #region Save currently opened file
        /// <summary>
        /// Saves and updates currently opened file in the database.
        /// </summary>
        /// <param name="updateDocumentID"></param>
        /// <param name="contentData"></param>
        /// <returns>Json</returns>
        [HttpPost]
        public ActionResult SaveFile(int? updateDocumentID, string contentData) {

            if (updateDocumentID.HasValue) {
                int updateDocumentIdSend = updateDocumentID ?? default(int);
                Document updateDocument = _service.GetDocumentByID(updateDocumentIdSend);
                updateDocument.Content = contentData;
                if (_service.UpdateDocument(updateDocument)) {
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });

        }
        #endregion
    }
}