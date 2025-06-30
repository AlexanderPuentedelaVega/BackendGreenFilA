using GreenFil.Application.DTOs;
using GreenFil.Application.Interfaces;
using GreenFil.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GreenFil.Infrastructure.Services;

public class Modelo3DService : IModelo3DService
{
    private readonly GreenFilContext.GreenfilContext _context;

    public Modelo3DService(GreenFilContext.GreenfilContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Modelo3DResponseDTO>> ObtenerTodosAsync()
    {
        return await _context.Modelo3ds
            .Include(m => m.Usuario)
            .Select(m => new Modelo3DResponseDTO
            {
                Id = m.Id,
                NombreModelo = m.Nombremodelo,
                FechaCreacion = m.Fechacreacion,
                RutaArchivo = m.Rutaarchivo,
                ImagenPreview = m.Imagenpreview,
                NombreUsuario = m.Usuario.Nombreusuario
            }).ToListAsync();
    }

    public async Task<Modelo3DResponseDTO?> ObtenerPorIdAsync(int id)
    {
        var modelo = await _context.Modelo3ds
            .Include(m => m.Usuario)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (modelo == null) return null;

        return new Modelo3DResponseDTO
        {
            Id = modelo.Id,
            NombreModelo = modelo.Nombremodelo,
            FechaCreacion = modelo.Fechacreacion,
            RutaArchivo = modelo.Rutaarchivo,
            ImagenPreview = modelo.Imagenpreview,
            NombreUsuario = modelo.Usuario.Nombreusuario
        };
    }

    public async Task<int> CrearAsync(Modelo3DCreateDTO dto)
    {
        var modelo = new Modelo3d
        {
            Usuarioid = dto.UsuarioId,
            Nombremodelo = dto.NombreModelo,
            Rutaarchivo = dto.RutaArchivo,
            Imagenpreview = dto.ImagenPreview,
            Fechacreacion = DateTime.UtcNow
        };

        _context.Modelo3ds.Add(modelo);
        await _context.SaveChangesAsync();
        return modelo.Id;
    }

    public async Task<bool> ActualizarAsync(int id, Modelo3DCreateDTO dto)
    {
        var modelo = await _context.Modelo3ds.FindAsync(id);
        if (modelo == null) return false;

        modelo.Nombremodelo = dto.NombreModelo;
        modelo.Rutaarchivo = dto.RutaArchivo;
        modelo.Imagenpreview = dto.ImagenPreview;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var modelo = await _context.Modelo3ds.FindAsync(id);
        if (modelo == null) return false;

        _context.Modelo3ds.Remove(modelo);
        await _context.SaveChangesAsync();
        return true;
    }
}