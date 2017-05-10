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
        //private string _currentUserEmail;
        //private int _currentUserID;
        private DocumentService _service = new DocumentService(null);

        // GET: Document
        public ActionResult Index(int? projectID) {
            //_currentUserEmail = User.Identity.GetUserName();
            //_currentUserID = _service.getUserID(_currentUserEmail);
            
            if (projectID.HasValue) {
                int id = projectID ?? default(int);
                DocumentViewModel model = new DocumentViewModel();
                model.CurrProjectID = id;
                model.Documents = _service.GetDocumentsByProjectID(id);
                return View(model);
            }

            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Create(FormCollection formCollection) {

            string fileName = formCollection["fileName"];
            string fileType = formCollection["fileType"];          
            int projectID = Int32.Parse(formCollection["projectID"]);
            string creator = User.Identity.Name;

            
            if(string.IsNullOrEmpty(fileName) && string.IsNullOrEmpty(fileType)) {
                return Json(new { success = "bothempty" });
            }
            else if (string.IsNullOrEmpty(fileName)) {
                return Json(new { success = "nameempty" });
            }
            else if(string.IsNullOrEmpty(fileType)) {
                return Json(new { success = "filetypeempty" });
            }
            else {
                fileType = "." + fileType;
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

        public ActionResult DownloadZip(int? projectID, int? userID, string projectName) {
            int id = projectID ?? default(int);
            List<Document> documents = _service.GetDocumentsByProjectID(id);
            int count = 0;

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

        public ActionResult _RenameDocument(int? documentID)
        {
            Document d = new Document();
            d = _service.GetDocumentByID(documentID ?? default(int));

            return PartialView("_RenameDocument", d);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult _RenameDocument(Document item)
        {
            if (ModelState.IsValid)
            {
                Document test = _service.GetDocumentByID(item.ID);
                test.Name = item.Name;
                _service._db.SaveChanges();

                return RedirectToAction("Index", new { projectID = item.ProjectID });
            }
            return View();
        }

        public ActionResult DownloadFile(int? documentID) {
            Document doc = _service.GetDocumentByID(documentID ?? default(int));

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "text/plain";
            Response.AppendHeader("content-disposition", "attachment;filename="+ doc.Name + doc.Type);

            StringBuilder content = new StringBuilder();
            content.Append(doc.Content);
            Response.Write(content.ToString());
            Response.End();

            return RedirectToAction("Index", new { projectID = doc.ProjectID });
        }
    }
}