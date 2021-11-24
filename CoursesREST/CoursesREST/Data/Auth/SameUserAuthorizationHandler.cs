 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoursesREST.Data.Auth.Model;
using Microsoft.AspNetCore.Authorization;

namespace CoursesREST.Data.Auth
{
    public class SameUserAuthorizationHandler : AuthorizationHandler<SameUserRequirement, IUserOwnedResource>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            SameUserRequirement requirement,
            IUserOwnedResource resource)
        {
            if (context.User.IsInRole(DemoRestUserRole.Admin) || context.User.FindFirst(CustomClaim.UserId).Value == resource.UserId)
            {
                context.Succeed(requirement);
            }



            return Task.CompletedTask;
        }
    }

    public record SameUserRequirement : IAuthorizationRequirement;
}
