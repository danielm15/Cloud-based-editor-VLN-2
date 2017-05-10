using Cloud_based_editor_VLN_2.Models.Entities;
using Cloud_based_editor_VLN_2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using Cloud_based_editor_VLN_2.Models;

namespace Cloud_based_editor_VLN_2.Services {
    public class DocumentService : BaseService {

        public DocumentService(IAppDataContext context) : base(context) {

        }

        // Fetch all files from a single project
        public List<Document> GetDocumentsByProjectID(int ProjectID) {
            var documents = (from doc in _db.Documents
                             join pro in _db.Projects on doc.ProjectID equals pro.ID
                             where doc.ProjectID == ProjectID
                             select doc).ToList();
            return documents;
        }

        // Fetch a single document by its ID
        public Document GetDocumentByID(int DocumentID) {
            var document = (from doc in _db.Documents
                            where doc.ID == DocumentID
                            select doc).SingleOrDefault();
            return document;
        }

        public bool AddDocument(Document newDocument) {
            var v = _db.Documents.Add(newDocument);
            //_db.SaveChanges();
            try {
                _db.SaveChanges();
                //return true;
            }
            catch(DbUpdateException e) {
                
                return false;
            }
            return true;
        }

        public bool UpdateDocument(Document documentToUpadate) {
            _db.Entry(documentToUpadate).State = System.Data.Entity.EntityState.Modified;
            return _db.SaveChanges() == 1;
        }

        public bool DeleteDocument() {
            // TODO
            return true;
        }
    }
}