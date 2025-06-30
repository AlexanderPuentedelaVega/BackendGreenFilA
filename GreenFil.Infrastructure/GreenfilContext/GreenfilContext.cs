using GreenFil.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GreenFil.Infrastructure.GreenFilContext;

public partial class GreenfilContext : DbContext
{
    public GreenfilContext()
    {
    }

    public GreenfilContext(DbContextOptions<GreenfilContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Detallepedido> Detallepedidos { get; set; }

    public virtual DbSet<Modelo3d> Modelo3ds { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }
    
    public virtual DbSet<Modelospremium> Modelospremiums { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Detallepedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("detallepedido_pkey");

            entity.ToTable("detallepedido");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Pedidoid).HasColumnName("pedidoid");
            entity.Property(e => e.Productoid).HasColumnName("productoid");
            entity.Property(e => e.Subtotalpuntos)
                .HasDefaultValue(0)
                .HasColumnName("subtotalpuntos");
            entity.Property(e => e.Subtotalsoles)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("subtotalsoles");

            entity.HasOne(d => d.Pedido).WithMany(p => p.Detallepedidos)
                .HasForeignKey(d => d.Pedidoid)
                .HasConstraintName("detallepedido_pedidoid_fkey");

            entity.HasOne(d => d.Producto).WithMany(p => p.Detallepedidos)
                .HasForeignKey(d => d.Productoid)
                .HasConstraintName("detallepedido_productoid_fkey");
        });

        modelBuilder.Entity<Modelo3d>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("modelo3d_pkey");

            entity.ToTable("modelo3d");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fechacreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fechacreacion");
            entity.Property(e => e.Imagenpreview)
                .HasMaxLength(255)
                .HasColumnName("imagenpreview");
            entity.Property(e => e.Nombremodelo)
                .HasMaxLength(100)
                .HasColumnName("nombremodelo");
            entity.Property(e => e.Rutaarchivo)
                .HasMaxLength(255)
                .HasColumnName("rutaarchivo");
            entity.Property(e => e.Usuarioid).HasColumnName("usuarioid");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Modelo3ds)
                .HasForeignKey(d => d.Usuarioid)
                .HasConstraintName("modelo3d_usuarioid_fkey");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pedido_pkey");

            entity.ToTable("pedido");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValueSql("'pendiente'::character varying")
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha");
            entity.Property(e => e.Totalpuntos)
                .HasDefaultValue(0)
                .HasColumnName("totalpuntos");
            entity.Property(e => e.Totalsoles)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("totalsoles");
            entity.Property(e => e.Usuarioid).HasColumnName("usuarioid");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.Usuarioid)
                .HasConstraintName("pedido_usuarioid_fkey");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("productos_pkey");

            entity.ToTable("productos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Imagen)
                .HasMaxLength(255)
                .HasColumnName("imagen");
            entity.Property(e => e.Nombreproducto)
                .HasMaxLength(100)
                .HasColumnName("nombreproducto");
            entity.Property(e => e.Preciopuntos)
                .HasDefaultValue(0)
                .HasColumnName("preciopuntos");
            entity.Property(e => e.Preciosoles)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("preciosoles");
            entity.Property(e => e.Stock)
                .HasDefaultValue(0)
                .HasColumnName("stock");
            entity.Property(e => e.Tipo)
                .HasMaxLength(20)
                .HasColumnName("tipo");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Email, "usuarios_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Fecharegistro)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecharegistro");
            entity.Property(e => e.Nombreusuario)
                .HasMaxLength(50)
                .HasColumnName("nombreusuario");
            entity.Property(e => e.Passwordhash)
                .HasMaxLength(255)
                .HasColumnName("passwordhash");
            entity.Property(e => e.Puntos)
                .HasDefaultValue(0)
                .HasColumnName("puntos");
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .HasColumnName("rol");
        });
        
        modelBuilder.Entity<Modelospremium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("modelospremium_pkey");

            entity.ToTable("modelospremium");

            entity.Property(e => e.Id).HasColumnName("Id");

            entity.Property(e => e.NombreModelo)
                .HasMaxLength(100)
                .HasColumnName("NombreModelo");

            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("Descripcion");

            entity.Property(e => e.PrecioSoles)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("PrecioSoles");

            entity.Property(e => e.PrecioPuntos)
                .HasDefaultValue(0)
                .HasColumnName("PrecioPuntos");

            entity.Property(e => e.RutaGLB)
                .HasMaxLength(255)
                .HasColumnName("RutaGLB");

            entity.Property(e => e.RutaSTL)
                .HasMaxLength(255)
                .HasColumnName("RutaSTL");

            entity.Property(e => e.RutaPreview)
                .HasMaxLength(255)
                .HasColumnName("RutaPreview");

            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValueSql("'pendiente'::character varying")
                .HasColumnName("Estado");

            entity.Property(e => e.FechaRegistro)
                .HasColumnType("timestamp without time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("FechaRegistro");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
