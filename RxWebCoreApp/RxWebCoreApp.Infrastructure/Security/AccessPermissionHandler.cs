using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using RxWebCoreApp.Infrastructure.Singleton;
using RxWebCoreApp.UnitOfWork.Main;
using RxWeb.Core.Security.Authorization;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;
using System;

namespace RxWebCoreApp.Infrastructure.Security
{
    public class AccessPermissionHandler : AuthorizationHandler<AccessPermissionRequirement>
    {
        private UserAccessConfigInfo UserAccessConfig { get; set; }
        private IHttpContextAccessor ContextAccessor { get; set; }
        public AccessPermissionHandler(UserAccessConfigInfo userAccessConfig, IHttpContextAccessor contextAccessor)
        {
            UserAccessConfig = userAccessConfig;
            ContextAccessor = contextAccessor;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessPermissionRequirement requirement)
        {
            var loginUow = ContextAccessor.HttpContext.RequestServices.GetService(typeof(ILoginUow)) as ILoginUow;
            var requestMethod = ContextAccessor.HttpContext.Request.Method.ToLower();
            var haveAccess = await UserAccessConfig.GetAccessInfoAsync(GetUserId(context.User), requirement.ApplicationModuleId, requestMethod, loginUow);
            if (haveAccess)
                context.Succeed(requirement);
            else
                context.Fail();
        }

        private int GetUserId(ClaimsPrincipal user)
        {
            var userId = 0;
            if (user.Claims != null)
            {
                var claim = user.Claims.SingleOrDefault(t => t.Type == ClaimTypes.NameIdentifier);
                if (claim != null)
                    userId = Convert.ToInt32(claim.Value);
            }
            return userId;
        }
    }
}


