using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcRicardoNogales.Models;
using MvcRicardoNogales.Services;

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

        public IActionResult CreateEquipo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEquipo(EquipoDTO equipo)
        {
            var token = HttpContext.Session.GetString("TOKEN");
            try
            {
                await _apiService.PostAsync<EquipoDTO, object>("api/equipos", equipo, token);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "Error al crear equipo.";
                return View();
            }
        }

        public async Task<IActionResult> CreateJugador()
        {
            var token = HttpContext.Session.GetString("TOKEN");
            var equipos = await _apiService.GetAsync<List<EquipoDTO>>("api/equipos", token);
            ViewBag.Equipos = equipos;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateJugador(JugadorDTO jugador)
        {
            var token = HttpContext.Session.GetString("TOKEN");
            try
            {
                await _apiService.PostAsync<JugadorDTO, object>("api/jugadores", jugador, token);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "Error al crear jugador.";
                var equipos = await _apiService.GetAsync<List<EquipoDTO>>("api/equipos", token);
                ViewBag.Equipos = equipos;
                return View();
            }
        }

        public IActionResult CreateGrupo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateGrupo(GrupoDTO grupo)
        {
            var token = HttpContext.Session.GetString("TOKEN");
            try
            {
                await _apiService.PostAsync<GrupoDTO, object>("api/grupos", grupo, token);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "Error al crear grupo.";
                return View();
            }
        }

        public async Task<IActionResult> AsignarEquipoGrupo()
        {
            var token = HttpContext.Session.GetString("TOKEN");
            var equipos = await _apiService.GetAsync<List<EquipoDTO>>("api/equipos", token);
            var grupos = await _apiService.GetAsync<List<GrupoDTO>>("api/grupos", token);

            ViewBag.Equipos = equipos;
            ViewBag.Grupos = grupos;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AsignarEquipoGrupo(EquipoGrupoDTO model)
        {
            var token = HttpContext.Session.GetString("TOKEN");

            try
            {
                await _apiService.PostAsync<EquipoGrupoDTO, object>("api/equipogrupo", model, token);

                var equiposGrupo = await _apiService.GetAsync<List<EquipoGrupoDTO>>("api/equipogrupo", token);
                var equiposEnEsteGrupo = equiposGrupo.Where(eg => eg.IdGrupo == model.IdGrupo).Select(eg => eg.IdEquipo).ToList();

                if (equiposEnEsteGrupo.Count == 3)
                {
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

        [HttpGet]
        public async Task<IActionResult> ActualizarResultado()
        {
            var token = HttpContext.Session.GetString("TOKEN");
            var partidos = await _apiService.GetAsync<List<PartidoDTO>>("api/partidos", token);

            var model = new ActualizarResultadoViewModel
            {
                PartidosDisponibles = partidos
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CargarDatosPartido(ActualizarResultadoViewModel model)
        {
            var token = HttpContext.Session.GetString("TOKEN");

            var partido = await _apiService.GetAsync<PartidoDTO>("api/partidos/" + model.IdPartidoSeleccionado, token);
            var jugadores = await _apiService.GetAsync<List<JugadorDTO>>("api/jugadores", token);

            var jugadoresLocal = jugadores.Where(j => j.IdEquipo == partido.IdEquipoLocal).ToList();
            var jugadoresVisitante = jugadores.Where(j => j.IdEquipo == partido.IdEquipoVisitante).ToList();

            var datos = new ActualizarResultadoViewModel
            {
                IdPartido = partido.IdPartido,
                NombreEquipoLocal = partido.NombreEquipoLocal,
                NombreEquipoVisitante = partido.NombreEquipoVisitante,
                JugadoresLocal = jugadoresLocal,
                JugadoresVisitante = jugadoresVisitante,
                PartidosDisponibles = await _apiService.GetAsync<List<PartidoDTO>>("api/partidos", token),
                IdPartidoSeleccionado = partido.IdPartido
            };

            return View("ActualizarResultado", datos);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarResultadoConfirmado(ActualizarResultadoViewModel datos)
        {
            var token = HttpContext.Session.GetString("TOKEN");

            var dto = new ActualizarResultadoDTO
            {
                IdPartido = datos.IdPartido,
                GolesLocal = datos.GolesLocal,
                GolesVisitante = datos.GolesVisitante,
                IdsGoleadoresLocal = datos.IdsGoleadoresLocal,
                IdsGoleadoresVisitante = datos.IdsGoleadoresVisitante,
                Tarjetas = datos.Tarjetas
            };

            try
            {
                await _apiService.PutAsync<ActualizarResultadoDTO, object>($"api/partidos/{dto.IdPartido}/resultado", dto, token);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "Error al actualizar resultado.";
                return RedirectToAction("ActualizarResultado");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerJugadores(int idPartido)
        {
            var token = HttpContext.Session.GetString("TOKEN");

            var partido = await _apiService.GetAsync<PartidoDTO>($"api/partidos/{idPartido}", token);
            var jugadoresLocal = await _apiService.GetAsync<List<JugadorDTO>>($"api/jugadores/equipo/{partido.IdEquipoLocal}", token);
            var jugadoresVisitante = await _apiService.GetAsync<List<JugadorDTO>>($"api/jugadores/equipo/{partido.IdEquipoVisitante}", token);

            return Json(new { jugadoresLocal, jugadoresVisitante, partido });
        }
    }
}
