using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualBasic;
using RegistroEstudiantes.Context;
using RegistroEstudiantes.Models;
using System.Linq.Expressions;

namespace RegistroEstudiantes.Services;

public class PrestamosService(
    IDbContextFactory<Contexto> contextFactory
) : Aplicada1.Core.IService<Prestamos,int>
{
    private async Task<bool> Existe(int prestamoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos
            .AnyAsync(p => p.PrestamoId == prestamoId);
    }

    private async Task<bool> Insertar(Prestamos prestamo)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var libro = await contexto.Libros
            .FirstOrDefaultAsync(l => l.LibroId == prestamo.LibroId);

        if (libro == null || !libro.Disponible)
        {
            return false;
        }

        libro.Disponible = false;
        contexto.Prestamos.Add(prestamo);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Prestamos prestamo)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(prestamo);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Prestamos prestamo)
    {
        if (!await Existe(prestamo.PrestamoId))
        {
            return await Insertar(prestamo);
        }
        else
        {
            return await Modificar(prestamo);
        }
    }

    public async Task<Prestamos?> Buscar(int prestamoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Prestamos
            .Include(prestamoId => prestamoId.Estudiante)
            .Include(prestamoId => prestamoId.Libro)
            .FirstOrDefaultAsync(p=> p.PrestamoId == prestamoId);
    }

    public async Task<bool> Eliminar(int prestamoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var prestamo = await contexto.Prestamos
            .FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);

        if (prestamo == null)
        {
            return false;
        }

        var libro = await contexto.Libros
            .FirstOrDefaultAsync(l => l.LibroId == prestamo.LibroId);

        if (libro != null)
        {
            libro.Disponible = true;
        }

        contexto.Prestamos.Remove(prestamo);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<Prestamos>> GetList(
        Expression<Func<Prestamos, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Prestamos
            .Include(p => p.Estudiante)
            .Include(p => p.Libro)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}
