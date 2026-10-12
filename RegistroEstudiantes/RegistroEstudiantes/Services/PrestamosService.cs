using Microsoft.EntityFrameworkCore;
using RegistroEstudiantes.Context;
using RegistroEstudiantes.Models;
using System.Linq.Expressions;
using Aplicada1.Core;

namespace RegistroEstudiantes.Services;

public class PrestamosService(
    IDbContextFactory<Contexto> contextFactory
) : IServiceResult<Prestamos, int>
{
    public async Task<Result> Guardar(Prestamos prestamo)
    {
        if (!await Existe(prestamo.PrestamoId))
        {
            var inserto = await Insertar(prestamo);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(prestamo);
            return Result.Success(modifico);
        }
    }

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

    public async Task<Result<Prestamos?>> Buscar(int prestamoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Prestamos
            .Include(p  => p.Estudiante)
            .Include(p => p.Libro)
            .FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);
    }

    public async Task<Result> Eliminar(int prestamoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var prestamo = await contexto.Prestamos.FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);

        if (prestamo == null)
        {
            return Result.Success(false);
        }

        var libro = await contexto.Libros.FirstOrDefaultAsync(l => l.LibroId == prestamo.LibroId);

        if (libro != null)
        {
            libro.Disponible = true;
        }

        contexto.Prestamos.Remove(prestamo);

        var eliminado = await contexto.SaveChangesAsync() > 0;
        return Result.Success(eliminado);
    }

    public async Task<bool> Devolver(int prestamoId, DateTime fechaDevolucion)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var prestamo = await contexto.Prestamos.FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);

        if (prestamo == null || prestamo.FechaDevolucion != null)
        {
            return false;
        }

        if (fechaDevolucion.Date < prestamo.FechaPrestamo.Date || fechaDevolucion.Date > DateTime.Today)
        {
            return false;
        }

        var libro = await contexto.Libros.FirstOrDefaultAsync(l => l.LibroId == prestamo.LibroId);

        if (libro == null)
        {
            return false;
        }

        prestamo.FechaDevolucion = fechaDevolucion.Date;
        libro.Disponible = true;

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<List<Prestamos>>> GetList(Expression<Func<Prestamos, bool>> criterio)
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
