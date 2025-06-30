using GreenFil.Application.Interfaces;
using GreenFil.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace GreenFil.Infrastructure.Services;

public class ModeloStlService : IModeloStlService
{
    private readonly GreenFilContext.GreenfilContext _context;
    private readonly IFileStorageService _fileStorageService;
    private readonly IPythonService _pythonService;
    
    private const int PuntosPorGeneracionSTL = 50;

    public ModeloStlService(
        GreenFilContext.GreenfilContext context,
        IFileStorageService fileStorageService,
        IPythonService pythonService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
        _pythonService = pythonService;
    }

    public async Task<(string glbUrl, string stlFileName)> GenerarDesdeImagenAsync(int usuarioId, IFormFile imagen, string nombreModelo)
    {
        var usuario = await _context.Usuarios.FindAsync(usuarioId)
                     ?? throw new Exception("Usuario no encontrado.");

        if ((usuario.Puntos ?? 0) < PuntosPorGeneracionSTL)
            throw new Exception("Puntos insuficientes.");

        var extension = Path.GetExtension(imagen.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";
        var tempPath = Path.Combine(Path.GetTempPath(), fileName);

        await using (var stream = File.Create(tempPath))
        {
            await imagen.CopyToAsync(stream);
        }

        string glbPath = "", s3Url = "";
        try
        {
            glbPath = await _pythonService.GenerateStlFromImage(tempPath, usuarioId, nombreModelo);

            var fileStream = File.OpenRead(glbPath);
            var formFile = new FormFile(fileStream, 0, fileStream.Length, null, Path.GetFileName(glbPath));

            s3Url = await _fileStorageService.GuardarImagenAsync(formFile, "modelos_3d");

            var modelo = new Modelo3d
            {
                Usuarioid = usuarioId,
                Nombremodelo = nombreModelo,
                Fechacreacion = DateTime.UtcNow,
                Rutaarchivo = s3Url
            };

            _context.Modelo3ds.Add(modelo);
            usuario.Puntos -= PuntosPorGeneracionSTL;

            await _context.SaveChangesAsync();

            return (s3Url, Path.GetFileName(glbPath));
        }
        finally
        {
            if (File.Exists(tempPath)) File.Delete(tempPath);
            if (File.Exists(glbPath)) File.Delete(glbPath);
        }
    }

    public async Task<(string glbUrl, string stlFileName)> GenerarDesdeTextoAsync(int usuarioId, string prompt, string nombreModelo)
    {
        var usuario = await _context.Usuarios.FindAsync(usuarioId)
                     ?? throw new Exception("Usuario no encontrado.");

        if ((usuario.Puntos ?? 0) < PuntosPorGeneracionSTL)
            throw new Exception("Puntos insuficientes.");

        var (glbPath, stlPath) = await _pythonService.GenerateStlFromText(prompt, $"{usuarioId}_{nombreModelo}");

        var fileStream = File.OpenRead(glbPath);
        var formFile = new FormFile(fileStream, 0, fileStream.Length, null, Path.GetFileName(glbPath));

        var s3Url = await _fileStorageService.GuardarImagenAsync(formFile, "modelos_3d");

        var modelo = new Modelo3d
        {
            Usuarioid = usuarioId,
            Nombremodelo = nombreModelo,
            Fechacreacion = DateTime.UtcNow,
            Rutaarchivo = s3Url
        };

        _context.Modelo3ds.Add(modelo);
        usuario.Puntos -= PuntosPorGeneracionSTL;

        await _context.SaveChangesAsync();

        return (s3Url, Path.GetFileName(stlPath));
    }
}