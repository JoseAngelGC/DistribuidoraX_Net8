using DistribuidoraX.Domain.Abstractions.Responses;
using DistribuidoraX.Domain.Entities;

namespace DistribuidoraX.Domain.Abstractions.UseCases.TransactionsRevertUseCases
{
    public interface IHubTransactionsRevertUseCase
    {
        Task<IGenericResult<bool>> NewProductTransactionRevertAsync(int productoId, string error);
        Task<IGenericResult<bool>> NewProductAndProductSupplierTransactionRevertAsync(int productoId, List<int> productSupplierIdList, string error);
        Task<IGenericResult<bool>> EditProductAndNewProductSupplierTransactionRevertAsync(Producto? productBackup, List<int> productSupplierIdList, string error);
        Task<IGenericResult<bool>> EditProductAndProductSupplierTransactionRevertAsync(Producto? productBackup, List<ProductoProveedor> productSupplierListBackup, string error);
    }
}
