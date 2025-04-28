using Microsoft.AspNetCore.Mvc;
using MvcRicardoNogales.Models;
using MvcRicardoNogales.Services;

namespace MvcRicardoNogales.Controllers
{
    public class EquiposController : Controller
    {
        private readonly ApiService _apiService;

        public EquiposController(ApiService apiService)
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

            var equipos = await _apiService.GetAsync<List<EquipoDTO>>("api/equipos", token);
            return View(equipos);
        }


        public async Task<IActionResult> Details(int id)
        {
            var token = HttpContext.Session.GetString("TOKEN");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Login");
            }

            var equipo = await _apiService.GetAsync<EquipoDetalleDTO>($"api/equipos/{id}", token);
            return View(equipo);
        }
    }
}
