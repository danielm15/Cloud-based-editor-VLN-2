using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using Cloud_based_editor_VLN_2.Models;
using Cloud_based_editor_VLN_2.Services;

namespace Cloud_based_editor_VLN_2.Controllers {

    [Authorize]
    public class UserController : Controller {

        private string _currentUserEmail;
        private int _currentUserID;
        private AppUserService _service = new AppUserService(null);

        #region ProjectOverview
        // GET: ProjectsOverview
        public ActionResult Index() {

            _currentUserEmail = User.Identity.GetUserName();
            _currentUserID = _service.getUserID(_currentUserEmail);

            return View();
        }
        #endregion

    }
}