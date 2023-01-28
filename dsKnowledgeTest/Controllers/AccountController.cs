using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using dsKnowledgeTest.Services;
using dsKnowledgeTest.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace dsKnowledgeTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        public const string DEFAULT_PASSWORD = "12345678";

        private readonly IAccountService _accountService;
        private readonly IEmailService _emailService;

        public AccountController(IAccountService accountService, IEmailService emailService)
        {
            _accountService = accountService;
            _emailService = emailService;
        }
        [Route("Login")]
        [HttpPost]
        public async Task<ObjectResult> Login(LoginUserViewModel loginUser)
        {
            var user = await _accountService.Login(loginUser);
            if (user == null) return BadRequest(user);
            var token = await Authenticate(user);
            //Response.Headers.Add("Bearer", token);
            return Ok(token);

            //return Ok(user);

        }

        /*[Route("Logout")]
        [HttpGet]
        public async Task<ObjectResult> Logout()
        {

        }/*

        /*[Route("IsAuthenticated")]
        [HttpGet]
        public async Task<ObjectResult> IsAuthenticated()
        {
            
        }*/

        [Route("Register")]
        [HttpPost]
        public async Task<ObjectResult> Register(RegisterUserViewModel registerUser)
        {
            var user = await _accountService.Register(registerUser);
            if (user == null) return BadRequest(user);
            await _emailService.SendEmailAsync(user.Email, "Пароль для входа", DEFAULT_PASSWORD);
            return Ok(user);
        }
        private async Task<string> Authenticate(UserViewModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.RoleName)
            };

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)), // время действия 2 минуты
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private string? AuthenticatedUserName()
        {
            var authenticatedUser = HttpContext.User.Identity;
            return (authenticatedUser is { IsAuthenticated: true }) ? authenticatedUser.Name : null;
        }
    }
}
