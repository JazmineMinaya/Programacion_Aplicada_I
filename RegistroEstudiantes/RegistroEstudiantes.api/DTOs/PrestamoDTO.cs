namespace RegistroEstudiantes.api.DTOs;

public class PrestamoDTO
{
    public int PrestamoId { get; set; }
    public int EstudianteId { get; set; }
    public int LibroId { get; set; }
    public DateTime FechaPrestamo { get; set; }
    public DateTime? FechaDevolucion { get; set; }
}
