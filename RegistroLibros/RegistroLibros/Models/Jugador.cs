using System.ComponentModel.DataAnnotations;

namespace RegistroLibros.Models;

public class Jugador
{
    [Key]
    public int JugadorId { get; set; }

    [Required]
    public string Nombre { get; set; } = "";

    public ICollection<Partida> Partidas { get; set; } = new List<Partida>();
}
