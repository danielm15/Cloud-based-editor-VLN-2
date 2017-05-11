using Cloud_based_editor_VLN_2.Models;
using System.Linq;


namespace Cloud_based_editor_VLN_2.Services {
    public class BaseService {

        public IAppDataContext Db;

        public BaseService(IAppDataContext context) {
            Db = context ?? new ApplicationDbContext();
        }

        public int getUserID(string userName) {
            var userID = (from users in Db.AppUsers
                          where users.UserName == userName
                          select users.ID).SingleOrDefault();

            return userID;
        }

        public string GetUserNameByUserID(int userID) {
            var userName = (from users in Db.AppUsers
                            where users.ID == userID
                            select users.UserName).SingleOrDefault();
            return userName;
        }
        
    }
}