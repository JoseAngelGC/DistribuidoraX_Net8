using DistribuidoraX.SitioWeb.Models.SupplierModels;
using System.ComponentModel.DataAnnotations;

namespace DistribuidoraX.SitioWeb.Models.ProductSuplierModels
{
    public class ProductSupplierModel : SupplierModel
    {
        public int ProductSupplierItem { get; set; } = 0;
        [Display(Name = "Clave")]
        public string? ProductSupplierCode { get; set; } = string.Empty;
        [Display(Name = "Costo")]
        public decimal ProductSupplierCost { get; set; } = decimal.Zero;
    }
}
