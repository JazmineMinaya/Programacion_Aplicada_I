using System.ComponentModel.DataAnnotations;

namespace Models;

public class Prioridades() 
{
    [Key]
    public int PrioridadId { get; set; }
    public string? Description { get; set; }
}
