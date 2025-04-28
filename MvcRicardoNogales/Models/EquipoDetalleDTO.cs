namespace MvcRicardoNogales.Models
{
    public class EquipoDetalleDTO
    {
        public int IdEquipo { get; set; }
        public string Nombre { get; set; }
        public string Escudo { get; set; }
        public List<JugadorDTO> Jugadores { get; set; }
    }

    public class JugadorDTO
    {
        public int IdJugador { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int Dorsal { get; set; }
        public int Goles { get; set; }
        public int IdEquipo { get; set; }
    }
}
