using DistribuidoraX.ApplicationServices.Responses;
using DistribuidoraX.Domain.Abstractions.Repositories.ProductRepositories;
using DistribuidoraX.Domain.Abstractions.Repositories.ProductSupplierRepositories;
using DistribuidoraX.Domain.Abstractions.Responses;
using DistribuidoraX.Domain.Abstractions.UseCases.TransactionsRevertUseCases;
using DistribuidoraX.Domain.Entities;
using DistribuidoraX.Shared.Utilities;

namespace DistribuidoraX.ApplicationServices.UseCases.TransactionsRevertUseCases
{
    internal class HubTransactionsRevertUseCase : IHubTransactionsRevertUseCase
    {
        private readonly IProductRepository_StoredProceduresSqlServer _productRepositoryStoredProceduresSqlServer;
        private readonly IProductSupplierRepository_StoredProceduresSqlServer _productSupplierRepositoryStoredProceduresSqlServer;
        private readonly GenericResult<bool> _genericResult;
        public HubTransactionsRevertUseCase(IProductRepository_StoredProceduresSqlServer productRepositoryStoredProceduresSqlServer, IProductSupplierRepository_StoredProceduresSqlServer productSupplierRepositoryStoredProceduresSqlServer)
        {
            _productRepositoryStoredProceduresSqlServer = productRepositoryStoredProceduresSqlServer;
            _productSupplierRepositoryStoredProceduresSqlServer = productSupplierRepositoryStoredProceduresSqlServer;
            _genericResult = new GenericResult<bool>();
        }

        public async Task<IGenericResult<bool>> NewProductTransactionRevertAsync(int productoId, string error)
        {
            var newProductRevert = await _productRepositoryStoredProceduresSqlServer.GetByIdAsync(productoId);
            if (newProductRevert != null)
            {
                var recordsAffected = await _productRepositoryStoredProceduresSqlServer.DeleteByParamsAsync(newProductRevert);
                if (!(recordsAffected > 0))
                {
                    return (IGenericResult<bool>)_genericResult.Failure_InternalServerError(
                                                        ReplyMessages.FAILED_RECORDCREATED_WITH_ERRORS,
                                                        false,
                                                        error
                                                    );
                }
            }

            return (IGenericResult<bool>)_genericResult.Failure_InternalServerError(
                                                        ReplyMessages.FAILED_CREATERECORD,
                                                        false,
                                                        null
                                                    );
        }

        public async Task<IGenericResult<bool>> NewProductAndProductSupplierTransactionRevertAsync(int productoId, List<int> productSupplierIdList, string error)
        {
            try
            {
                //productSupplier revert
                foreach (var productSupplierId in productSupplierIdList)
                {
                    var productSupplierRevertRecordsAffected = await _productSupplierRepositoryStoredProceduresSqlServer.DeleteItemByIdAsync(productSupplierId);
                    if (!(productSupplierRevertRecordsAffected > 0))
                    {
                        return (IGenericResult<bool>)_genericResult.Failure_InternalServerError(
                                                        ReplyMessages.FAILED_RECORDCREATED_WITH_ERRORS,
                                                        false,
                                                        error
                                                    );
                    }
                }

                //product revert
                var newProductRevert = await _productRepositoryStoredProceduresSqlServer.GetByIdAsync(productoId);
                if (newProductRevert != null)
                {
                    var newProductRevertRecordsAffected = await _productRepositoryStoredProceduresSqlServer.DeleteByParamsAsync(newProductRevert);
                    if (!(newProductRevertRecordsAffected > 0))
                    {
                        return (IGenericResult<bool>)_genericResult.Failure_InternalServerError(
                                                            ReplyMessages.FAILED_RECORDCREATED_WITH_ERRORS,
                                                            false,
                                                            error
                                                        );
                    }
                }

                return (IGenericResult<bool>)_genericResult.Failure_InternalServerError(
                                                        ReplyMessages.FAILED_CREATERECORD,
                                                        false,
                                                        error
                                                    );
            }
            catch (Exception)
            {
                return (IGenericResult<bool>)_genericResult.Failure_InternalServerError(
                                                        ReplyMessages.FAILED_RECORDCREATED_WITH_ERRORS,
                                                        false,
                                                        error
                                                    );
            }

            
        }

        public async Task<IGenericResult<bool>> EditProductAndProductSupplierTransactionRevertAsync(Producto? productBackup, List<ProductoProveedor> productSupplierListBackup, string error)
        {
            try
            {
                foreach (var productSupplierBackup in productSupplierListBackup)
                {
                    var productSupplierRevertRecordsAffected = await _productSupplierRepositoryStoredProceduresSqlServer.UpdateItemAsync(productSupplierBackup, true);
                    if (!(productSupplierRevertRecordsAffected > 0))
                    {
                        return (IGenericResult<bool>)_genericResult.Failure_InternalServerError(
                                                        ReplyMessages.FAILED_RECORDEDITED_WITH_ERRORS,
                                                        false,
                                                        error
                                                    );
                    }
                }

                if (productBackup != null)
                {
                    var productRevertRecordsAffected = await _productRepositoryStoredProceduresSqlServer.UpdateItemAsync(productBackup);
                    if (!(productRevertRecordsAffected > 0))
                    {
                        return (IGenericResult<bool>)_genericResult.Failure_InternalServerError(
                                                        ReplyMessages.FAILED_RECORDEDITED_WITH_ERRORS,
                                                        false,
                                                        error
                                                    );
                    }
                }

                return (IGenericResult<bool>)_genericResult.Failure_InternalServerError(
                                                        ReplyMessages.FAILED_EDITRECORD,
                                                        false,
                                                        error
                                                    );
            }
            catch (Exception)
            {
                return (IGenericResult<bool>)_genericResult.Failure_InternalServerError(
                                                        ReplyMessages.FAILED_RECORDEDITED_WITH_ERRORS,
                                                        false,
                                                        error
                                                    );
            }
        }

        public async Task<IGenericResult<bool>> EditProductAndNewProductSupplierTransactionRevertAsync(Producto? productBackup, List<int> productSupplierIdList, string error)
        {
            try
            {
                //productSupplier revert
                foreach (var productSupplierId in productSupplierIdList)
                {
                    var productSupplierRevertRecordsAffected = await _productSupplierRepositoryStoredProceduresSqlServer.DeleteItemByIdAsync(productSupplierId);
                    if (!(productSupplierRevertRecordsAffected > 0))
                    {
                        return (IGenericResult<bool>)_genericResult.Failure_InternalServerError(
                                                        ReplyMessages.FAILED_RECORDCREATED_WITH_ERRORS,
                                                        false,
                                                        error
                                                    );
                    }
                }

                if (productBackup != null)
                {
                    var productRevertRecordsAffected = await _productRepositoryStoredProceduresSqlServer.UpdateItemAsync(productBackup);
                    if (!(productRevertRecordsAffected > 0))
                    {
                        return (IGenericResult<bool>)_genericResult.Failure_InternalServerError(
                                                        ReplyMessages.FAILED_RECORDEDITED_WITH_ERRORS,
                                                        false,
                                                        error
                                                    );
                    }
                }

                return (IGenericResult<bool>)_genericResult.Failure_InternalServerError(
                                                        ReplyMessages.FAILED_EDITRECORD,
                                                        false,
                                                        error
                                                    );

            }
            catch (Exception)
            {
                return (IGenericResult<bool>)_genericResult.Failure_InternalServerError(
                                                        ReplyMessages.FAILED_RECORDEDITED_WITH_ERRORS,
                                                        false,
                                                        error
                                                    );
            }

        }
    }
}
