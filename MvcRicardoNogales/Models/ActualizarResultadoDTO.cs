using MvcRicardoNogales.Models;

public class ActualizarResultadoDTO
{
    public int IdPartido { get; set; }
    public int GolesLocal { get; set; }
    public int GolesVisitante { get; set; }
    public List<int> IdsGoleadoresLocal { get; set; } = new();
    public List<int> IdsGoleadoresVisitante { get; set; } = new();
    public List<TarjetaDTO> Tarjetas { get; set; } = new();
}
