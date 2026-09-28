using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroEstudiantes.Models
{
    public class Prestamos
    {
        [Key]
        public int PrestamoId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Este campo es requerido")]
        public int EstudianteId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Este campo es requerido")]
        public int LibroId { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public DateTime FechaPrestamo { get; set; }

        [ForeignKey("EstudianteId")]
        public Estudiantes? Estudiante { get; set; }

        [ForeignKey("LibroId")]
        public Libro? Libro { get; set; }
    }
}
