using Microsoft.AspNetCore.Http;

namespace GreenFil.Application.Interfaces;

public interface IModeloStlService
{
    Task<(string glbUrl, string stlFileName)> GenerarDesdeImagenAsync(int usuarioId, IFormFile imagen, string nombreModelo);
    Task<(string glbUrl, string stlFileName)> GenerarDesdeTextoAsync(int usuarioId, string prompt, string nombreModelo);
}