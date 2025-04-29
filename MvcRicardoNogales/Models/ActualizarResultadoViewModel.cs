namespace MvcRicardoNogales.Models
{
    public class ActualizarResultadoViewModel
    {
        public List<PartidoDTO> PartidosDisponibles { get; set; }
        public int IdPartidoSeleccionado { get; set; }

        public int IdPartido { get; set; }
        public string NombreEquipoLocal { get; set; }
        public string NombreEquipoVisitante { get; set; }
        public List<JugadorDTO> JugadoresLocal { get; set; } = new();
        public List<JugadorDTO> JugadoresVisitante { get; set; } = new();

        public int GolesLocal { get; set; }
        public int GolesVisitante { get; set; }

        public List<int> IdsGoleadoresLocal { get; set; } = new();
        public List<int> IdsGoleadoresVisitante { get; set; } = new();
        public List<TarjetaDTO> Tarjetas { get; set; } = new();
    }
}
