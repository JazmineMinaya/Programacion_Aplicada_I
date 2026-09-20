using Microsoft.EntityFrameworkCore;
using RegistroLibros.Context;
using RegistroLibros.Models;
using System.Linq.Expressions;
using Aplicada1.Core;

namespace RegistroLibros.Services
{
    public class LibrosService(IDbContextFactory<Contexto> DbFactory) : IService<Libro, int>
    {
        public async Task<bool> Guardar(Libro libro)
        {
            if (!await Existe(libro.LibroId))
            {
                return await Insertar(libro);
            }
            else
            {
                return await Modificar(libro);
            }
        }

        private async Task<bool> Existe(int libroId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Libros
                .AnyAsync(libro => libro.LibroId == libroId);
        }

        public async Task<bool> TituloExiste(string titulo, int libroId = 0)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Libros
                .AnyAsync(libro => libro.Titulo == titulo && libro.LibroId != libroId);
        }

        private async Task<bool> Insertar(Libro libro)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Libros.Add(libro);
            return await contexto.SaveChangesAsync() > 0;
        }

        private async Task <bool> Modificar(Libro libro)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Update(libro);
            return await contexto
                .SaveChangesAsync() > 0;
        }

        public async Task<Libro?> Buscar(int libroId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Libros
                .AsNoTracking()
                .FirstOrDefaultAsync(libro => libro.LibroId == libroId);
        }

        public async Task<bool> Eliminar(int libroId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Libros
                .AsNoTracking()
                .Where(libro => libro.LibroId == libroId)
                .ExecuteDeleteAsync() > 0;
        }

        public async Task<List<Libro>> GetList(Expression<Func<Libro, bool>> criterio)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Libros
                .Where(criterio)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
