using System.Web.Mvc;

namespace StudentInternshipManagement.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        //public override string AreaName => "Admin";

        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //code gốc lỗi
            //context.MapRoute(
            //    "Admin_default",
            //    "Admin/{controller}/{action}/{id}",
            //    new {action = "Index", id = UrlParameter.Optional},
            //    new[] {"StudentInternshipManagement.Areas.Admin.Controllers"}
            //);

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}