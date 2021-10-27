using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using URLShortener.Models;

namespace URLShortener.Areas.Identity.Authorization
{
    public class UrlAdministratorsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, UserUrl>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, UserUrl resource)
        {

            // admins can do anything
            if (context.User.IsInRole(Constants.UrlAdministratorsRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

