using System;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Saned.Delco.Admin.Extensions
{
    public static class OvverideActionLink
    {
        public static MvcHtmlString RawActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            var repId = Guid.NewGuid().ToString();
            var lnk = ajaxHelper.ActionLink(repId, actionName, controllerName, routeValues, ajaxOptions, htmlAttributes);
            return MvcHtmlString.Create(lnk.ToString().Replace(repId, linkText));
        } 
    }
}