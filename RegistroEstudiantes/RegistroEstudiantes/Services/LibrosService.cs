using RegistroEstudiantes.Context;
using RegistroEstudiantes.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace RegistroEstudiantes.Services;

public class LibrosService(
    IDbContextFactory<Contexto> contextFactory
) : Aplicada1.Core.IService<Libro, int>
{
    private async Task<bool> Existe(int libroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros
            .AnyAsync(l => l.LibroId == libroId);
    }

    private async Task<bool> TituloExiste(string titulo, int libroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros
            .AnyAsync(l => l.Titulo.ToLower() == titulo.ToLower()
                        && l.LibroId != libroId);
    }

    private async Task<bool> Insertar(Libro libro)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Libros.Add(libro);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Libro libro)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(libro);
        return await contexto
            .SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Libro libro)
    {
        if (await TituloExiste(libro.Titulo, libro.LibroId))
        {
            return false;
        }

        if (!await Existe(libro.LibroId))
        {
            return await Insertar(libro);
        }
        else
        {
            return await Modificar(libro);
        }
    }

    public async Task<Libro?> Buscar(int libroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros
            .FirstOrDefaultAsync(l => l.LibroId == libroId);
    }

    public async Task<bool> Eliminar(int libroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros
            .Where(l => l.LibroId == libroId)
            .ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Libro>> GetList(Expression<Func<Libro, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}