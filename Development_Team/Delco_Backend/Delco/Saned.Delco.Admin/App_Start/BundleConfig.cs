using System.Web;
using System.Web.Optimization;

namespace Saned.Delco.Admin
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         //"~/Scripts/jquery-{version}.js"

                         "~/assets/js/jquery.min.js"
                        ));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));


            bundles.Add(new ScriptBundle("~/bundles/unobtrusive").Include(
                       "~/Scripts/jquery.unobtrusive-ajax.js"

                ));



           


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/dist/js/bootstrap-dialog.js"
                  ));


            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                   
                    "~/assets/js/bootstrap-rtl.min.js",
                     "~/assets/js/detect.js",
                     "~/assets/js/fastclick.js",
                     "~/assets/js/jquery.slimscroll.js",
                     "~/assets/js/jquery.blockUI.js",
                     "~/assets/js/waves.js",
                     "~/assets/js/jquery.nicescroll.js",
                     "~/assets/js/jquery.scrollTo.min.js",
                     "~/assets/js/jquery.core.js",
                     "~/assets/js/jquery.app.js"
            ));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                   
                     "~/assets/css/bootstrap-rtl.min.css",
                     "~/assets/css/core.css",
                     "~/assets/css/components.css",
                     "~/assets/css/icons.css",
                     "~/assets/css/pages.css",
                     "~/assets/css/menu.css",
                     "~/assets/css/responsive.css",
                     "~/Content/Site.css"
                      ));



            
            bundles.Add(new ScriptBundle("~/bundles/flashmessage").Include(
                "~/Scripts/jquery.cookie.js",
                "~/Scripts/jquery.flashmessage.js"
            ));

              bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
              "~/Scripts/kendo/kendo.all.min.js",
              // "~/Scripts/kendo/kendo.timezones.min.js", // uncomment if using the Scheduler
              "~/Scripts/kendo/kendo.aspnetmvc.min.js"));

            bundles.Add(new StyleBundle("~/Content/kendo/css").Include(
             "~/Content/kendo/kendo.common-bootstrap.min.css",
             "~/Content/kendo/kendo.default.min.css",
             "~/Content/kendo/kendo.rtl.min.css"));





            bundles.IgnoreList.Clear();
        }
    }
}
