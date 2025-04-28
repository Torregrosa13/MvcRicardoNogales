using Microsoft.AspNetCore.Mvc;
using MvcRicardoNogales.Models;
using MvcRicardoNogales.Services;

namespace MvcRicardoNogales.Controllers
{
    public class PartidosController : Controller
    {
        private readonly ApiService _apiService;

        public PartidosController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Details(int id)
        {
            var token = HttpContext.Session.GetString("TOKEN");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Login");
            }

            var partido = await _apiService.GetAsync<PartidoDetalleDTO>($"api/partidos/{id}", token);
            return View(partido);
        }
    }
}
