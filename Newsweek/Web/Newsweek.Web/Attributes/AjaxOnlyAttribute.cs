namespace Newsweek.Web.Attributes
{
    using System;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    [AttributeUsage(AttributeTargets.Method)]
    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        private const string RequestedWithHeader = "X-Requested-With";
        private const string XmlHttpRequest = "XMLHttpRequest";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Headers[RequestedWithHeader] == XmlHttpRequest)
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                filterContext.HttpContext.Response.StatusCode = 404;
                filterContext.Result = new NotFoundResult();
            }
        }
    }
}
