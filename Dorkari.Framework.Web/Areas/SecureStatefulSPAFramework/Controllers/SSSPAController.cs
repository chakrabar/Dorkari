using System.Web.Mvc;

namespace Dorkari.Framework.Web.Areas.SecureStatefulSPAFramework.Controllers
{
    public class SSSPAController : Controller
    {
        // GET: SecureStatefulSPAFramework/SSSPA
        public ActionResult Index()
        {
            return Content("Welcome to the Secure Stateful SPA aka s3pa");
        }
    }
}