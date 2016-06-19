using System.Web.Mvc;

namespace McKaren
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "McKaren.Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "McKaren.Admin",  action = "Index", id = UrlParameter.Optional },
                new string[] { "McKaren.Admin.Controllers" }
            );
        }
    }
}
