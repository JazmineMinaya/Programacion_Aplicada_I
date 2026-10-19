using System.ComponentModel.DataAnnotations;

namespace RegistroEstudiantes.Models;

public class Entradas
{
    [Key]
    public int EntradaId { get; set; }

    [Required(ErrorMessage = "Este campo es requerido")]
    public DateTime Fecha { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Este campo es requerido")]
    public string Concepto { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Este campo es requerido")]
    public int Total { get; set; }

    public virtual ICollection<EntradasDetalle> EntradasDetalles { get; set; } = new List<EntradasDetalle>();
}
