using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroLibros.Models;

public class Partida
{
    [Key]
    public int PartidaId { get; set; }

    [Required]
    public int IdJugador { get; set; }

    [Required]
    public int Puntuacion { get; set; }

    [Required]
    public DateTime Fecha { get; set; }

    [ForeignKey("IdJugador")]
    public Jugador? Jugador { get; set; }
}
