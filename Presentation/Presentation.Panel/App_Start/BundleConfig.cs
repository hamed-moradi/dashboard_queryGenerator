using System.Collections.Generic;
using System.Web.Optimization;

namespace Presentation.Panel
{
    internal class NonOrderingBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            var bundle1 = new ScriptBundle("~/bundles/requierdScripts").Include(
                        "~/Scripts/jQuery-2.1.4.min.js",
                        "~/Scripts/jquery-ui.min.js",
                        "~/Scripts/jquery.loading.block.js",
                        "~/Scripts/app.min.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/respond.js");
            bundle1.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle1);

            var bundle2 = new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*");
            bundle2.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle2);

            var bundle3 = new StyleBundle("~/Styles/css").Include(
                      "~/Styles/bootstrap.min.css",
                      "~/Styles/font-awesome.min.css",
                      "~/Styles/ionicons.min.css",
                      "~/Styles/AdminLTE.min.css",
                      "~/Styles/Skins/skin-red.min.css",
                      "~/Styles/blue.min.css",
                      "~/Styles/bootstrap3-wysihtml5.min.css",
                      "~/Styles/fonts-fa.min.css",
                      "~/Styles/bootstrap-rtl.min.css");
            bundle3.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle3);

            var bundle4 = new ScriptBundle("~/bundles/GridScripts").Include(
                        "~/Scripts/jalaali.min.js",
                        "~/Scripts/jquery.Bootstrap-PersianDateTimePicker.min.js");
            bundle4.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle4);

            var bundle5 = new ScriptBundle("~/bundles/GridCss").Include(
                        "~/Styles/bootstrap-theme.min.css",
                        "~/Styles/jquery.Bootstrap-PersianDateTimePicker.css");
            bundle5.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle5);
        }
    }
}
