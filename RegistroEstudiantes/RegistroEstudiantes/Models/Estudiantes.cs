using System.ComponentModel.DataAnnotations;

namespace RegistroEstudiantes.Models
{
    public partial class Estudiantes
    {
        [Key]
        public int EstudianteId { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "Este campo es requerido")]
        public string Direccion { get; set; } = null!;

        [Required(ErrorMessage = "Este campo es requerido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Este campo es requerido")]
        public DateTime FechaNacimiento { get; set; } = DateTime.Today;
    }
}
