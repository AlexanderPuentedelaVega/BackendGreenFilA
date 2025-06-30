using GreenFil.Application.Interfaces;
using GreenFil.Domain.Entities;
using GreenFil.Infrastructure.GreenFilContext;

namespace GreenFil.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly GreenFilContext.GreenfilContext _context;
    private IRepository<Producto>? _productoRepository;
    
    public IRepository<Producto> ProductoRepository => _productoRepository ??= new Repository<Producto>(_context);
    public IRepository<Detallepedido> DetallePedidoRepository { get; private set; }
    public IRepository<Pedido> PedidoRepository { get; private set; }
    public IRepository<Usuario> Usuarios { get; }
    public IRepository<Modelo3d> Modelos { get; }

    public UnitOfWork(GreenFilContext.GreenfilContext context)
    { 
        _context = context;
        Usuarios = new Repository<Usuario>(context);
        Modelos = new Repository<Modelo3d>(context);
        DetallePedidoRepository = new Repository<Detallepedido>(context);
        PedidoRepository = new Repository<Pedido>(context);
    }
    

    public Task CommitAsync() => _context.SaveChangesAsync();
}