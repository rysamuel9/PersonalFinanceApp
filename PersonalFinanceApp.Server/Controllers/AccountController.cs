using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceApp.Server.DTO;
using PersonalFinanceApp.Server.Models;
using PersonalFinanceApp.Server.Repositories.IRepository;

namespace PersonalFinanceApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AccountRegisterDTO model)
        {
            try
            {
                var user = await _userService.RegisterAsync(model.Username, model.Email, model.PasswordHash, model.FirstName);
                return Ok(new { user.Email, user.Username });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AccountLoginDTO model)
        {
            try
            {
                var token = await _userService.LoginAsync(model.Username, model.Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("logout"), Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();
                Response.Cookies.Delete(".AspNetCore.Identity.Application");

                return Ok(new { message = "Logout successful." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
