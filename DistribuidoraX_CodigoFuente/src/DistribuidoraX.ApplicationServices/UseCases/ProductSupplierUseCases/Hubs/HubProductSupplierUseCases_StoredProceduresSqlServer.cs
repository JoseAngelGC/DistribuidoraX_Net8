using DistribuidoraX.Domain.Abstractions.UseCases.ProductSupplierUseCases;
using DistribuidoraX.Domain.Abstractions.UseCases.ProductSupplierUseCases.Hubs;
using DistribuidoraX.Domain.Entities;
using DistribuidoraX.Domain.Objects.GenericObjects;

namespace DistribuidoraX.ApplicationServices.UseCases.ProductSupplierUseCases.Hubs
{
    internal class HubProductSupplierUseCases_StoredProceduresSqlServer : IHubProductSupplierUseCases_StoredProceduresSqlServer
    {
        private readonly IGetProductSuppliersUseCase _getProductSuppliersUseCase;
        private readonly INewProductSupplierUseCase _newSupplierProductSupplierUseCase;
        private readonly IEditProductSupplierUseCase _editProductSupplierUseCase;
        private readonly IDeleteProductSupplierUseCase _deleteProductSupplierUseCase;
        private readonly IProductSupplierValidationsUseCase _productSupplierValidationsUseCase;
        public HubProductSupplierUseCases_StoredProceduresSqlServer(
            IGetProductSuppliersUseCase getProductSuppliersUseCase,
            INewProductSupplierUseCase newSupplierProductSupplierUseCase,
            IEditProductSupplierUseCase editProductSupplierUseCase,
            IDeleteProductSupplierUseCase deleteProductSupplierUseCase,
            IProductSupplierValidationsUseCase productSupplierValidationsUseCase)
        {
            _getProductSuppliersUseCase = getProductSuppliersUseCase;
            _newSupplierProductSupplierUseCase = newSupplierProductSupplierUseCase;
            _editProductSupplierUseCase = editProductSupplierUseCase;
            _deleteProductSupplierUseCase = deleteProductSupplierUseCase;
            _productSupplierValidationsUseCase = productSupplierValidationsUseCase;
        }

        public async Task<List<ProductoProveedor>> GetProductSuppliersUseCase_GetListByProductIdAsync(int productId)
        {
            return await _getProductSuppliersUseCase.GetListByProductIdAsync(productId);
        }

        public async Task<bool> ProductSupplierValidationsUseCase_ExistCodeAsync(ExistCodeParameters productSupplierCodeParameters)
        {
            return await _productSupplierValidationsUseCase.ExistProductSupplierCode_StoredProceduresSqlServerAsync(productSupplierCodeParameters);
        }

        public async Task<int> EditProductSupplierUseCase_UpdateItemAsync(ProductoProveedor productSuplierEntity, bool revertFlag)
        {
            return await _editProductSupplierUseCase.UpdateItem_StoredProceduresSqlServerAsync(productSuplierEntity, revertFlag);
        }

        public async Task<int> NewProductSupplierUseCase_SaveItemAsync(ProductoProveedor productSupplierEntity)
        {
            return await _newSupplierProductSupplierUseCase.SaveItem_StoredProceduresSqlServerAsync(productSupplierEntity);
        }

        public async Task<int> DeleteProductSupplierUseCase_DeleteItemByStateAsync(ProductoProveedor productSupplierEntity)
        {
            return await _deleteProductSupplierUseCase.DeleteItemByDeletedStateUpdating_StoredProceduresSqlServerAsync(productSupplierEntity);
        }
        
    }
}
