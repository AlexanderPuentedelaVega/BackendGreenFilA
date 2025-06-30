using GreenFil.Application.DTOs;
using GreenFil.Domain.Entities;

namespace GreenFil.Application.Interfaces;

public interface IModeloPremiumService
{
    Task<int> SubirYRegistrarModeloAsync(ModelosPremiumUploadRequest request);
    Task<IEnumerable<object>> ObtenerCatalogoAsync();
    Task<object?> ObtenerPorIdAsync(int id);
    Task<bool> ActualizarAsync(int id, Modelospremium actualizado);
    Task<bool> EliminarAsync(int id);
}