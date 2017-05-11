using Cloud_based_editor_VLN_2.Models.Entities;
using Cloud_based_editor_VLN_2.Models.ViewModels;
using Cloud_based_editor_VLN_2.Services;
using Microsoft.AspNet.Identity;
using System;
using Ionic.Zip;
using System.Web.Mvc;
using System.Text;

namespace Cloud_based_editor_VLN_2.Controllers {

    public class DocumentController : Controller {

        private DocumentService _service = new DocumentService(null);
        private ProjectService _projectService = new ProjectService(null);

        #region Get Documents(Index)
        // GET: Document
        public ActionResult Index(int? projectID) {

            if (projectID.HasValue) {
	            var id = projectID ?? default(int);
                if (!checkAuthorization(id)) return RedirectToAction("AccessDenied", "Error");
	            var model = new DocumentViewModel();
                model.CurrProjectID = id;
                model.Documents = _service.GetDocumentsByProjectID(id);
                return View(model);
            }

            return HttpNotFound();
        }
        #endregion

        #region CreateDocument POST
        [HttpPost]
        public ActionResult Create(FormCollection formCollection) {

            var fileName = formCollection["fileName"];
            var fileType = formCollection["fileType"];
            var projectID = int.Parse(formCollection["projectID"]);
            var creator = User.Identity.Name;


            if (string.IsNullOrEmpty(fileName) && string.IsNullOrEmpty(fileType)) {
                return Json(new { success = "bothempty" });
            }
            else if (string.IsNullOrEmpty(fileName)) {
                return Json(new { success = "nameempty" });
            }
            else if (string.IsNullOrEmpty(fileType)) {
                return Json(new { success = "filetypeempty" });
            }
            else {
                if (fileType[0] != '.') fileType = "." + fileType;
	            var newDocument = new Document();
                newDocument.Name = fileName;
                newDocument.Type = fileType;
                newDocument.ProjectID = projectID;
                newDocument.LastUpdatedBy = creator;
                newDocument.DateCreated = DateTime.Now;
                newDocument.CreatedBy = creator;
                newDocument.Content = "";

                if (_service.AddDocument(newDocument)) return Json(newDocument);
            }
            return Json(new { success = false });
        }
        #endregion

        #region RenameDocument GET and POST
        public ActionResult _RenameDocument(int? documentID) {

            var doc = _service.GetDocumentByID(documentID ?? default(int));

            return PartialView("_RenameDocument", doc);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult _RenameDocument(Document item) {

            var documentToUpdate = _service.GetDocumentByID(item.ID);
            documentToUpdate.Name = item.Name;
            var projectForOwner = _projectService.GetProjectByID(documentToUpdate.ProjectID);

            if (documentToUpdate.CreatedBy != User.Identity.GetUserName() && projectForOwner.AppUser.UserName != User.Identity.GetUserName()) return Json(new { success = false, message = "noPermission", name = documentToUpdate.Name, type = documentToUpdate.Type, docID = documentToUpdate.ID });
	        if (_service.UpdateDocument(documentToUpdate)) return Json(new { success = true, name = documentToUpdate.Name, type = documentToUpdate.Type, docID = documentToUpdate.ID });
	        else return Json(new { success = false, message = "duplicateFileName", name = documentToUpdate.Name, type = documentToUpdate.Type, docID = documentToUpdate.ID });
        }
        #endregion

        #region Download Documents
        public ActionResult DownloadZip(int? projectID, int? userID, string projectName) {

            var id = projectID ?? default(int);
            var count = 0;
            var documents = _service.GetDocumentsByProjectID(id);

            if (!checkAuthorization(id)) return RedirectToAction("AccessDenied", "Error");

	        using (var zip = new ZipFile()) {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                foreach (var item in documents)
	                if (zip.ContainsEntry(item.Name + item.Type)) {
		                zip.AddEntry(item.Name + "(" + count.ToString() + ")" + item.Type, item.Content);
		                count++;
	                }
	                else {
		                zip.AddEntry(item.Name + item.Type, item.Content);
	                }
		        Response.Clear();
                Response.BufferOutput = false;
                var zipName = string.Format(projectName + ".zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();
            }

            return RedirectToAction("Index", "Project", new { userID });
        }



        public ActionResult DownloadFile(int? documentID) {
            var doc = _service.GetDocumentByID(documentID ?? default(int));

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "text/plain";
            Response.AppendHeader("content-disposition", "attachment;filename=" + doc.Name + doc.Type);

            var content = new StringBuilder();
            content.Append(doc.Content);
            Response.Write(content.ToString());
            Response.End();

            return RedirectToAction("Index", new { projectID = doc.ProjectID });
        }
        #endregion

        #region Delete POST
        [HttpPost]
        public ActionResult Delete(int? documentID) {

            if (documentID.HasValue) {
                var ID = documentID ?? default(int);
                var documentToDelete = _service.GetDocumentByID(ID);
                var projectForOwner = _projectService.GetProjectByID(documentToDelete.ProjectID);

                if (documentToDelete.CreatedBy != User.Identity.GetUserName() && projectForOwner.AppUser.UserName != User.Identity.GetUserName()) return Json(new { success = false, message = "noPermission", name = documentToDelete.Name, documentID = documentID, type = documentToDelete.Type });

	            if (_service.DeleteDocument(documentToDelete)) return Json(new { success = true, name = documentToDelete.Name, documentID = documentID, type = documentToDelete.Type });
            }

            return Json(new { success = false });
        }
        #endregion

        #region checkAtuorization
        private bool checkAuthorization(int projectID) {

            var userID = _service.getUserID(User.Identity.GetUserName());
            var userProjects = _projectService.GetProjectsByUserID(userID);
            var currentProject = _projectService.GetProjectByID(projectID);

            if (userProjects.Contains(currentProject)) return true;
	        return false;
        }
        #endregion
    }

}