using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cloud_based_editor_VLN_2.Services;
using Cloud_based_editor_VLN_2.Models.Entities;
using Cloud_based_editor_VLN_2.Tests;

namespace Cloud_based_editor_VLN_2.Tests.Services {
    [TestClass]
    public class ProjectServiceTest {

        private ProjectService _ProjectService;

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
                DateCreated = new DateTime(2017, 1, 1),
                AppUser = user1,
                ProjectType = "Javascript"
            };
            mockContext.Projects.Add(project1);

            var project2 = new Project {
                ID = 2,
                OwnerID = 1,
                Name = "Project2",
                DateCreated = new DateTime(2017, 1, 1),
                AppUser = user1,
                ProjectType = "HTML"
            };
            mockContext.Projects.Add(project2);

            var userProject1 = new UserProjects {
                ID = 1,
                AppUserID = 1,
                ProjectID = 1,
                AppUser = user1,
                Project = project1
            };
            mockContext.UserProjects.Add(userProject1);

            var userProject2 = new UserProjects {
                ID = 2,
                AppUserID = 1,
                ProjectID = 2,
                AppUser = user1,
                Project = project2
            };
            mockContext.UserProjects.Add(userProject2);

            var document1 = new Document {
                ID = 1,
                Name = "Document1",
                Type = "HTML",
                CreatedBy = "User1",
                DateCreated = new DateTime(2017, 1, 1),
                Content = null,
                LastUpdatedBy = null,
                ProjectID = 1,
                Project = project1
            };
            mockContext.Documents.Add(document1);

            _ProjectService = new ProjectService(mockContext);

        }

        [TestMethod]
        public void TestGetProjectsByUserID() {
            // Arrange:
            // Act:
            var projects = _ProjectService.GetProjectsByUserID(1);

            // Assert:
            Assert.IsNotNull(projects);
            Assert.AreEqual(2, projects.Count);
            Assert.AreEqual(1, projects[0].ID);
            Assert.AreEqual(2, projects[1].ID);
            Assert.AreEqual(1, projects[0].OwnerID);
            Assert.AreEqual(1, projects[1].OwnerID);
        }

        [TestMethod]
        public void TestGetProjectByID() {
            // Arrange:
            // Act:
            var project1 = _ProjectService.GetProjectByID(1);
            var project2 = _ProjectService.GetProjectByID(2);
            // Assert:
            Assert.AreEqual(1, project1.ID);
            Assert.AreEqual("Project1", project1.Name);
            Assert.AreEqual(2, project2.ID);
            Assert.AreEqual("Project2", project2.Name);
        }

        [TestMethod]
        public void TestAddProject() {
            // Arrange:
            var project3 = new Project {
                ID = 3,
                OwnerID = 1,
                Name = "Project3",
                DateCreated = new DateTime(2017, 1, 1),
                ProjectType = "HTML"
            };

            // Act:
            var added = _ProjectService.AddProject(project3);
            var getProject = _ProjectService.GetProjectByID(3);

            // Assert:
            Assert.IsTrue(added);
            Assert.AreEqual(3, getProject.ID);
            Assert.AreEqual(1, getProject.OwnerID);
        }

        [TestMethod]
        public void TestUpdateProject() {
            // Arrange:
            var project2 = _ProjectService.GetProjectByID(2);
            var originalName = project2.Name;

            // Act:
            project2.Name = "New name";
            var updated = _ProjectService.UpdateProject(project2);
            var updatedProject = _ProjectService.GetProjectByID(2);

            // Assert:
            Assert.IsTrue(updated);
            Assert.IsNotNull(updatedProject);
            Assert.AreNotEqual(originalName, updatedProject.Name);
            Assert.AreEqual("New name", updatedProject.Name);
        }

        [TestMethod]
        public void TestDeleteProject() {
            // Arrange:
            var project1 = _ProjectService.GetProjectByID(1);

            // Act:
            var deleted = _ProjectService.DeleteProject(project1.ID);
            var projects = _ProjectService.GetProjectsByUserID(1);

            // Should return null as project should already be deleted
            var deletedProject = _ProjectService.GetProjectByID(1);

            // Assert:
            Assert.IsTrue(deleted);
            Assert.AreEqual(1, projects.Count);
            Assert.IsNull(deletedProject);
        }
    }
}
