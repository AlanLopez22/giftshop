using System.Web.Optimization;

namespace GiftShop
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                      "~/Scripts/umd/poper.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularjs").Include(
                    "~/Scripts/angular.js",
                    "~/Scripts/angular-animate.js",
                    "~/Scripts/angular-aria.js",
                    "~/Scripts/angular-cookies.js",
                    "~/Scripts/angular-route.js",
                    "~/Scripts/angular-ui-router.js",
                    "~/Scripts/angular-sanitize.js",
                    "~/Scripts/angular-messages.js",
                    "~/Scripts/angular-local-storage.js",
                    "~/Scripts/angular-toastr.tpls.js",
                    "~/Scripts/angular-base64.js",
                    "~/Scripts/ng-file-upload-shim.js",
                    "~/Scripts/ng-file-upload.js",
                    "~/Scripts/angular-ui/ui-bootstrap-tpls.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                    "~/App/modules/*.js",
                    "~/App/app.js",
                    "~/App/services/*.js",
                    "~/App/directives/*.js")
                    .IncludeDirectory("~/App/controllers", "*.js", true));

            bundles.Add(new StyleBundle("~/bundles/styles").Include(
                      "~/Content/fontawesome-all.css",
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-grid.css",
                      "~/Content/bootstrap-reboot.css",
                      "~/Content/ui-bootstrap-csp.css",
                      "~/Content/angular-toastr.css",
                      "~/Content/giftshop.css"));
        }
    }
}
