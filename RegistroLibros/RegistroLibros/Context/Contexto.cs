using RegistroLibros.Models;
using Microsoft.EntityFrameworkCore;

namespace RegistroLibros.Context
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) { }

        public DbSet<Libro> Libros { get; set; }
    }
}
