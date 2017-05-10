using Cloud_based_editor_VLN_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Services {
    public class BaseService {
        public ApplicationDbContext _db = new ApplicationDbContext();

        public int getUserID(string userName) {
            var userID = (from users in _db.AppUsers
                          where users.UserName == userName
                          select users.ID).SingleOrDefault();

            return userID;
        }

        public string GetUserNameByUserID(int userID) {
            var userName = (from users in _db.AppUsers
                            where users.ID == userID
                            select users.UserName).SingleOrDefault();
            return userName;
        }
        
    }
}