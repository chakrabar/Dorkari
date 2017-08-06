using System.IO;
using System.Web.Mvc;

namespace Dorkari.Framework.Web.Helpers
{
    public static class ControllerExtensions
    {
        public static string RenderPartialViewAsString(this Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            using (var writer = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, writer);
                viewResult.View.Render(viewContext, writer);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
                return writer.GetStringBuilder().ToString();
            }
        }

        public static string RenderPartialViewAsString(this Controller controller, PartialViewResult partialViewResult)
        {
            return RenderPartialViewAsString(controller, partialViewResult.ViewName, partialViewResult.Model);
        }
    }
}