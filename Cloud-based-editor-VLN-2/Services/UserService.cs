using Cloud_based_editor_VLN_2.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cloud_based_editor_VLN_2.Services {
    
    public class UserService {
        public int getUserID(string userEmail) {
            ApplicationDbContext db = new ApplicationDbContext();
            var userID = (from users in db.AppUsers
                          where userEmail == users.Email
                          select users.ID).SingleOrDefault();

            return userID;
        }
    }
}