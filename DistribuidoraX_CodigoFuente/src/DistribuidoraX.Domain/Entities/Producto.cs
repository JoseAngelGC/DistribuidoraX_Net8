
namespace DistribuidoraX.Domain.Entities
{
    public class Producto
    {
        public int ProductoId { get; set; }
        public string? ClaveInterna { get; set; }
        public string? Nombre { get; set; }
        public decimal Precio { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaAlta { get; set; }
        public int TipoProductoId { get; set; }
        public List<TipoProducto>? ListaTipoProducto { get; set; }
    }
}
