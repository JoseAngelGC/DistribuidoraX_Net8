using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.Domain.Abstractions.UseCases.ProductSupplierUseCases
{
    public interface IGetProductSuppliersUseCase
    {
        Task<List<ProductoProveedor>> GetListByProductIdAsync(int productId);
    }
}
