using GreenFil.Application.DTOs;
using GreenFil.Application.Interfaces;
using GreenFil.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GreenFil.Infrastructure.Services
{
   public class DetallePedidoService : IDetallePedidoService
    {
        private readonly GreenFilContext.GreenfilContext _context;

        public DetallePedidoService(GreenFilContext.GreenfilContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DetallePedidoDTO>> ObtenerTodosAsync()
        {
            var detalles = await _context.Detallepedidos
                .Include(d => d.Producto)
                .Include(d => d.Pedido)
                .AsNoTracking()
                .ToListAsync();

            return detalles.Select(d => new DetallePedidoDTO
            {
                Id = d.Id,
                PedidoId = d.Pedidoid,
                ProductoId = d.Productoid,
                Cantidad = d.Cantidad,
                SubtotalSoles = d.Subtotalsoles,
                SubtotalPuntos = d.Subtotalpuntos,
                Pedido = new PedidoInfoDTO
                {
                    Id = d.Pedido.Id,
                    UsuarioId = d.Pedido.Usuarioid,
                    TotalSoles = d.Pedido.Totalsoles,
                    TotalPuntos = d.Pedido.Totalpuntos,
                    Estado = d.Pedido.Estado!
                },
                Producto = new ProductoInfoDTO
                {
                    Id = d.Producto.Id,
                    NombreProducto = d.Producto.Nombreproducto,
                    Tipo = d.Producto.Tipo,
                    Descripcion = d.Producto.Descripcion,
                    PrecioSoles = d.Producto.Preciosoles,
                    PrecioPuntos = d.Producto.Preciopuntos,
                    Stock = d.Producto.Stock,
                    Imagen = d.Producto.Imagen
                }
            }).ToList();
        }

        public async Task<Detallepedido?> ObtenerPorIdAsync(int id)
        {
            // si quieres DTO, mapea aquí también
            return await _context.Detallepedidos
                .Include(d => d.Producto)
                .Include(d => d.Pedido)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Detallepedido> CrearAsync(Detallepedido detalle)
        {
            _context.Detallepedidos.Add(detalle);
            await _context.SaveChangesAsync();
            return detalle;
        }

        public async Task<bool> ActualizarAsync(int id, Detallepedido detalle)
        {
            var existente = await _context.Detallepedidos.FindAsync(id);
            if (existente == null) return false;

            existente.Cantidad = detalle.Cantidad;
            existente.Subtotalsoles = detalle.Subtotalsoles;
            existente.Subtotalpuntos = detalle.Subtotalpuntos;
            // no toques FK salvo que quieras reasignar

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var detalle = await _context.Detallepedidos.FindAsync(id);
            if (detalle == null) return false;

            _context.Detallepedidos.Remove(detalle);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
