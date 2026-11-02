namespace RegistroEstudiantes.api.DTOs;

public class EstudianteDTO
{
    public int EstudianteId { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
}
