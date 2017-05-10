using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cloud_based_editor_VLN_2.Services;
using Cloud_based_editor_VLN_2.Models.Entities;
using Cloud_based_editor_VLN_2.Tests;

namespace Cloud_based_editor_VLN_2.Tests.Services {
    [TestClass]
    public class AppUserServiceTest {

        private AppUserService _AppUserService;

        [TestMethod]
        public void TestAddUser() {
            // Arrange:
            var mockContext = new MockDataContext();

            var user1 = new AppUser {
                ID = 1,
                UserName = "User1",
                Email = "Email1@Email1.com"
            };

            var user2 = new AppUser {
                ID = 2,
                UserName = "User2",
                Email = "Email2@Email2.com"
            };

            var user3 = new AppUser {
                ID = 3,
                UserName = "User3",
                Email = "Email3@Email3.com"
            };

            // Act:
            mockContext.AppUsers.Add(user1);
            mockContext.AppUsers.Add(user2);
            mockContext.AppUsers.Add(user3);
            _AppUserService = new AppUserService(mockContext);
            
            //CollectionAssert.


            // Assert:
            
        }
    }
}
