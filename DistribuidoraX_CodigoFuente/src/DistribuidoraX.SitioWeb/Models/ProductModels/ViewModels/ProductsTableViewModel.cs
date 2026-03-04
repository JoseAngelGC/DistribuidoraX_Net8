namespace DistribuidoraX.SitioWeb.Models.ProductModels.ViewModels
{
    public class ProductsTableViewModel
    {
        public string ProductListErrorMessage { get; set; } = string.Empty;
        public List<ProductModel>? ProductList { get; set; }
    }
}
