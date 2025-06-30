using GreenFil.Application.DTOs;

namespace GreenFil.Application.Interfaces;

public interface ICanjeProductoService
{
    Task<string> CanjearAsync(int usuarioId, CanjeProductoDTO dto);
    Task<IEnumerable<object>> ObtenerPedidosPendientesAsync();
}