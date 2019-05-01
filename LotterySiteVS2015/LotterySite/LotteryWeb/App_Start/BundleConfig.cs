using System.Web;
using System.Web.Optimization;

namespace LotteryWeb
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {
            // ======= css ========== 
            bundles.Add(new StyleBundle("~/HPluscss").Include(
                        // "~/StaticPage/backstage/css/bootstrap.css",
                        "~/Content/backstage/css/font-awesome-{version}.css",
                        "~/Content/backstage/css/animate.css",
                        "~/Content/backstage/css/style-{version}.css",
                        "~/Content/backstage/css/plugins/iCheck/custom.css"
                        ));

            bundles.Add(new StyleBundle("~/HPlusLogincss").Include(
                 "~/Content/backstage/css/login.css"
                ));


            // ====== js =======
            // 公用js
            bundles.Add(new ScriptBundle("~/HPlusPublicjs").Include(
                        //"~/StaticPage/backstage/js/jquery-{version}.js"
                        "~/Content/backstage/js/jquery-{version}.js"
                        ));

            // jshelper
            bundles.Add(new ScriptBundle("~/jshelper").Include(
                        "~/Content/jshelper-{version}.js"
                        ));

            // layer 弹窗
            bundles.Add(new ScriptBundle("~/layerjs").Include(
                        "~/Content/layer-v2.4/layer/layer.js"
                        ));


            // home 页面
            bundles.Add(new ScriptBundle("~/HPlusHomejs").Include(
                        "~/Content/backstage/js/plugins/metisMenu/jquery.metisMenu.js",
                        "~/Content/backstage/js/plugins/slimscroll/jquery.slimscroll.js",
                        //"~/Content/backstage/js/plugins/layer/layer.js",
                        "~/Content/backstage/js/hplus-{version}.js",
                        "~/Content/backstage/js/contabs.js"
                        //,"~/Content/backstage/js/plugins/pace/pacess.js"
                        ));

            // ====== 插件 =======
            // bootstrapcss
            bundles.Add(new StyleBundle("~/bootstrapcss").Include(
                "~/Content/backstage/css/bootstrap.css"
                ));
            // bootstrapjs
            bundles.Add(new ScriptBundle("~/bootstrapjs").Include(
                        "~/Content/backstage/js/bootstrap-{version}.js"
                ));

            // jqGrid css
            bundles.Add(new StyleBundle("~/HPlusjqGridcss").Include(
                 //"~/Scripts/jqGrid_JS_5.1.1/css/bootstrap.css"
                 "~/Content/jqGrid_JS_5.1.1/css/ui.jqgrid-bootstrap.css"
                ));
            // jqGrid js
            bundles.Add(new ScriptBundle("~/HPlusjqGridjs").Include(
                        //"~/Scripts/jqGrid_JS_5.1.1/js/jquery-{version}.js",
                        "~/Content/jqGrid_JS_5.1.1/js/i18n/grid.locale-cn.js",
                        "~/Content/jqGrid_JS_5.1.1/js/jquery.jqGrid.js"
                        //"~/Scripts/jqGrid_JS_5.1.1/js/bootstrap.js"
                        ));
            ////上传图片
            //bundles.Add(new ScriptBundle("~/ajaxfileupload").Include(
            //          "~/Scripts/jquery.ajaxfileupload.js"
            //          ));

            // datepicker
            bundles.Add(new StyleBundle("~/datepickercss").Include(
                "~/Content/backstage/css/plugins/datapicker/datepicker3.css"
                ));
            // datepicker
            bundles.Add(new ScriptBundle("~/datepickerjs").Include(
                        "~/Content/backstage/js/plugins/datapicker/bootstrap-datepicker.js"
                ));

            //// ckeditor 编辑器
            //bundles.Add(new ScriptBundle("~/ckeditorjs").Include(
            //            "~/Scripts/ckeditor/ckeditor.js"
            //            ));

            // jsrender
            bundles.Add(new ScriptBundle("~/jsrenderjs").Include(
                        "~/Content/jsrender.min.js"
                        ));

            //<link href="~/Scripts/zTree_v3-master/css/demo.css" rel="stylesheet" />
            //<link href="~/Scripts/zTree_v3-master/css/metroStyle/metroStyle.css" rel="stylesheet" />
            //<script src="~/Scripts/zTree_v3-master/js/jquery.ztree.core.js"></script>
            //<script src="~/Scripts/zTree_v3-master/js/jquery.ztree.excheck.js"></script>
            // ztree
            bundles.Add(new StyleBundle("~/zTreevcss").Include(
                "~/Content/zTree_v3-master/css/demo.css",
                "~/Content/zTree_v3-master/css/metroStyle/metroStyle.css"
                ));
            // ztree
            bundles.Add(new ScriptBundle("~/zTreevjs").Include(
                        "~/Content/zTree_v3-master/js/jquery.ztree.core.js",
                        "~/Content/zTree_v3-master/js/jquery.ztree.excheck.js"
                ));

        }


    }
}