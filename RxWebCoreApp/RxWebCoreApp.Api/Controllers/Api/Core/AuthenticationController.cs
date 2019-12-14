using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RxWebCoreApp.Infrastructure.Security;
using RxWebCoreApp.Models.Main;
using RxWebCoreApp.Models.ViewModels;
using RxWebCoreApp.UnitOfWork.Main;
using RxWeb.Core.Security.Cryptography;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace RxWebCoreApp.Api.Controllers
{
	[ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private ILoginUow LoginUow { get; set; }
        private IApplicationTokenProvider ApplicationTokenProvider { get; set; }
        private IPasswordHash PasswordHash { get; set; }

        public AuthenticationController(ILoginUow loginUow, IApplicationTokenProvider tokenProvider, IPasswordHash passwordHash)
        {
            this.LoginUow = loginUow;
            ApplicationTokenProvider = tokenProvider;
            PasswordHash = passwordHash;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(AuthenticationModel authentication)
        {
            var user = await LoginUow.Repository<vUser>().SingleOrDefaultAsync(t => t.UserName == authentication.UserName && !t.LoginBlocked);
            if (user != null && PasswordHash.VerifySignature(authentication.Password, user.Password, user.Salt))
            {
                var token = await ApplicationTokenProvider.GetTokenAsync(user);
                return Ok(token);
            }
            else
                return BadRequest();
        }
    }
}

