using DistribuidoraX.SitioWeb.Models.ProductModels;

namespace DistribuidoraX.SitioWeb.Models.ProductSuplierModels
{
    public class ProductSupplierFullDataModel
    {
        public ProductFullDataModel? ProductData { get; set; }
        public List<ProductSupplierModel>? ProductSupplierList { get; set; }
    }
}
