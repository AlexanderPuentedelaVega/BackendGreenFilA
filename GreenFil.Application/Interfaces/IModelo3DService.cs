using GreenFil.Application.DTOs;

namespace GreenFil.Application.Interfaces;

public interface IModelo3DService
{
    Task<IEnumerable<Modelo3DResponseDTO>> ObtenerTodosAsync();
    Task<Modelo3DResponseDTO?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Modelo3DCreateDTO dto);
    Task<bool> ActualizarAsync(int id, Modelo3DCreateDTO dto);
    Task<bool> EliminarAsync(int id);
}