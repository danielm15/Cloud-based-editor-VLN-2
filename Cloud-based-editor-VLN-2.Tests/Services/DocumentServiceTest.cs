using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cloud_based_editor_VLN_2.Services;
using Cloud_based_editor_VLN_2.Models.Entities;
using Cloud_based_editor_VLN_2.Tests;

namespace Cloud_based_editor_VLN_2.Tests.Services {
    [TestClass]
    public class DocumentServiceTest {

        private DocumentService _DocumentService;

        [TestInitialize]
        public void Initialize() {
            var mockContext = new MockDataContext();

            var user1 = new AppUser {
                ID = 1,
                UserName = "User1",
                Email = "Email1@Email1.com"
            };
            mockContext.AppUsers.Add(user1);

            var project1 = new Project {
                ID = 1,
                OwnerID = 1,
                Name = "Project1",
                AppUser = user1,
                ProjectType = "Javascript"
            };
            mockContext.Projects.Add(project1);

            var document1 = new Document {
                ID = 1,
                Name = "Document1",
                Type = "Javascript",
                CreatedBy = "User1",
                DateCreated = new DateTime(2017, 1, 1),
                Content = "Hello World!",
                LastUpdatedBy = "User1",
                ProjectID = 1,
                Project = project1
            };
            mockContext.Documents.Add(document1);

            var document2 = new Document {
                ID = 2,
                Name = "Document2",
                Type = "HTML",
                CreatedBy = "User1",
                DateCreated = new DateTime(2017, 1, 1),
                Content = null,
                LastUpdatedBy = null,
                ProjectID = 1,
                Project = project1
            };
            mockContext.Documents.Add(document2);

            _DocumentService = new DocumentService(mockContext);
        }

        [TestMethod]
        public void TestGetDocumentsByProjectID() {
            // Arrange:
            
            // Act:
            var documents = _DocumentService.GetDocumentsByProjectID(1);

            // Assert:
            Assert.IsNotNull(documents);
            Assert.AreEqual(2, documents.Count);
            Assert.AreEqual(1, documents[0].ProjectID);
            Assert.AreEqual(1, documents[1].ProjectID);
            Assert.AreEqual("Document1", documents[0].Name);
            Assert.AreEqual("Document2", documents[1].Name);
        }

        [TestMethod]
        public void TestGetDocumentByID() {
            // Arrange:
            // Act:
            var document = _DocumentService.GetDocumentByID(1);
            // Assert:
            Assert.IsNotNull(document);
            Assert.AreEqual(1, document.ID);
            Assert.AreEqual("Document1", document.Name);
        }

        [TestMethod]
        public void TestAddDocument() {
            // Arrange:
            var document3 = new Document {
                ID = 3,
                Name = "Document3",
                Type = "CSS",
                CreatedBy = "User1",
                DateCreated = new DateTime(2017, 1, 1),
                Content = "Some Content",
                LastUpdatedBy = null,
                ProjectID = 1
            };

            // Act:
            var added = _DocumentService.AddDocument(document3);
            var document = _DocumentService.GetDocumentByID(3);

            // Assert:
            Assert.AreEqual(true, added);
            Assert.AreEqual(3, document.ID);
            Assert.AreEqual("Document3", document.Name);
            Assert.AreEqual("Some Content", document.Content);
        }

        [TestMethod]
        public void TestUpdateDocument() {
            // Arrange:
            var document2 = _DocumentService.GetDocumentByID(2);
            document2.Content = "Updated Content";

            // Act:
            var updated = _DocumentService.UpdateDocument(document2);

            // Assert:
            Assert.IsTrue(updated);
            Assert.IsNotNull(document2);
            Assert.AreEqual(2, document2.ID);
            Assert.AreEqual("HTML", document2.Type);
            Assert.AreEqual("Updated Content", document2.Content);
        }

        [TestMethod]
        public void TestDeleteDocument() {
            // Arrange:
            var document1 = _DocumentService.GetDocumentByID(1);

            // Act:
            var deleted = _DocumentService.DeleteDocument(document1);
            var documents = _DocumentService.GetDocumentsByProjectID(1);

            // Should return null as doc should already be deleted
            var deletedDocument = _DocumentService.GetDocumentByID(1);

            // Assert:
            Assert.IsTrue(deleted);
            Assert.AreEqual(1, documents.Count);
            Assert.IsNull(deletedDocument);

        }
    }
}
