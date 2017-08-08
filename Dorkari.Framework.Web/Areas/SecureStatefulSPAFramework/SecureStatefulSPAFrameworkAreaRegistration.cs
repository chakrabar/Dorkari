using System.Web.Mvc;

namespace Dorkari.Framework.Web.Areas.SecureStatefulSPAFramework
{
    public class SecureStatefulSPAFrameworkAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SecureStatefulSPAFramework";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SecureStatefulSPAFramework_default",
                "s3pa/{controller}/{action}/{id}",
                new { controller = "SSSPA", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}