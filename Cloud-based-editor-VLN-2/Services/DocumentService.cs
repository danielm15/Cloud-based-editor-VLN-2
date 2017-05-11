using Cloud_based_editor_VLN_2.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using Cloud_based_editor_VLN_2.Models;

namespace Cloud_based_editor_VLN_2.Services {
    public class DocumentService : BaseService {

        public DocumentService(IAppDataContext context) : base(context) {

        }

        // Fetch all files from a single project
        public List<Document> GetDocumentsByProjectID(int ProjectID) {
            var documents = (from doc in Db.Documents
                             join pro in Db.Projects on doc.ProjectID equals pro.ID
                             where doc.ProjectID == ProjectID
                             select doc).ToList();
            return documents;
        }

        // Fetch a single document by its ID
        public Document GetDocumentByID(int DocumentID) {
            var document = (from doc in Db.Documents
                            where doc.ID == DocumentID
                            select doc).SingleOrDefault();
            return document;
        }

        public bool AddDocument(Document newDocument) {
            Db.Documents.Add(newDocument);

            try {
                Db.SaveChanges();
            }
            catch {
                return false;
            }
            return true;
        }

        public bool UpdateDocument(Document documentToUpdate) {
            Db.SetModified(documentToUpdate);

            try {
                Db.SaveChanges();
            }
            catch {
                return false;
            }
            return true;
        }

        public bool DeleteDocument(Document documentToDelete) {
            Db.Documents.Remove(documentToDelete);
            Db.SaveChanges();
            return true;
        }


    }
}