using System.Web.Mvc;


namespace Cloud_based_editor_VLN_2.Controllers {
    public class HomeController : ParentController {

        public ActionResult Index() {

            return View();
        }

        public ActionResult About() { 

            return View();
        }

        public ActionResult Support() { 

            return View();
        }

	    public ActionResult Error() {

		    return View("Error");
	    }
    }
}