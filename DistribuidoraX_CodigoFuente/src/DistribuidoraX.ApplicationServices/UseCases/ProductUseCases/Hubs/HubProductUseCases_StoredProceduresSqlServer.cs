using DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases;
using DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases.Hubs;
using DistribuidoraX.Domain.Entities;
using DistribuidoraX.Domain.Objects.GenericObjects;

namespace DistribuidoraX.ApplicationServices.UseCases.ProductUseCases.Hubs
{
    internal class HubProductUseCases_StoredProceduresSqlServer : IHubProductUseCases_StoredProceduresSqlServer
    {
        private readonly IGetProductUseCase _getProductUseCase;
        private readonly INewProductUseCase _newProductUseCase;
        private readonly IEditProductUseCase _editProductUseCase;
        private readonly IDeleteProductUseCase _deleteProductUseCase;
        private readonly IProductValidationsUseCase _productValidationsUseCase;
        public HubProductUseCases_StoredProceduresSqlServer(
            IGetProductUseCase getProductUseCase,
            INewProductUseCase newProductUseCase,
            IEditProductUseCase editProductUseCase,
            IDeleteProductUseCase deleteProductUseCase,
            IProductValidationsUseCase productValidationsUseCase)
        {
            _getProductUseCase = getProductUseCase;
            _newProductUseCase = newProductUseCase;
            _editProductUseCase = editProductUseCase;
            _deleteProductUseCase = deleteProductUseCase;
            _productValidationsUseCase = productValidationsUseCase;
        }

        public async Task<Producto> GetProductUseCase_GetItemByIdAsync(int productId)
        {
            return await _getProductUseCase.GetItemById_StoredProceduresSqlServerAsyncAsync(productId);
        }

        public async Task<bool> ProductValidationsUseCase_ExistCodeAsync(ExistCodeParameters productCodeParameters)
        {
            return await _productValidationsUseCase.ExistProductCode_StoredProceduresSqlServerAsync(productCodeParameters);
        }

        public async Task<int> NewProductUseCase_SaveItemAsync(Producto product)
        {
            return await _newProductUseCase.SaveItem_StoredProceduresSqlServerAsync(product);
        }

        public async Task<int> EditProductUseCase_UpdateItemAsync(Producto product)
        {
            return await _editProductUseCase.UpdateItem_StoredProceduresSqlServerAsync(product);
        }

        public async Task<int> EditProductSupplierUseCase_UpdatePriceAsync(int productId)
        {
            return await _editProductUseCase.UpdatePrice_StoredProceduresSqlServerAsync(productId);
        }

        public async Task<int> DeleteProductUseCase_DeleteByParamsAsync(Producto product)
        {
            return await _deleteProductUseCase.DeleteByParams_StoredProceduresSqlServerAsync(product);
        }
        
    }
}
