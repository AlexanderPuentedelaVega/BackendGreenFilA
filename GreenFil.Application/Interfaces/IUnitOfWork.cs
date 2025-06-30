using GreenFil.Domain.Entities;

namespace GreenFil.Application.Interfaces;

public interface IUnitOfWork
{
    IRepository<Usuario> Usuarios { get; }
    IRepository<Modelo3d> Modelos { get; }
    IRepository<Producto> ProductoRepository { get; }
    IRepository<Pedido> PedidoRepository { get; }
    IRepository<Detallepedido> DetallePedidoRepository { get; }
    
    Task CommitAsync();
    

}