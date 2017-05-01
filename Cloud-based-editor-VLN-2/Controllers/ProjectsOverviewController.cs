using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cloud_based_editor_VLN_2.Controllers
{
    public class ProjectsOverviewController : Controller
    {
        // GET: ProjectsOverview
        [Authorize]
        public ActionResult Index()
        {
            //var userID = System.Web.HttpContext.Current.User.Identity.
            return View();
        }
    }
}