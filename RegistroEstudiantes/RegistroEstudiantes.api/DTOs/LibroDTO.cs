namespace RegistroEstudiantes.api.DTOs;

public class LibroDTO
{
    public int LibroId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public int AnoPublicacion { get; set; }
    public bool Disponible { get; set; }
    public int Cantidad { get; set; }
}
