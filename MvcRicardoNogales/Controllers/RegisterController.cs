using Microsoft.AspNetCore.Mvc;
using MvcRicardoNogales.Models;
using MvcRicardoNogales.Services;

namespace MvcRicardoNogales.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApiService _apiService;

        public RegisterController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterDTO register)
        {
            try
            {
                await _apiService.PostAsync<RegisterDTO, object>("api/auth/register", register);
                return RedirectToAction("Index", "Login");
            }
            catch
            {
                ViewBag.Error = "Error al registrar. Puede que el email ya esté registrado.";
                return View();
            }
        }
    }
}
