using DistribuidoraX.SitioWeb.Models.TypeProductModels;
using System.ComponentModel.DataAnnotations;

namespace DistribuidoraX.SitioWeb.Models.ProductModels.ViewModels
{
    public class ProductsViewModel
    {
        [Display(Name = "Clave")]
        public string? ProductCodeFilter { get; set; } = string.Empty;
        [Display(Name = "Tipo Producto")]
        public int TypeProductItemFilter { get; set; } = 0;
        public string TypeProductMessageError { get; set; } = string.Empty;
        public string ProductListMessageError { get; set; } = string.Empty;

        public List<TypeProductModel>? TypeProductList { get; set; }
        public ProductsTableViewModel? ProductsTableModel { get; set; }
    }
}
