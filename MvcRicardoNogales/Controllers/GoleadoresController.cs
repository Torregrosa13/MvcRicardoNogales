using Microsoft.AspNetCore.Mvc;
using MvcRicardoNogales.Models;
using MvcRicardoNogales.Services;

namespace MvcRicardoNogales.Controllers
{
    public class GoleadoresController : Controller
    {
        private readonly ApiService _apiService;

        public GoleadoresController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("TOKEN");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Login");
            }

            var goleadores = await _apiService.GetAsync<List<GoleadorDTO>>("api/goleadores", token);
            return View(goleadores);
        }
    }
}
