using GreenFil.Application.DTOs;
using GreenFil.Application.Interfaces;
using GreenFil.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;

namespace GreenFil.Infrastructure.Services;

public class ModeloPremiumService : IModeloPremiumService
{
    private readonly GreenFilContext.GreenfilContext _context;
    private readonly IFileStorageService _fileStorage;

    public ModeloPremiumService(GreenFilContext.GreenfilContext context, IFileStorageService fileStorage)
    {
        _context = context;
        _fileStorage = fileStorage;
    }

    public async Task<int> SubirYRegistrarModeloAsync(ModelosPremiumUploadRequest request)
    {
        var nombreBase = Path.GetFileNameWithoutExtension(request.NombreModelo).Replace(" ", "_").ToLower();

        string nombreGLB = $"{nombreBase}.glb";
        string nombreSTL = $"{nombreBase}.stl";
        string nombrePreview = $"{nombreBase}.jpg";

        var rutaGLB = await SubirArchivoAS3(request.ArchivoGLB, "glb", nombreGLB);
        var rutaSTL = await SubirArchivoAS3(request.ArchivoSTL, "stl", nombreSTL);
        var rutaPreview = await SubirArchivoAS3(request.ImagenPreview, "previews", nombrePreview);

        var modelo = new Modelospremium
        {
            NombreModelo = request.NombreModelo,
            Descripcion = request.Descripcion,
            PrecioSoles = request.PrecioSoles,
            PrecioPuntos = request.PrecioPuntos,
            RutaGLB = rutaGLB,
            RutaSTL = rutaSTL,
            RutaPreview = rutaPreview,
            Estado = request.Estado,
            FechaRegistro = DateTime.UtcNow
        };

        _context.Modelospremiums.Add(modelo);
        await _context.SaveChangesAsync();

        return modelo.Id;
    }

    public async Task<IEnumerable<object>> ObtenerCatalogoAsync()
    {
        return await _context.Modelospremiums
            .Select(m => new
            {
                m.Id,
                m.NombreModelo,
                m.Descripcion,
                m.RutaPreview,
                m.PrecioSoles,
                m.PrecioPuntos,
                m.Estado
            }).ToListAsync();
    }

    public async Task<object?> ObtenerPorIdAsync(int id)
    {
        var modelo = await _context.Modelospremiums.FindAsync(id);
        if (modelo == null) return null;

        return new
        {
            modelo.Id,
            modelo.NombreModelo,
            modelo.Descripcion,
            modelo.RutaGLB,
            modelo.RutaSTL,
            modelo.RutaPreview,
            modelo.PrecioSoles,
            modelo.PrecioPuntos,
            modelo.Estado,
            modelo.FechaRegistro
        };
    }

    public async Task<bool> ActualizarAsync(int id, Modelospremium actualizado)
    {
        var modelo = await _context.Modelospremiums.FindAsync(id);
        if (modelo == null) return false;

        modelo.NombreModelo = actualizado.NombreModelo;
        modelo.Descripcion = actualizado.Descripcion;
        modelo.PrecioSoles = actualizado.PrecioSoles;
        modelo.PrecioPuntos = actualizado.PrecioPuntos;
        modelo.RutaGLB = actualizado.RutaGLB;
        modelo.RutaSTL = actualizado.RutaSTL;
        modelo.RutaPreview = actualizado.RutaPreview;
        modelo.Estado = actualizado.Estado;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var modelo = await _context.Modelospremiums.FindAsync(id);
        if (modelo == null) return false;

        _context.Modelospremiums.Remove(modelo);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<string> SubirArchivoAS3(IFormFile archivo, string subcarpeta, string nombreArchivo)
    {
        // Crear un archivo temporal
        var extension = Path.GetExtension(nombreArchivo);
        var tempPath = Path.GetTempFileName();

        await using (var stream = File.Create(tempPath))
        {
            await archivo.CopyToAsync(stream);
        }

        var transferFile = new FormFile(File.OpenRead(tempPath), 0, archivo.Length, null!, nombreArchivo);

        var result = await _fileStorage.GuardarImagenAsync(transferFile, subcarpeta);

        File.Delete(tempPath);
        return result;
    }
}