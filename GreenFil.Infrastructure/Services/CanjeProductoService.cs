using GreenFil.Application.DTOs;
using GreenFil.Application.Interfaces;
using GreenFil.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GreenFil.Infrastructure.Services;

public class CanjeProductoService : ICanjeProductoService
{
    private readonly GreenFilContext.GreenfilContext _context;

    public CanjeProductoService(GreenFilContext.GreenfilContext context)
    {
        _context = context;
    }

    public async Task<string> CanjearAsync(int usuarioId, CanjeProductoDTO dto)
    {
        var usuario = await _context.Usuarios.FindAsync(usuarioId);
        if (usuario == null) return "Usuario no encontrado.";

        var producto = await _context.Productos
            .FirstOrDefaultAsync(p => p.Id == dto.ProductoId && p.Tipo == "filamento");
        if (producto == null) return "Producto de tipo 'filamento' no encontrado.";

        if (producto.Stock < dto.Cantidad) return "Stock insuficiente.";

        int puntosRequeridos = (producto.Preciopuntos ?? 0) * dto.Cantidad;
        if (usuario.Puntos < puntosRequeridos) return "Puntos insuficientes.";

        var pedido = new Pedido
        {
            Usuarioid = usuarioId,
            Fecha = DateTime.UtcNow,
            Totalsoles = 0,
            Totalpuntos = puntosRequeridos,
            Estado = "pendiente"
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        var detalle = new Detallepedido
        {
            Pedidoid = pedido.Id,
            Productoid = producto.Id,
            Cantidad = dto.Cantidad,
            Subtotalsoles = 0,
            Subtotalpuntos = puntosRequeridos
        };

        _context.Detallepedidos.Add(detalle);
        producto.Stock -= dto.Cantidad;
        usuario.Puntos -= puntosRequeridos;

        await _context.SaveChangesAsync();
        return "Canje exitoso.";
    }

    public async Task<IEnumerable<object>> ObtenerPedidosPendientesAsync()
    {
        return await _context.Pedidos
            .Include(p => p.Detallepedidos)
            .ThenInclude(dp => dp.Producto)
            .Where(p => p.Estado == "pendiente")
            .Select(p => new
            {
                p.Id,
                p.Fecha,
                p.Totalpuntos,
                p.Estado,
                Usuario = p.Usuarioid,
                Productos = p.Detallepedidos.Select(dp => new
                {
                    dp.Productoid,
                    dp.Producto.Nombreproducto,
                    dp.Cantidad,
                    dp.Subtotalpuntos
                })
            }).ToListAsync();
    }
}