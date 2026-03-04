using DistribuidoraX.SitioWeb.Models.ProductModels;
using DistribuidoraX.SitioWeb.Models.TypeProductModels;
using System.ComponentModel.DataAnnotations;

namespace DistribuidoraX.SitioWeb.Models.ProductSuplierModels.ViewModels
{
    public class ProductSupplierViewModel : ProductModel
    {
        [Display(Name = "Tipo Producto")]
        public int TypeProductId { get; set; }
        public List<TypeProductModel>? TypeProductList { get; set; }
        public List<ProductSupplierModel>? ProductSupplierList { get; set; }
    }
}
