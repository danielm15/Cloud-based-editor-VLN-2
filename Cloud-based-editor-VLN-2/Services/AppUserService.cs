using Cloud_based_editor_VLN_2.Models;
using Cloud_based_editor_VLN_2.Models.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cloud_based_editor_VLN_2.Services {
    public class AppUserService : BaseService{

        public void addUser(AppUser newUser) {
            _db.AppUsers.Add(newUser);
            _db.SaveChanges();
        }

        public void addUserToProject(UserProjects user) {
            _db.UserProjects.Add(user);
            _db.SaveChanges();
        }

        public IEnumerable<AppUser> getLimitedUserList(string searchString) {
            IEnumerable<AppUser> users = (from user in _db.AppUsers
                                          where user.UserName.Contains(searchString) || user.Email.Contains(searchString)
                                          select user).Take(10);
            return users;
        }


    }
}