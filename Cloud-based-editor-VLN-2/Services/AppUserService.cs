using Cloud_based_editor_VLN_2.Models;
using Cloud_based_editor_VLN_2.Models.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cloud_based_editor_VLN_2.Services {
    public class AppUserService : BaseService   {

        public AppUserService(IAppDataContext context) : base(context) {
            
        }

        public void addUser(AppUser newUser) {
            _db.AppUsers.Add(newUser);
            _db.SaveChanges();
        }

        public void addUserToProject(UserProjects user) {
            _db.UserProjects.Add(user);
            _db.SaveChanges();
        }

        public List<AppUser> getAllUsers() {
            List<AppUser> AllUsers = (from user in _db.AppUsers
                                             select user).ToList();
            return AllUsers;
        }

        public IEnumerable<AppUser> getLimitedUserList(string searchString) {
            IEnumerable<AppUser> users = (from user in _db.AppUsers
                                          where user.UserName.Contains(searchString) || user.Email.Contains(searchString)
                                          select user).Take(10);
            return users;
        }

        public List<UserProjects> getAllUserProjects() {
            List<UserProjects> AllUserProjects = (from userProject in _db.UserProjects
                                                  select userProject).ToList();
            return AllUserProjects;
        }
    }
}