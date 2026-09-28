using System.ComponentModel.DataAnnotations;

namespace RegistroEstudiantes.Models
{
    public class Libro
    {
        [Key]
        public int LibroId { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        public string Titulo { get; set; } = "";

        [Required(ErrorMessage = "El autor es obligatorio")]
        public string Autor { get; set; } = "";

        [Required(ErrorMessage = "El año de publicación es obligatorio")]
        public int AnoPublicacion { get; set; }

        public bool Disponible { get; set; } = true;
    }
}
