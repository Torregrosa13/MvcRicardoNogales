using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MvcRicardoNogales.Models;
using MvcRicardoNogales.Services;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace MvcRicardoNogales.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApiService _apiService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginController(ApiService apiService, IHttpContextAccessor httpContextAccessor)
        {
            _apiService = apiService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginDTO loginDto)
        {
            try
            {
                var response = await _apiService.PostAsync<LoginDTO, JObject>("api/auth/login", loginDto);

                var token = response["token"]?.ToString();

                if (!string.IsNullOrEmpty(token))
                {
                    _httpContextAccessor.HttpContext.Session.SetString("TOKEN", token);

                    // LEEMOS EL TOKEN PARA SACAR LOS CLAIMS
                    var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);

                    var email = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    var rol = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                    var idUsuario = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, idUsuario ?? ""),
                        new Claim(ClaimTypes.Email, email ?? ""),
                        new Claim(ClaimTypes.Role, rol ?? "")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Login incorrecto.";
                    return View();
                }
            }
            catch
            {
                ViewBag.Error = "Error al intentar iniciar sesión.";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _httpContextAccessor.HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}
