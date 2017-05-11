using Cloud_based_editor_VLN_2.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Cloud_based_editor_VLN_2.Controllers {
    public class HomeController : Controller {

        public ActionResult Index() {

            return View();
        }

        public ActionResult About() { 

            return View();
        }

        public ActionResult Support() { 

            return View();
        }
    }
}