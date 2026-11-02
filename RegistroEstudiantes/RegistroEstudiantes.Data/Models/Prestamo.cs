using System;
using System.Collections.Generic;

namespace RegistroEstudiantes.Data.Models;

public partial class Prestamo
{
    public int PrestamoId { get; set; }

    public int EstudianteId { get; set; }

    public int LibroId { get; set; }

    public DateTime FechaPrestamo { get; set; }

    public DateTime? FechaDevolucion { get; set; }

    public virtual Estudiante Estudiante { get; set; } = null!;

    public virtual Libro Libro { get; set; } = null!;
}
