using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ContactManagerApi.Utils.Policies
{
    public class RolAdministradorCubanoHandler : AuthorizationHandler<RolAdministradorCubanoRequierment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolAdministradorCubanoRequierment requirement)
        {
            var roles = context.User.Claims.First(c => c.Type == "Roles").Value;
            var contry = context.User.Claims.First(c => c.Type == ClaimTypes.Country).Value;
           if (roles.Contains("Administrator") && contry.Equals("CU"))
           {
                context.Succeed(requirement);
           }

           //return Task.CompletedTask;
           return Task.FromResult(0);
        }
    }
}
