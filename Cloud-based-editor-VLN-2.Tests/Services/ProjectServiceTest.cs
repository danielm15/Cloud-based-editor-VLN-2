using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cloud_based_editor_VLN_2.Services;
using Cloud_based_editor_VLN_2.Models.Entities;

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

            var user2 = new AppUser {
                ID = 2,
                UserName = "User2",
                Email = "Email2@Email2.com"
            };
            mockContext.AppUsers.Add(user2);

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

            var userProject3 = new UserProjects {
                ID = 3,
                AppUserID = 2,
                ProjectID = 2,
                AppUser = user2,
                Project = project2
            };
            mockContext.UserProjects.Add(userProject3);

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

	        var invitation1 = new Invitation {
		        ID = 1,
				fromUserName = "User1",
				AppUserID = 2,
				ProjectID = 1,
				AppUser = user2,
				Project = project1
	        };
	        mockContext.Invitations.Add(invitation1);

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
	        var date = project1.DateCreated;
	        var type = project1.ProjectType;
	        var AppUser = project1.AppUser;

	        var user1 = new AppUser {
		        ID = 1,
		        UserName = "User1",
		        Email = "Email1@Email1.com"
	        };

			// Assert:
			Assert.IsNotNull(date);
			Assert.AreEqual(user1.ID, AppUser.ID);
	        Assert.AreEqual(user1.UserName, AppUser.UserName);
	        Assert.AreEqual(user1.Email, AppUser.Email);
			Assert.AreEqual("Javascript", type);
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
        public void TestUpdateProjectSuccess() {
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
	    public void TestUpdateProjectFail() {
			// Arrange:
		    var mockContext = new MockDataContext();

			var user1 = new AppUser {
			    ID = 1,
			    UserName = "User1",
			    Email = "Email1@Email1.com"
		    };
		    mockContext.AppUsers.Add(user1);

		    var user2 = new AppUser {
			    ID = 2,
			    UserName = "User2",
			    Email = "Email2@Email2.com"
		    };
		    mockContext.AppUsers.Add(user2);

		    var project1 = new Project {
			    ID = 1,
			    OwnerID = 1,
			    Name = "Project1",
			    DateCreated = new DateTime(2017, 1, 1),
			    AppUser = user1,
			    ProjectType = "Javascript"
		    };
		    mockContext.Projects.Add(project1);

		    _ProjectService = new ProjectService(mockContext);
		    mockContext.SaveSuccess = false;

			// Act:
		    project1.OwnerID = 2;
		    var updated = _ProjectService.UpdateProject(project1);

		    // Assert:
			Assert.IsFalse(updated);

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

        [TestMethod]
        public void TestAbandonProject() {
            // Arrange:
            var project2ID = 2;
            var user2ID = 2;
            // Act:
            var usersBefore = _ProjectService.HowManyUsersAreInTheProject(project2ID);
            var abandoned = _ProjectService.AbandonProject(project2ID, user2ID);
            var usersAfter = _ProjectService.HowManyUsersAreInTheProject(project2ID);

            // Assert:
            Assert.IsTrue(abandoned);
            Assert.AreEqual(2, usersBefore);
            Assert.AreEqual(1, usersAfter);
            
        }

        [TestMethod]
        public void TestHowManyUsersAreInTheProject() {
            // Arrange:

            // Act:
            int numberBefore = _ProjectService.HowManyUsersAreInTheProject(2);
            var abandoned = _ProjectService.AbandonProject(2, 2);
            var numberAfter = _ProjectService.HowManyUsersAreInTheProject(2);

            // Assert:
            Assert.AreEqual(2, numberBefore);
            Assert.IsTrue(abandoned);
            Assert.AreEqual(1, numberAfter);

        }

	    [TestMethod]
	    public void TestAddInvitation() {
			// Arrange:
		    var invitation2 = new Invitation {
			    ID = 2,
			    AppUserID = 2,
			    ProjectID = 2
		    };

			// Act:
			var added = _ProjectService.AddInvitation(invitation2);

		    // Assert:
			Assert.AreEqual(true, added);
	    }

	    [TestMethod]
	    public void TestGetUserInvitations() {
			// Arrange:
		    var invitation2 = new Invitation {
			    ID = 2,
			    AppUserID = 2,
			    ProjectID = 2
		    };

			// Act:
			var invitations = _ProjectService.GetUserInvitations(2);
		    invitation2.AppUser = invitations[0].AppUser;
		    invitation2.Project = invitations[0].Project;

			// Assert:
			Assert.AreEqual(1, invitations.Count);
			Assert.AreEqual(1, invitations[0].ID);
			Assert.AreEqual(2, invitations[0].AppUserID);
		    Assert.AreEqual(1, invitations[0].ProjectID);
			Assert.AreEqual(invitation2.AppUser.ID, invitations[0].AppUserID);
		    Assert.AreEqual(invitation2.Project.ID, invitations[0].ProjectID);
		}

	    [TestMethod]
	    public void TestContainsInvitation() {
			// Arrange:
		    var invitation2 = new Invitation {
			    ID = 2,
			    AppUserID = 2,
			    ProjectID = 2
		    };
		    var invitations = _ProjectService.GetUserInvitations(2);
		    var invitation1 = invitations[0];
		    var sentFrom = invitation1.fromUserName;

		    // Act:
		    var contains = _ProjectService.ContainsInvitation(invitation1);
		    var containsNot = _ProjectService.ContainsInvitation(invitation2);

		    // Assert:
			Assert.IsTrue(contains);
			Assert.AreEqual("User1", sentFrom);
			Assert.IsFalse(containsNot);
	    }

	    [TestMethod]
	    public void TestHasUserProject() {
		    // Arrange:
		    var userProject1 = new UserProjects {
			    ID = 3,
				AppUserID = 2,
				ProjectID = 2
		    };

		    // Act:
		    var isLinked = _ProjectService.HasUserProject(userProject1);

		    // Assert:
		    Assert.IsTrue(isLinked);
	    }

	    [TestMethod]
	    public void TestAddUserToProject() {
			// Arrange:
		    var userProject4 = new UserProjects {
			    ID = 4,
			    AppUserID = 2,
			    ProjectID = 1
		    };

			// Act:
		    var added = _ProjectService.AddUserToProject(userProject4);
		    var isLinked = _ProjectService.HasUserProject(userProject4);

		    // Assert:
			Assert.IsTrue(added);
			Assert.IsTrue(isLinked);
	    }

	    [TestMethod]
	    public void TestRemoveInvite() {
			// Arrange:
		    var invitation1 = new Invitation {
			    ID = 1,
			    AppUserID = 2,
			    ProjectID = 1,
		    };

			// Act:
		    var removed = _ProjectService.RemoveInvite(invitation1);
		    var contains = _ProjectService.ContainsInvitation(invitation1);

		    // Assert:
			Assert.IsTrue(removed);
			Assert.IsFalse(contains);
	    }

	    [TestMethod]
	    public void TestchangeOwnerSuccess() {
		    // Arrange:
		    var project1ID = 1;
		    var user2ID = 2;
		    var project1 = _ProjectService.GetProjectByID(project1ID);
		    var beforeID = project1.OwnerID;

		    // Act:
		    var changed = _ProjectService.changeOwner(project1ID, user2ID);
		    var afterID = project1.OwnerID;

		    // Assert:
			Assert.IsTrue(changed);
			Assert.AreEqual(1, beforeID);
			Assert.AreEqual(2, afterID);
	    }

	    [TestMethod]
	    public void TestchangeOwnerFail() {
			// Arrange:
		    var mockContext = new MockDataContext();

		    var user1 = new AppUser {
			    ID = 1,
			    UserName = "User1",
			    Email = "Email1@Email1.com"
		    };
		    mockContext.AppUsers.Add(user1);

		    var user2 = new AppUser {
			    ID = 2,
			    UserName = "User2",
			    Email = "Email2@Email2.com"
		    };
		    mockContext.AppUsers.Add(user2);

		    var project1 = new Project {
			    ID = 1,
			    OwnerID = 1,
			    Name = "Project1",
			    DateCreated = new DateTime(2017, 1, 1),
			    AppUser = user1,
			    ProjectType = "Javascript"
		    };
		    mockContext.Projects.Add(project1);

		    _ProjectService = new ProjectService(mockContext);
		    mockContext.SaveSuccess = false;

			// Act:
		    var changed = _ProjectService.changeOwner(project1.ID, user2.ID);

		    // Assert:
			Assert.IsFalse(changed);

	    }
	}
}
