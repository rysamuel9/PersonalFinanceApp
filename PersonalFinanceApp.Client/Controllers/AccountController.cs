using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalFinanceApp.Client.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PersonalFinanceApp.Client.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _client;

        public AccountController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:7253/api/Account/");
        }

        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var requestData = new
            {
                Username = model.Username,
                Password = model.Password
            };

            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync("login", content);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                var tokenObject = JsonConvert.DeserializeObject<JObject>(token);
                var tokenValue = tokenObject["token"].ToString();

                var claims = new List<Claim>
                {
                    new Claim("AccessToken", tokenValue)
                };

                var identity = new ClaimsIdentity(claims, "ApplicationCookie");

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(principal);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View(model);
            }
        }


    }
}
