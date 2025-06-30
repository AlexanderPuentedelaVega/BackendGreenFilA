namespace GreenFil.Application.DTOs;

public class DetallePedidoDTO
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public int ProductoId { get; set; }
    public int? Cantidad { get; set; }
    public decimal? SubtotalSoles { get; set; }
    public int? SubtotalPuntos { get; set; }
    public PedidoInfoDTO Pedido { get; set; }
    public ProductoInfoDTO Producto { get; set; }
}

public class PedidoInfoDTO
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    //public DateTime Fecha { get; set; }
    public decimal? TotalSoles { get; set; }
    public int? TotalPuntos { get; set; }
    public string Estado { get; set; }
}

public class ProductoInfoDTO
{
    public int Id { get; set; }
    public string NombreProducto { get; set; }
    public string Tipo { get; set; }
    public string? Descripcion { get; set; }
    public decimal? PrecioSoles { get; set; }
    public int? PrecioPuntos { get; set; }
    public int? Stock { get; set; }
    public string? Imagen { get; set; }
}