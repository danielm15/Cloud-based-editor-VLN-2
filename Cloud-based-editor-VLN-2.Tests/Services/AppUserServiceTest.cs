using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cloud_based_editor_VLN_2.Services;
using Cloud_based_editor_VLN_2.Models.Entities;
using System.Collections.Generic;

namespace Cloud_based_editor_VLN_2.Tests.Services {
    [TestClass]
    public class AppUserServiceTest {

        private AppUserService _AppUserService;

	    private static T First<T>(IEnumerable<T> items) {
            using (var iter = items.GetEnumerator()) {
                iter.MoveNext();
                return iter.Current;
            }
        }

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

            var user3 = new AppUser {
                ID = 3,
                UserName = "User3",
                Email = "Email3@Email3.com"
            };
            mockContext.AppUsers.Add(user3);

            var project1 = new Project {
                ID = 1,
                OwnerID = 1,
                Name = "Project1",
                DateCreated = new DateTime(2017, 1, 1),
                AppUser = user1,
                ProjectType = "HTML"
            };
	        mockContext.Projects.Add(project1);

            var userProject1 = new UserProjects {
                ID = 1,
                AppUserID = 1,
                ProjectID = 1,
                AppUser = user1,
                Project = project1
            };

            mockContext.UserProjects.Add(userProject1);

            _AppUserService = new AppUserService(mockContext);
        }

        [TestMethod]
        public void TestAddUser() {
            // Arrange:
            var user4 = new AppUser {
                ID = 4,
                UserName = "User4",
                Email = "Email4@Email4.com"
            };
            // Act:
            _AppUserService.addUser(user4);

            var AllUsers = _AppUserService.getAllUsers();

            // Assert:
            Assert.AreEqual(4, AllUsers.Count);
            Assert.AreEqual(4, AllUsers[3].ID);
            Assert.AreEqual("User4", AllUsers[3].UserName);
            Assert.AreEqual("Email4@Email4.com", AllUsers[3].Email);
        }

        [TestMethod]
        public void TestAddUserToProject() {
            // Arrange:
            var user2 = new AppUser {
                ID = 2,
                UserName = "User2",
                Email = "Email2@Email2.com"
            };

            var project2 = new Project {
                ID = 2,
                OwnerID = 2,
                Name = "Project2",
                DateCreated = new DateTime(2017, 1, 1),
                AppUser = user2,
                ProjectType = "HTML"
            };

            var userProject2 = new UserProjects {
                ID = 2,
                AppUserID = 2,
                ProjectID = 2,
                AppUser = user2,
                Project = project2
            };
            // Act:
            _AppUserService.addUserToProject(userProject2);
            var AllUserProjects = _AppUserService.getAllUserProjects();

            // Assert:
            Assert.AreEqual(2, AllUserProjects.Count);
            Assert.AreEqual(2, AllUserProjects[1].ID);
            Assert.AreEqual(2, AllUserProjects[1].AppUserID);
            Assert.AreEqual(user2, AllUserProjects[1].AppUser);
            Assert.AreEqual(project2, AllUserProjects[1].Project);
        }

        [TestMethod]
        public void GetAllUsers() {
            // Arrange:

            // Act:
            var AllUsers = _AppUserService.getAllUsers();

            // Assert:
            Assert.IsNotNull(AllUsers);
            Assert.AreEqual(3, AllUsers.Count);
            Assert.AreNotEqual(4, AllUsers.Count);
            Assert.AreEqual(1, AllUsers[0].ID);
        }

        [TestMethod]
        public void GetLimitedUserList() {
            // Arrange:
            // Act:
            var FoundUsers = _AppUserService.getLimitedUserList("User1");

            var firstUser = First(FoundUsers);

            // Assert:
            Assert.IsNotNull(FoundUsers);
            Assert.AreEqual(1, firstUser.ID);
            Assert.AreEqual("User1", firstUser.UserName);
            Assert.AreEqual("Email1@Email1.com", firstUser.Email);
        }

        [TestMethod]
        public void GetAllUserProjects() {
            // Arrange:

            // Act:
            var AllUserProjects = _AppUserService.getAllUserProjects();

            // Assert:
            Assert.IsNotNull(AllUserProjects);
            Assert.AreEqual(1, AllUserProjects.Count);
            Assert.AreEqual(1, AllUserProjects[0].ID);
            Assert.AreEqual(1, AllUserProjects[0].AppUserID);
            Assert.AreEqual(1, AllUserProjects[0].ProjectID);
            Assert.AreEqual("User1", AllUserProjects[0].AppUser.UserName);
        }

	    [TestMethod]
	    public void TestGetAllUsersInProject() {
			// Arrange:
		    var project1 = new Project {
			    ID = 1,
			    OwnerID = 1,
			    Name = "Project1",
			    DateCreated = new DateTime(2017, 1, 1),
			    ProjectType = "HTML"
		    };

			// Act:
		    var usersInProject = _AppUserService.getAllUsersInProject(project1);

		    // Assert:
			Assert.IsNotNull(usersInProject);
			Assert.AreEqual(1, usersInProject.Count);
			Assert.AreEqual(1, usersInProject[0].ID);
	    }
    }
}
