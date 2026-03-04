
namespace DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases
{
    public interface ICalculateProductPriceUseCase
    {
        Task<decimal> SimpleCalculation_ProductPriceAsync(int productId);
    }
}
