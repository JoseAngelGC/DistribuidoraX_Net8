
namespace DistribuidoraX.Domain.Entities
{
    public class Proveedor
    {
        public int ProveedorId { get; set; }
        public string? Nombre { get; set; }
        public string Descripcion { get; set; } = null!;
        public DateTime FechaAlta { get; set; }
    }
}
