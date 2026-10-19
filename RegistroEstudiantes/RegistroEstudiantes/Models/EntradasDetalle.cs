using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroEstudiantes.Models;

public class EntradasDetalle
{
    [Key]
    public int Id { get; set; }
    public int EntradaId { get; set; }
    public int LibroId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Este campo es requerido")]
    public int Cantidad { get; set; }

    [ForeignKey("EntradaId")]
    public virtual Entradas? Entrada { get; set; }

    [ForeignKey("LibroId")]
    public virtual Libro? Libro { get; set; }
}
