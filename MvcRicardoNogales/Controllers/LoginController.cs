using Microsoft.AspNetCore.Mvc;
using MvcRicardoNogales.Models;
using MvcRicardoNogales.Services;
using Newtonsoft.Json.Linq;

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
    }
}
