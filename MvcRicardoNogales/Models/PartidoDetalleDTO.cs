namespace MvcRicardoNogales.Models
{
    public class PartidoDetalleDTO
    {
        public int IdPartido { get; set; }
        public string NombreEquipoLocal { get; set; }
        public string NombreEquipoVisitante { get; set; }
        public int? GolesLocal { get; set; }
        public int? GolesVisitante { get; set; }
        public DateTime FechaHora { get; set; }
        public List<GolDTO> Goleadores { get; set; }
        public List<TarjetaDTO> Tarjetas { get; set; }
    }

    public class GolDTO
    {
        public string NombreJugador { get; set; }
        public int Minuto { get; set; }
    }

    public class TarjetaDTO
    {
        public string NombreJugador { get; set; }
        public string TipoTarjeta { get; set; }
    }
}
