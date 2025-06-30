using GreenFil.Application.DTOs;
using GreenFil.Domain.Entities;

namespace GreenFil.Application.Interfaces;

public interface IDetallePedidoService
{
    Task<IEnumerable<DetallePedidoDTO>> ObtenerTodosAsync();
    Task<Detallepedido?> ObtenerPorIdAsync(int id);
    Task<Detallepedido> CrearAsync(Detallepedido detalle);
    Task<bool> ActualizarAsync(int id, Detallepedido detalle);
    Task<bool> EliminarAsync(int id);
}