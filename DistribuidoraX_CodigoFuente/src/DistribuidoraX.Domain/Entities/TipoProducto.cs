
namespace DistribuidoraX.Domain.Entities
{
    public class TipoProducto
    {
        public int TipoProductoId { get; set; }
        public string? Nombre { get; set; }
        public string Descripcion { get; set; } = null!;
        public bool Activo { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
