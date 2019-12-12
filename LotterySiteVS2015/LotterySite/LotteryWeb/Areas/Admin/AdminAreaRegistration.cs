using System.Web.Mvc;

namespace LotteryWeb.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        // 将控制器命名空间传入 避免控制器重复 
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                , new string[] { "LotteryWeb.Areas.Admin.Controllers" }
            );
        }
    }
}
