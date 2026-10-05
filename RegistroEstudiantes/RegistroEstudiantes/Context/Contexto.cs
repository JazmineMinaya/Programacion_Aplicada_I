using RegistroEstudiantes.Models;
using Microsoft.EntityFrameworkCore;

namespace RegistroEstudiantes.Context;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options) { }

    public virtual DbSet<Libro> Libros { get; set; }
    public virtual DbSet<Estudiantes> Estudiantes { get; set; }
    public virtual DbSet<Prestamos> Prestamos { get; set; }
}
