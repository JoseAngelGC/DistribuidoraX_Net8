using System.ComponentModel.DataAnnotations;

namespace DistribuidoraX.SitioWeb.Models.SupplierModels
{
    public class SupplierModel
    {
        public int SupplierItem { get; set; } = 0;
        [Display(Name = "Proveedor")]
        public string? SupplierName { get; set; } = string.Empty;
    }
}
