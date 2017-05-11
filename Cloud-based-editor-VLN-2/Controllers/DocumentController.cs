using Cloud_based_editor_VLN_2.Models.Entities;
using Cloud_based_editor_VLN_2.Models.ViewModels;
using Cloud_based_editor_VLN_2.Services;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using Ionic.Zip;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Newtonsoft.Json;

namespace Cloud_based_editor_VLN_2.Controllers {
    public class DocumentController : Controller {

        private DocumentService _service = new DocumentService(null);
        private ProjectService _projectService = new ProjectService(null);

        #region Get Documents(Index)
        // GET: Document
        public ActionResult Index(int? projectID) {

            if (projectID.HasValue) {
                int id = projectID ?? default(int);
                if (!checkAuthorization(id)) {
                    return RedirectToAction("AccessDenied", "Error");
                }
                DocumentViewModel model = new DocumentViewModel();
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

            string fileName = formCollection["fileName"];
            string fileType = formCollection["fileType"];
            int projectID = Int32.Parse(formCollection["projectID"]);
            string creator = User.Identity.Name;


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
                if (fileType[0] != '.') {
                    fileType = "." + fileType;
                }
                Document newDocument = new Document();
                newDocument.Name = fileName;
                newDocument.Type = fileType;
                newDocument.ProjectID = projectID;
                newDocument.LastUpdatedBy = creator;
                newDocument.DateCreated = DateTime.Now;
                newDocument.CreatedBy = creator;
                newDocument.Content = "";

                if (_service.AddDocument(newDocument)) {
                    return Json(newDocument);
                }

            }
            return Json(new { success = false });
        }
        #endregion

        #region RenameDocument GET and POST
        public ActionResult _RenameDocument(int? documentID) {

            Document d = new Document();
            d = _service.GetDocumentByID(documentID ?? default(int));

            return PartialView("_RenameDocument", d);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult _RenameDocument(Document item) {

            Document documentToUpdate = _service.GetDocumentByID(item.ID);
            documentToUpdate.Name = item.Name;
            Project projectForOwner = _projectService.GetProjectByID(documentToUpdate.ProjectID);

            if (documentToUpdate.CreatedBy != User.Identity.GetUserName() && projectForOwner.AppUser.UserName != User.Identity.GetUserName()) {
                return Json(new { success = false, message = "noPermission", name = documentToUpdate.Name, type = documentToUpdate.Type, docID = documentToUpdate.ID });
            }
            if (_service.UpdateDocument(documentToUpdate)) {
                return Json(new { success = true, name = documentToUpdate.Name, type = documentToUpdate.Type, docID = documentToUpdate.ID });
            }
            else {
                return Json(new { success = false, message = "duplicateFileName", name = documentToUpdate.Name, type = documentToUpdate.Type, docID = documentToUpdate.ID });
            }
        }
        #endregion

        #region Download Documents
        public ActionResult DownloadZip(int? projectID, int? userID, string projectName) {

            int id = projectID ?? default(int);
            int count = 0;
            List<Document> documents = _service.GetDocumentsByProjectID(id);

            if (!checkAuthorization(id)) {
                return RedirectToAction("AccessDenied", "Error");
            }

            using (ZipFile zip = new ZipFile()) {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                foreach (Document item in documents) {
                    if (zip.ContainsEntry(item.Name + item.Type)) {
                        zip.AddEntry(item.Name + "(" + count.ToString() + ")" + item.Type, item.Content);
                        count++;
                    }
                    else {
                        zip.AddEntry(item.Name + item.Type, item.Content);
                    }
                }
                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format(projectName + ".zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();
            }

            return RedirectToAction("Index", "Project", new { userID = userID });
        }



        public ActionResult DownloadFile(int? documentID) {
            Document doc = _service.GetDocumentByID(documentID ?? default(int));

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "text/plain";
            Response.AppendHeader("content-disposition", "attachment;filename=" + doc.Name + doc.Type);

            StringBuilder content = new StringBuilder();
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
                int ID = documentID ?? default(int);
                Document documentToDelete = _service.GetDocumentByID(ID);
                Project projectForOwner = _projectService.GetProjectByID(documentToDelete.ProjectID);

                if (documentToDelete.CreatedBy != User.Identity.GetUserName() && projectForOwner.AppUser.UserName != User.Identity.GetUserName()) {
                    return Json(new { success = false, message = "noPermission", name = documentToDelete.Name, documentID = documentID, type = documentToDelete.Type });
                }

                if (_service.DeleteDocument(documentToDelete)) {
                    return Json(new { success = true, name = documentToDelete.Name, documentID = documentID, type = documentToDelete.Type });
                }
            }

            return Json(new { success = false });
        }
        #endregion

        #region checkAtuorization
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
    }

}