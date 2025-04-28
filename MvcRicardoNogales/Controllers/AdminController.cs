using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcRicardoNogales.Models;
using MvcRicardoNogales.Services;
using Newtonsoft.Json.Linq;

namespace MvcRicardoNogales.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApiService _apiService;

        public AdminController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Métodos para: Crear equipo, jugador, grupo, asignar equipos, actualizar resultados

        // GET: Formulario para crear equipo
        public IActionResult CreateEquipo()
        {
            return View();
        }

        // POST: Guardar equipo
        [HttpPost]
        public async Task<IActionResult> CreateEquipo(EquipoDTO equipo)
        {
            try
            {
                var token = HttpContext.Session.GetString("TOKEN");
                await _apiService.PostAsync<EquipoDTO, object>("api/equipos", equipo, token);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "Error al crear equipo.";
                return View();
            }
        }

        // GET: Formulario para crear jugador
        public async Task<IActionResult> CreateJugador()
        {
            var token = HttpContext.Session.GetString("TOKEN");
            var equipos = await _apiService.GetAsync<List<EquipoDTO>>("api/equipos", token);
            ViewBag.Equipos = equipos;
            return View();
        }

        // POST: Guardar jugador
        [HttpPost]
        public async Task<IActionResult> CreateJugador(JugadorDTO jugador)
        {
            try
            {
                var token = HttpContext.Session.GetString("TOKEN");
                await _apiService.PostAsync<JugadorDTO, object>("api/jugadores", jugador, token);
                return RedirectToAction("Index");
            }
            catch
            {
                var token = HttpContext.Session.GetString("TOKEN");
                ViewBag.Error = "Error al crear jugador.";
                var equipos = await _apiService.GetAsync<List<EquipoDTO>>("api/equipos", token);
                ViewBag.Equipos = equipos;
                return View();
            }
        }

        // GET: Formulario para crear grupo
        public IActionResult CreateGrupo()
        {
            return View();
        }

        // POST: Guardar grupo
        [HttpPost]
        public async Task<IActionResult> CreateGrupo(GrupoDTO grupo)
        {
            try
            {
                var token = HttpContext.Session.GetString("TOKEN");
                await _apiService.PostAsync<GrupoDTO, object>("api/grupos", grupo, token);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "Error al crear grupo.";
                return View();
            }
        }

        // GET: Formulario para asignar equipo a grupo
        public async Task<IActionResult> AsignarEquipoGrupo()
        {
            var token = HttpContext.Session.GetString("TOKEN");
            var equipos = await _apiService.GetAsync<List<EquipoDTO>>("api/equipos", token);
            var grupos = await _apiService.GetAsync<List<GrupoDTO>>("api/grupos", token);

            ViewBag.Equipos = equipos;
            ViewBag.Grupos = grupos;
            return View();
        }

        // POST: Asignar equipo a grupo
        [HttpPost]
        public async Task<IActionResult> AsignarEquipoGrupo(EquipoGrupoDTO model)
        {
            var token = HttpContext.Session.GetString("TOKEN");

            try
            {
                // 1. Asignar el equipo al grupo
                await _apiService.PostAsync<EquipoGrupoDTO, object>("api/equipogrupo", model, token);

                // 2. Comprobar cuántos equipos hay en el grupo
                var equiposGrupo = await _apiService.GetAsync<List<EquipoGrupoDTO>>("api/equipogrupo", token);
                var equiposEnEsteGrupo = equiposGrupo.Where(eg => eg.IdGrupo == model.IdGrupo).Select(eg => eg.IdEquipo).ToList();

                // 3. Si hay exactamente 3 equipos, crear los 3 partidos
                if (equiposEnEsteGrupo.Count == 3)
                {
                    // Crear combinaciones de partidos
                    var partidos = new List<object>
            {
                new { IdEquipoLocal = equiposEnEsteGrupo[0], IdEquipoVisitante = equiposEnEsteGrupo[1], FechaHora = DateTime.Now.AddDays(1), Fase = "Triangular" },
                new { IdEquipoLocal = equiposEnEsteGrupo[0], IdEquipoVisitante = equiposEnEsteGrupo[2], FechaHora = DateTime.Now.AddDays(2), Fase = "Triangular" },
                new { IdEquipoLocal = equiposEnEsteGrupo[1], IdEquipoVisitante = equiposEnEsteGrupo[2], FechaHora = DateTime.Now.AddDays(3), Fase = "Triangular" }
            };

                    foreach (var partido in partidos)
                    {
                        await _apiService.PostAsync<object, object>("api/partidos", partido, token);
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "Error al asignar equipo al grupo.";
                var equipos = await _apiService.GetAsync<List<EquipoDTO>>("api/equipos", token);
                var grupos = await _apiService.GetAsync<List<GrupoDTO>>("api/grupos", token);

                ViewBag.Equipos = equipos;
                ViewBag.Grupos = grupos;
                return View();
            }
        }


    }
}
