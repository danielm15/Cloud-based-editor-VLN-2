using System.Web;
using System.Web.Optimization;

namespace Cloud_based_editor_VLN_2
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                    "~/Scripts/jquery-{version}.js", "~/Scripts/jquery.signalR-2.2.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                    "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                    "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/customscript").Include(
                    "~/Scripts/dropDownScript.js", "~/Scripts/src-noconflict/ace.js",
                    "~/Scripts/IndexSliderScript.js", "~/Scripts/editor.js", "~/Scripts/ProjectOverview.js",
                    "~/Scripts/documentList.js", "~/Scripts/FileOverview.js", "~/Scripts/notifications.js"));

            bundles.Add(new ScriptBundle("~/bundles/customscript2").Include(
                 "~/Scripts/documentList.js"
                ));

            bundles.Add(new StyleBundle("~/Content/Stylesheets/AllStyles").Include(
                "~/Content/Stylesheets/AllStylesConcat.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/bootstrap.min.css"
                    ));

            bundles.Add(new StyleBundle("~/Content/Stylesheets/css").Include(
                    "~/Content/Stylesheets/Documents.css", "~/Content/Stylesheets/Editor.css",
                    "~/Content/Stylesheets/GlobalStyles.css", "~/Content/Stylesheets/HomePage.css",
                    "~/Content/Stylesheets/Projects.css"
                    ));
       
        }
    }
}
