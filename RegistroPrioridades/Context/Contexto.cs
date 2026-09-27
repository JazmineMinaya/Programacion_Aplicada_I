using Microsoft.EntityFrameworkCore;

namespace Models;

public class Contexto : DbContext
{ 
    public DbSet<Prioridades> Prioridades { get; set; }
}