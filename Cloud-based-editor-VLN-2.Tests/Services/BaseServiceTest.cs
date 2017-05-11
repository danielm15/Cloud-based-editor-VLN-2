using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cloud_based_editor_VLN_2.Services;
using Cloud_based_editor_VLN_2.Models.Entities;


namespace Cloud_based_editor_VLN_2.Tests.Services {
    [TestClass]
    public class BaseServiceTest {

        private BaseService _BaseService;

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

            _BaseService = new BaseService(mockContext);
        }

        [TestMethod]
        public void TestGetUserId() {
            // Arrange:
            // Act:
            var user1ID = _BaseService.getUserID("User1");
            var user2ID = _BaseService.getUserID("User2");

            // Assert:
            Assert.IsNotNull(user1ID);
            Assert.IsNotNull(user2ID);
            Assert.AreEqual(1, user1ID);
            Assert.AreEqual(2, user2ID);
        }

        [TestMethod]
        public void TestGetUserNameByUserID() {
            // Arrange:
            // Act:
            var UserName1 = _BaseService.GetUserNameByUserID(1);
            var UserName2 = _BaseService.GetUserNameByUserID(2);
            // Assert:
            Assert.IsNotNull(UserName1);
            Assert.IsNotNull(UserName2);
            Assert.AreEqual("User1", UserName1);
            Assert.AreEqual("User2", UserName2);
        }
    }
}
