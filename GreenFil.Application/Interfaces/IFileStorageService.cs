using Microsoft.AspNetCore.Http;

namespace GreenFil.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> GuardarImagenAsync(IFormFile archivo, string subcarpeta);
}