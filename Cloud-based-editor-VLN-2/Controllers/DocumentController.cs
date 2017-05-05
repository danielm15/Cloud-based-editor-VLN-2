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
                model.CurrProjectID = id;
                model.Documents = _service.GetDocumentsByProjectID(id);
                return View(model);
            }

            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult Create(int? projectID) {
            Document newDocument = new Document();
            newDocument.ProjectID = (projectID ?? default(int));

            return View(newDocument);
        }

        [HttpPost]
        public ActionResult Create(Document newDocument) {
            if( ModelState.IsValid) {
                newDocument.DateCreated = DateTime.Now;
                _service.AddDocument(newDocument);

                return RedirectToAction("Index", new { projectID = newDocument.ProjectID });
            }

            return View(newDocument);
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