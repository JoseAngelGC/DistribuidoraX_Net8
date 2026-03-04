
namespace DistribuidoraX.Domain.Entities
{
    public class ProductoProveedor
    {
        public int ProductoProveedorId { get; set; }
        public string? ClaveProductoProveedor { get; set; }
        public decimal Costo { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaActualizado { get; set; }
        public bool EstadoEliminado { get; set; }
        public int ProductoId { get; set; }
        public int ProveedorId { get; set; }
        public List<Producto>? ListaProducto { get; set; }
        public List<Proveedor>? ListaProveedores { get; set; }
    }
}
