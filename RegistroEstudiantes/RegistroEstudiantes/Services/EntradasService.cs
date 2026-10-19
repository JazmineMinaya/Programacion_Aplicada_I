using Aplicada1.Core;
using RegistroEstudiantes.Context;
using Microsoft.EntityFrameworkCore;
using RegistroEstudiantes.Models;
using System.Linq.Expressions;

namespace RegistroEstudiantes.Services;

public class EntradasService(
    IDbContextFactory<Contexto> contextFactory
) : IServiceResult<Entradas, int>
{
    public async Task<Result> Guardar(Entradas entradas)
    {
        if (!await Existe(entradas.EntradaId))
        {
            var inserto = await Insertar(entradas);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(entradas);
            return Result.Success(modifico);
        }
    }

    private async Task<bool> Existe(int entradaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Entradas.AnyAsync(e => e.EntradaId == entradaId);
    }

    private async Task<bool> Insertar(Entradas entrada)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Entradas.Add(entrada);

        await ActualizarCantidadLibros(
            entrada.EntradasDetalles.ToArray(), 
            TipoOperacionEntrada.Suma, 
            contexto);

        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Entradas entradas)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var entradaInicial = await contexto.Entradas
            .Include(e => e.EntradasDetalles)
            .FirstOrDefaultAsync(e => e.EntradaId == entradas.EntradaId);

        if (entradaInicial == null)
        {
            return false;
        }

        await ActualizarCantidadLibros(entradaInicial.EntradasDetalles.ToArray(), 
            TipoOperacionEntrada.Resta, 
            contexto);

        contexto.RemoveRange(entradaInicial.EntradasDetalles);

        await ActualizarCantidadLibros(entradas.EntradasDetalles.ToArray(), 
            TipoOperacionEntrada.Suma, 
            contexto);

        entradaInicial.Fecha = entradas.Fecha;
        entradaInicial.Concepto = entradas.Concepto;
        entradaInicial.Total = entradas.Total;
        entradaInicial.EntradasDetalles = entradas.EntradasDetalles
            .Select(d => new EntradasDetalle
            {
                EntradaId = entradaInicial.EntradaId,
                LibroId = d.LibroId,
                Cantidad = d.Cantidad
            })
            .ToList();

        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task ActualizarCantidadLibros(EntradasDetalle[] detalles, TipoOperacionEntrada tipoOperacion, Contexto contexto)
    {
        foreach (var detalle in detalles)
        {
            var libro = await contexto.Libros.SingleAsync(l => l.LibroId == detalle.LibroId);

            if (tipoOperacion == TipoOperacionEntrada.Suma)
            {
                libro.Cantidad += detalle.Cantidad;
            }
            else
            {
                libro.Cantidad -= detalle.Cantidad;
            }
        }
    }
    public async Task<Result<Entradas?>> Buscar(int entradaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Entradas
            .Include(e => e.EntradasDetalles)
                .ThenInclude(d => d.Libro)
            .FirstOrDefaultAsync(e => e.EntradaId == entradaId);
    }

    public async Task<Result> Eliminar(int entradaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var entrada = await contexto.Entradas
            .Include(e => e.EntradasDetalles)
            .FirstOrDefaultAsync(e => e.EntradaId == entradaId);

        if (entrada == null)
        {
            return Result.Success(false);
        }

        await ActualizarCantidadLibros(
            entrada.EntradasDetalles.ToArray(), 
            TipoOperacionEntrada.Resta, 
            contexto);
        contexto.RemoveRange(entrada.EntradasDetalles);
        contexto.Entradas.Remove(entrada);

        var eliminado = await contexto.SaveChangesAsync() > 0;
        return Result.Success(eliminado);
    }

    public async Task<Result<List<Entradas>>> GetList(Expression<Func<Entradas, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Entradas
            .Include(e => e.EntradasDetalles)
            .ThenInclude(d => d.Libro)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public enum TipoOperacionEntrada
    {
        Suma,
        Resta
    }
}
