using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace URLShortener.Areas.Identity.Attributes
{
    public class AllowAnonymousOnlyAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity is {IsAuthenticated: true})
            {
                filterContext.Result = new RedirectToActionResult("Index", "Urls", null);
            }
        }
    }
}
