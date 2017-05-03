using Cloud_based_editor_VLN_2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Services {
    public class DocumentService : BaseService{

        // Fetch all files from a single project
        public List<DocumentViewModel> GetDocumentsByProjectID(int ProjectID) {
            // TODO
            return null;
        }

        // Fetch a single document by its ID
        public DocumentViewModel GetDocumentByID(int DocumentID) {
            // TODO
            return null;
        }

        public bool AddDocument() {
            // TODO
            return true;
        }

        public bool DeleteDocument() {
            // TODO
            return true;
        }
    }
}