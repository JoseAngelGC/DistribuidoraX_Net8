using System.ComponentModel.DataAnnotations;

namespace DistribuidoraX.SitioWeb.Models.ProductModels
{
    public class ProductModel
    {
        public int ProductItem { get; set; }
        [Display(Name = "Clave")]
        public string? ProductCode { get; set; } = string.Empty;
        [Display(Name = "Nombre")]
        public string? ProductName { get; set; } = string.Empty;
        [Display(Name = "Precio")]
        public decimal ProductPrice { get; set; } = decimal.Zero;
        [Display(Name = "Es Activo")]
        public bool ProductActive { get; set; } = true;

    }
}
