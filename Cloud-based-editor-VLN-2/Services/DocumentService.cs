using Cloud_based_editor_VLN_2.Models.Entities;
using Cloud_based_editor_VLN_2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Services {
    public class DocumentService : BaseService {

        // Fetch all files from a single project
        public List<Document> GetDocumentsByProjectID(int ProjectID) {
            var documents = (from doc in _db.Documents
                             join pro in _db.Projects on doc.ProjectID equals pro.ID
                             select doc).ToList();
            return documents;
        }

        // Fetch a single document by its ID
        public DocumentViewModel GetDocumentByID(int DocumentID) {
            // TODO
            return null;
        }

        public bool AddDocument(Document newDocument) {
            _db.Documents.Add(newDocument);
            if(_db.SaveChanges() == 1) {
                return true;
            }
            return false;
        }

        public bool DeleteDocument() {
            // TODO
            return true;
        }
    }
}