using System.ComponentModel.DataAnnotations;

namespace DistribuidoraX.SitioWeb.Models.ProductModels.ViewModels
{
    public class DeleteProductViewModel
    {
        public int ItemProduct { get; set; }
        [Display(Name = "Clave")]
        public string? CodeProduct { get; set; }
        [Display(Name = "Nombre")]
        public string? NameProduct { get; set; }
        [Display(Name = "Precio")]
        public decimal PriceProduct { get; set; }
    }
}
