using System.Security.Claims;
using dsKnowledgeTest.Services;
using dsKnowledgeTest.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dsKnowledgeTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [Route("Login")]
        [HttpPost]
        public async Task<ObjectResult> Login(LoginUserViewModel loginUser)
        {
            var user = await _accountService.Login(loginUser);
            if (user == null) return BadRequest(user);
            await Authenticate(user);
            return Ok(user);

        }

        [Route("Logout")]
        [HttpGet]
        public async Task<ObjectResult> Logout()
        {
            var logoutUserName = await LogoutUser();
            if (logoutUserName == null) return BadRequest(logoutUserName);
            return Ok(logoutUserName);
        }

        /*[Route("IsAuthenticated")]
        [HttpGet]
        public async Task<ObjectResult> IsAuthenticated()
        {
            var userEmail = AuthenticatedUserName();
            if (userEmail == null) return BadRequest(userEmail);
            return Ok(await _userService.GetByEmailAsync(userEmail));
        }*/

        [Route("Register")]
        [HttpPost]
        public async Task<ObjectResult> Register(RegisterUserViewModel registerUser)
        {
            var user = await _accountService.Register(registerUser);
            if (user == null) return BadRequest(user);
            await Authenticate(user);
            return Ok(user);

        }
        private async Task Authenticate(UserViewModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.RoleName)
            };

            var id = new ClaimsIdentity(
                claims,
                "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(id));
        }

        private async Task<string?> LogoutUser()
        {
            var logoutUser = HttpContext.User.Identity;

            if (logoutUser is not { IsAuthenticated: true }) return null;

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return logoutUser.Name;

        }

        private string? AuthenticatedUserName()
        {
            var authenticatedUser = HttpContext.User.Identity;
            return (authenticatedUser is { IsAuthenticated: true }) ? authenticatedUser.Name : null;
        }
    }
}
