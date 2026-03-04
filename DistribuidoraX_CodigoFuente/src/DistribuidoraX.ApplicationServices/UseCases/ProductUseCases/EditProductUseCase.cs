using DistribuidoraX.Domain.Abstractions.Repositories.ProductRepositories;
using DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases;
using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.ApplicationServices.UseCases.ProductUseCases
{
    internal class EditProductUseCase : IEditProductUseCase
    {
        private readonly IProductRepository_StoredProceduresSqlServer _productRepositoryStoredProceduresSqlServer;
        private readonly ICalculateProductPriceUseCase _calculateProductPriceUseCase;
        public EditProductUseCase(IProductRepository_StoredProceduresSqlServer productRepositoryStoredProceduresSqlServer, ICalculateProductPriceUseCase calculateProductPriceUseCase)
        {
            _productRepositoryStoredProceduresSqlServer = productRepositoryStoredProceduresSqlServer;
            _calculateProductPriceUseCase = calculateProductPriceUseCase;
        }
        public async Task<int> UpdateItem_StoredProceduresSqlServerAsync(Producto product)
        {
            return await _productRepositoryStoredProceduresSqlServer.UpdateItemAsync(product);
        }

        public async Task<int> UpdatePrice_StoredProceduresSqlServerAsync(int productId)
        {
            var updateProduct = await _productRepositoryStoredProceduresSqlServer.GetByIdAsync(productId);
            var updatePrice = await _calculateProductPriceUseCase.SimpleCalculation_ProductPriceAsync(updateProduct.ProductoId);
            updateProduct.Precio = updatePrice;

            return await _productRepositoryStoredProceduresSqlServer.UpdatePriceAsync(updateProduct);
        }
    }
}
