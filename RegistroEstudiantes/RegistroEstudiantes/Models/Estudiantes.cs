using System.ComponentModel.DataAnnotations;

namespace RegistroEstudiantes.Models:

public partial class Estudiantes
{
    [Key]
    public int EstudianteId { get; set; }

    [Required(ErrorMessage = "Este campo es requerido")]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "Este campo es requerido")]
    public string Direccion { get; set; } = string.Empty;

    [Required(ErrorMessage = "Este campo es requerido")]
    [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Este campo es requerido")]
    public DateTime FechaNacimiento { get; set; }
}
