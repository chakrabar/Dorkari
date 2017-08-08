using Dorkari.Framework.Web.Areas.SecureStatefulSPAFramework.Models;
using System.Web.Mvc;
using Dorkari.Framework.Web.Helpers;
using Dorkari.Framework.Web.Areas.SecureStatefulSPAFramework.Models.Enums;

namespace Dorkari.Framework.Web.Areas.SecureStatefulSPAFramework.Controllers
{
    public class SSSPAController : Controller
    {
        readonly BookingSessionAdapter _bookingAdaper;

        public SSSPAController()
        {
            _bookingAdaper = new BookingSessionAdapter();
        }

        // GET: sp3a/SSSPA
        public ActionResult Index()
        {
            return Content("Welcome to the Secure Stateful SPA aka s3pa");
        }

        public ActionResult IndexToBeFixed(string email = "")
        {
            var initialViewAndModel = _bookingAdaper.UpdateSession(email);
            return View("BookingPageContainer",
                new S3paJsonModelBase
                {
                    NextPageHtml = this.RenderPartialViewAsString(initialViewAndModel.Item1, initialViewAndModel.Item2),
                    NextStepString = initialViewAndModel.Item1
                }
            );
        }

        private void UpdateViewData(S3paJsonModelBase jsonData) //to be called from ajax requests
        {
            if (jsonData != null && jsonData.ResponseStatus.Equals(BookingReponseStatus.Success))
            {
                var currentViewAndModel = _bookingAdaper.GetCurrentViewDetails();
                jsonData.NextPageHtml = this.RenderPartialViewAsString(currentViewAndModel.Item1, currentViewAndModel.Item2);
            }
        }
    }
}