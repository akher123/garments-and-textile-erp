using System.Web.Optimization;

namespace SCERP.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                            "~/Scripts/jquery-{version}.js",
                            "~/Scripts/jquery.unobtrusive-ajax.js"
                            ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                     "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/others").Include(
                    "~/Scripts/Plugins/uploadify/swfobject.js",
                    "~/Scripts/Plugins/uploadify/jquery.uploadify.v2.1.4.js",
                    "~/Scripts/jquery.ui.timepicker.js",
                    "~/Scripts/Common/Calendar.js",    
                    "~/Scripts/SCERP/comboBox.js",
                    "~/Scripts/jquery.iframe-transport.js",
                    "~/Scripts/jQuery-File-Upload/jquery.uploadfile.min.js",
                    "~/Scripts/menu.js",
                     "~/Scripts/fullcalendar.js",
                   "~/Scripts/handsontable/handsontable.full.min.js",
                    "~/Scripts/showLoading/jquery.showLoading.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));


            bundles.Add(new ScriptBundle("~/bundles/scerp").Include(
                "~/Scripts/SCERP/ajax.js",
                "~/Scripts/jquery.alert-confirm.js",
                "~/Scripts/SCERP/common.js",
                 "~/Scripts/SCERP/Merchandising/mrcSearch.js"
                
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/themes/base/minified/jquery.ui.timepicker.css",
                "~/Scripts/Plugins/uploadify/uploadify.css",
                "~/Scripts/jQuery-File-Upload/uploadfile.css",
                 "~/Content/themes/base/jquery-ui.css",
                "~/Content/themes/base/jquery.ui.core.css",
                "~/Content/themes/base/jquery.ui.resizable.css",
                "~/Content/themes/base/jquery.ui.selectable.css",
                "~/Content/themes/base/jquery.ui.accordion.css",
                "~/Content/themes/base/jquery.ui.autocomplete.css",
                "~/Content/themes/base/jquery.ui.button.css",
                "~/Content/themes/base/jquery.ui.dialog.css",
                "~/Content/themes/base/jquery.ui.slider.css",
                "~/Content/themes/base/jquery.ui.tabs.css",
                "~/Content/themes/base/jquery.ui.datepicker.css",
                "~/Content/themes/base/jquery.ui.progressbar.css",
                "~/Content/themes/base/jquery.ui.theme.css",
                 "~/Content/fullcalendar.css",
                "~/Content/Common/webGridScript.css",
                "~/Content/Common/tooltip.css",
                "~/Content/site.css",
                "~/Content/Master.css",
                "~/Content/Menu.css",
                "~/Content/Common/formControlsStyle.css",
                  "~/Content/Common/comboBoxStyle.css",
                "~/Scripts/showLoading/css/showLoading.css"
               , "~/Content/handsontable/handsontable.full.min.css"
                ));


            // SCERP application 
         

            bundles.Add(new ScriptBundle("~/bundles/Scripts/SCERP/EmployeeLeaveApprovaljs").Include(
                           "~/Scripts/SCERP/EmployeeLeaveApproval.js"
                           ));
            bundles.Add(new ScriptBundle("~/bundles/Scripts/AppScripts/districtjs").Include(
                           "~/Scripts/AppScripts/district.js"
                           ));

            //Merchandising Start 
            bundles.Add(new ScriptBundle("~/bundles/SCERP/Merchandising").Include(
                            "~/Scripts/SCERP/Merchandising/Buyer.js"
                           )); 
        }

    }
}
