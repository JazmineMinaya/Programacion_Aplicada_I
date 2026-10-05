using RegistroEstudiantes.Models;
using Microsoft.EntityFrameworkCore;

namespace RegistroEstudiantes.Context
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) { }

        public DbSet<Libro> Libros { get; set; }
        public DbSet<Estudiantes> Estudiantes { get; set; }
        public DbSet<Prestamos> Prestamos { get; set; }
    }
}
