using DistribuidoraX.ApplicationServices.Mappers.ProductMappers;
using DistribuidoraX.ApplicationServices.Mappers.ProductSupplierMappers;
using DistribuidoraX.Domain.Abstractions.UseCases.ProductSupplierUseCases.Hubs;
using DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases.Hubs;
using DistribuidoraX.Domain.Abstractions.UseCases.TransactionsRevertUseCases;
using DistribuidoraX.Domain.Entities;
using DistribuidoraX.Domain.Objects.GenericObjects;
using DistribuidoraX.Shared.Dtos.ProductDtos;
using DistribuidoraX.Shared.Dtos.ProductSupplierDtos;
using DistribuidoraX.Shared.Responses;
using DistribuidoraX.Shared.Services.ProductServices;
using DistribuidoraX.Shared.Utilities;
using FluentValidation;

namespace DistribuidoraX.ApplicationServices.Services.ProductServices
{
    public class ProductCommandsService : IProductCommandsService
    {
        private readonly IGenericResult<bool> _genericResult;
        private readonly IValidator<ProductFullDataDto> _productFullDataDtoValidator;
        private readonly IValidator<SupplierProductDto> _supplierProductDtoValidator;
        private readonly IHubTransactionsRevertUseCase _hubTransactionsRevertUseCase;
        private readonly IHubProductSupplierUseCases_StoredProceduresSqlServer _hubProductSupplierUseCasesStoredProceduresSqlServer;
        private readonly IHubProductUseCases_StoredProceduresSqlServer _hubProductUseCases_StoredProceduresSqlServer;
        public ProductCommandsService(
            IGenericResult<bool> genericResult,
            IValidator<ProductFullDataDto> productFullDataDtoValidator,
            IValidator<SupplierProductDto> supplierProductDtoValidator,
            IHubTransactionsRevertUseCase hubTransactionsRevertUseCase,
            IHubProductSupplierUseCases_StoredProceduresSqlServer hubProductSupplierUseCasesStoredProceduresSqlServer,
            IHubProductUseCases_StoredProceduresSqlServer hubProductUseCases_StoredProceduresSqlServer)
        {
            _genericResult = genericResult;
            _productFullDataDtoValidator = productFullDataDtoValidator;
            _supplierProductDtoValidator = supplierProductDtoValidator;
            _hubTransactionsRevertUseCase = hubTransactionsRevertUseCase;
            _hubProductSupplierUseCasesStoredProceduresSqlServer = hubProductSupplierUseCasesStoredProceduresSqlServer;
            _hubProductUseCases_StoredProceduresSqlServer = hubProductUseCases_StoredProceduresSqlServer;
        }

        public async Task<IGenericResult<bool>> DeleteProductoAsync(int productItem)
        {
            try
            {
                var productoResponse = await _hubProductUseCases_StoredProceduresSqlServer
                                            .GetProductUseCase_GetItemByIdAsync(productItem);
                
                if (productoResponse == null)
                    return _genericResult.Failure_NotFound(
                                                            ReplyMessages.OPERATION_FINISHED, 
                                                            false, 
                                                            ReplyMessages.EMPTYELEMENT_QUERY
                                                        );
                

                var elementosAfectados = await _hubProductUseCases_StoredProceduresSqlServer
                                            .DeleteProductUseCase_DeleteByParamsAsync(productoResponse!);
                
                if (elementosAfectados == 0)
                    return _genericResult.Failure_BadRequest(
                                                            ReplyMessages.OPERATION_FINISHED, 
                                                            false,
                                                            ReplyMessages.FAILED_DELETERECORD
                                                            );
                

                return _genericResult.Success(ReplyMessages.SUCCESSFULL_DELETEDRECORD, true);
            }
            catch (Exception ex)
            {
                return _genericResult.Failure_InternalServerError(ReplyMessages.FAILED_OPERATION, false, null);
            }
        }

        public async Task<IGenericResult<bool>> SaveProductAsync(ProductSupplierRequestDto productSupplierRequestDto)
        {
            int newProductIdResponse = -1;
            List<int> productSupplierIdsList = [];
            try
            {
                if (productSupplierRequestDto != null)
                {
                    if (productSupplierRequestDto.ProductDataDto != null)
                    {
                        //validations request
                        var productDataDtoValidationResult = await _productFullDataDtoValidator
                                                                .ValidateAsync(productSupplierRequestDto.ProductDataDto);
                        
                        if (!productDataDtoValidationResult.IsValid)
                            return _genericResult.Failure_BadRequest(
                                                                    ReplyMessages.FAILED_BADREQUEST, 
                                                                    false, 
                                                                    productDataDtoValidationResult.Errors[0].ErrorMessage
                                                                    );

                        var existProductCode = await _hubProductUseCases_StoredProceduresSqlServer
                                                    .ProductValidationsUseCase_ExistCodeAsync(
                                                                        new ExistCodeParameters 
                                                                        { 
                                                                            ItemId = productSupplierRequestDto.ProductDataDto.ProductItem,
                                                                            CodeValue= productSupplierRequestDto.ProductDataDto.ProductCode
                                                                        });
                        if (existProductCode)
                            return _genericResult.Failure_BadRequest(
                                                                        ReplyMessages.FAILED_BADREQUEST,
                                                                        false,
                                                                        ReplyMessages.FAILED_BADREQUEST_EXISTPRODUCTCODE
                                                                        );

                        foreach (var supplierProductDto in productSupplierRequestDto.ProductSupplierListDto!)
                        {
                            var supplierProductDataDtoValidationResult = await _supplierProductDtoValidator
                                                                            .ValidateAsync(supplierProductDto);
                            
                            if (!supplierProductDataDtoValidationResult.IsValid)
                                return _genericResult.Failure_BadRequest(
                                                                        ReplyMessages.FAILED_BADREQUEST, 
                                                                        false,
                                                                        supplierProductDataDtoValidationResult.Errors[0].ErrorMessage
                                                                        );

                            var existProductSupplierCode = await _hubProductSupplierUseCasesStoredProceduresSqlServer
                                                            .ProductSupplierValidationsUseCase_ExistCodeAsync(
                                                                                             new ExistCodeParameters
                                                                                             {
                                                                                                 ItemId = supplierProductDto.SupplierProductItem,
                                                                                                 CodeValue = supplierProductDto.SupplierProductCode
                                                                                             });

                            if (existProductSupplierCode)
                                return _genericResult.Failure_BadRequest(
                                                                            ReplyMessages.FAILED_BADREQUEST,
                                                                            false,
                                                                            ReplyMessages.FAILED_BADREQUEST_EXISTPRODUCTSUPPLIERCODE
                                                                            );

                        }

                        //newProduct mapping
                        var productEntityMapping = ProductMapping.ProductoEntityMapper(productSupplierRequestDto.ProductDataDto!);
                        
                        //save product
                        newProductIdResponse = await _hubProductUseCases_StoredProceduresSqlServer
                                                        .NewProductUseCase_SaveItemAsync(productEntityMapping);
                        
                        if (!(newProductIdResponse > 0))
                            return _genericResult.Failure_InternalServerError(
                                                                                ReplyMessages.FAILED_CREATERECORD, 
                                                                                false, 
                                                                                null
                                                                            );
                        

                        //productSupplier mapping
                        var productSupplierListBackup = await _hubProductSupplierUseCasesStoredProceduresSqlServer
                                                            .GetProductSuppliersUseCase_GetListByProductIdAsync(newProductIdResponse);
                        
                        var productSupplierListsMapping = ProductSupplierMapping.ProductSupplierMappingLists(
                                                                                    productSupplierRequestDto.ProductSupplierListDto, 
                                                                                    productSupplierListBackup, 
                                                                                    newProductIdResponse
                                                                                );

                        //save productSupplier
                        foreach (var addProductSupplier in productSupplierListsMapping.AddProductSupplierList)
                        {
                            var newProductSupplierIdResponse = await _hubProductSupplierUseCasesStoredProceduresSqlServer.
                                                                    NewProductSupplierUseCase_SaveItemAsync(addProductSupplier);
                            
                            if (!(newProductSupplierIdResponse > 0))
                            {
                                return (IGenericResult<bool>)await _hubTransactionsRevertUseCase
                                                                    .NewProductAndProductSupplierTransactionRevertAsync(
                                                                                newProductIdResponse, 
                                                                                productSupplierIdsList, 
                                                                                ReplyMessages.FAILED_UNEXPECTEDERROR_SUPPLIER
                                                                            );
                            }

                            productSupplierIdsList.Add(newProductSupplierIdResponse);
                        }

                        var updateProductPriceResponse = await _hubProductUseCases_StoredProceduresSqlServer.EditProductSupplierUseCase_UpdatePriceAsync(newProductIdResponse);
                        if (!(updateProductPriceResponse > 0))
                        {
                            return (IGenericResult<bool>)await _hubTransactionsRevertUseCase
                                                                    .NewProductAndProductSupplierTransactionRevertAsync(
                                                                                newProductIdResponse,
                                                                                productSupplierIdsList,
                                                                                ReplyMessages.FAILED_UNEXPECTEDERROR_UPDATEPRICE
                                                                            );
                        }

                    }
                    else
                    {
                        return _genericResult.Failure_BadRequest(
                                                                    ReplyMessages.FAILED_CREATERECORD, 
                                                                    false, 
                                                                    ReplyMessages.FAILED_BADREQUEST_NULLPARAMETER
                                                                );

                    }
                }
                else
                {
                    return _genericResult.Failure_BadRequest(
                                                                ReplyMessages.FAILED_CREATERECORD, 
                                                                false,
                                                                ReplyMessages.FAILED_BADREQUEST_NULLPARAMETER
                                                            );

                }

                return _genericResult.Success(ReplyMessages.OPERATION_FINISHED, true);
            }
            catch (Exception ex)
            {
                if (newProductIdResponse > 0)
                    return (IGenericResult<bool>)await _hubTransactionsRevertUseCase
                                                                    .NewProductAndProductSupplierTransactionRevertAsync(
                                                                                newProductIdResponse,
                                                                                productSupplierIdsList,
                                                                                ReplyMessages.FAILED_INTERNALSERVERERROR_UNEXPECTEDERROR
                                                                            );
                

                return _genericResult.Failure_InternalServerError(ReplyMessages.FAILED_OPERATION, false, null);
            }

        }

        public async Task<IGenericResult<bool>> UpdateProductAsync(ProductSupplierRequestDto productSupplierRequestDto, int productItem)
        {
            int updateProductRowsAffected = -1;
            Producto productBackup = new();
            List<ProductoProveedor> productSupplierListBackup = [];
            try
            {
                if (productSupplierRequestDto != null) 
                {
                    if (productSupplierRequestDto.ProductDataDto != null)
                    {
                        List<int> newProductSupplierIdsList = [];

                        //validations request
                        if (productSupplierRequestDto.ProductDataDto.ProductItem != productItem)
                            return _genericResult.Failure_BadRequest(
                                                                        ReplyMessages.FAILED_BADREQUEST, 
                                                                        false, 
                                                                        ReplyMessages.FAILED_BADREQUEST_FIELDNOTVALID
                                                                    );
                        

                        var productDataDtoValidationResult = await _productFullDataDtoValidator
                                                                .ValidateAsync(productSupplierRequestDto.ProductDataDto);
                        
                        if (!productDataDtoValidationResult.IsValid)
                            return _genericResult.Failure_BadRequest(
                                                                    ReplyMessages.FAILED_BADREQUEST, 
                                                                    false,
                                                                    productDataDtoValidationResult.Errors[0].ErrorMessage
                                                                    );

                        var existProductCode = await _hubProductUseCases_StoredProceduresSqlServer
                                                    .ProductValidationsUseCase_ExistCodeAsync(
                                                                        new ExistCodeParameters
                                                                        {
                                                                            ItemId = productSupplierRequestDto.ProductDataDto.ProductItem,
                                                                            CodeValue = productSupplierRequestDto.ProductDataDto.ProductCode
                                                                        });
                        if (existProductCode)
                            return _genericResult.Failure_BadRequest(
                                                                        ReplyMessages.FAILED_BADREQUEST,
                                                                        false,
                                                                        ReplyMessages.FAILED_BADREQUEST_EXISTPRODUCTCODE
                                                                        );

                        foreach (var supplierProductDto in productSupplierRequestDto.ProductSupplierListDto!)
                        {
                            var supplierProductDataDtoValidationResult = await _supplierProductDtoValidator
                                                                            .ValidateAsync(supplierProductDto);
                            
                            if (!supplierProductDataDtoValidationResult.IsValid)
                                return _genericResult.Failure_BadRequest(
                                                                        ReplyMessages.FAILED_BADREQUEST, 
                                                                        false,
                                                                        supplierProductDataDtoValidationResult.Errors[0].ErrorMessage
                                                                        );


                            var existProductSupplierCode = await _hubProductSupplierUseCasesStoredProceduresSqlServer
                                                            .ProductSupplierValidationsUseCase_ExistCodeAsync(
                                                                                             new ExistCodeParameters
                                                                                             {
                                                                                                 ItemId = supplierProductDto.SupplierProductItem,
                                                                                                 CodeValue = supplierProductDto.SupplierProductCode
                                                                                             });

                            if (existProductSupplierCode)
                                return _genericResult.Failure_BadRequest(
                                                                            ReplyMessages.FAILED_BADREQUEST,
                                                                            false,
                                                                            ReplyMessages.FAILED_BADREQUEST_EXISTPRODUCTSUPPLIERCODE
                                                                            );


                        }

                        //newProduct mapping
                        productBackup = await _hubProductUseCases_StoredProceduresSqlServer
                                                .GetProductUseCase_GetItemByIdAsync(productSupplierRequestDto.ProductDataDto.ProductItem);
                        var productEntityMapping = ProductMapping
                                                    .ProductoEntityMapper(productSupplierRequestDto.ProductDataDto!);

                        //productSupplier mapping
                        productSupplierListBackup = await _hubProductSupplierUseCasesStoredProceduresSqlServer.
                                                        GetProductSuppliersUseCase_GetListByProductIdAsync(productSupplierRequestDto.ProductDataDto.ProductItem);
                        var productSupplierListsMapping = ProductSupplierMapping.ProductSupplierMappingLists(
                                                                                    productSupplierRequestDto.ProductSupplierListDto,
                                                                                    productSupplierListBackup,
                                                                                    productSupplierRequestDto.ProductDataDto.ProductItem
                                                                                );
                        if (productSupplierListsMapping.IncoherentMapping)
                            return _genericResult.Failure_NotFound(
                                                                    ReplyMessages.FAILED_EDITRECORD, 
                                                                    false, 
                                                                    ReplyMessages.FAILED_NOTFOUND_RECORD
                                                                  );
                        

                        updateProductRowsAffected = await _hubProductUseCases_StoredProceduresSqlServer
                                                            .EditProductUseCase_UpdateItemAsync(productEntityMapping);
                        
                        if (!(updateProductRowsAffected > 0))
                            return _genericResult.Failure_InternalServerError(
                                                        ReplyMessages.FAILED_EDITRECORD,
                                                        false,
                                                        null
                                                    );
                        

                        //save newProductSupplier
                        foreach (var addProductSupplier in productSupplierListsMapping.AddProductSupplierList)
                        {
                            var newProductSupplierIdResponse = await _hubProductSupplierUseCasesStoredProceduresSqlServer
                                                                    .NewProductSupplierUseCase_SaveItemAsync(addProductSupplier);
                            
                            if (!(newProductSupplierIdResponse > 0))
                                return (IGenericResult<bool>)await _hubTransactionsRevertUseCase
                                                                .EditProductAndNewProductSupplierTransactionRevertAsync(
                                                                                            productBackup, 
                                                                                            newProductSupplierIdsList, 
                                                                                            ReplyMessages.FAILED_INTERNALSERVERERROR_UNEXPECTEDERROR_RECORD
                                                                                        );
                            

                            newProductSupplierIdsList.Add(newProductSupplierIdResponse);
                        }

                        //update ProductSupplier
                        foreach (var updateProductSupplier in productSupplierListsMapping.UpdateProductSupplierList)
                        {
                            var updateProductSupplierIdAffected = await _hubProductSupplierUseCasesStoredProceduresSqlServer
                                                                    .EditProductSupplierUseCase_UpdateItemAsync(
                                                                                                updateProductSupplier,
                                                                                                false
                                                                                            );
                            
                            if (!(updateProductSupplierIdAffected > 0))
                                return (IGenericResult<bool>)await _hubTransactionsRevertUseCase
                                                                .EditProductAndProductSupplierTransactionRevertAsync(
                                                                                productBackup, 
                                                                                productSupplierListBackup,
                                                                                ReplyMessages.FAILED_INTERNALSERVERERROR_UNEXPECTEDERROR_RECORD
                                                                            );

                            
                        }

                        //deleteUpdate ProductSupplier
                        foreach (var deleteUpdateProductSupplier in productSupplierListsMapping.DeleteProductSupplierList)
                        {
                            var deleteUpdateProductSupplierIdAffected = await _hubProductSupplierUseCasesStoredProceduresSqlServer
                                                                            .DeleteProductSupplierUseCase_DeleteItemByStateAsync(deleteUpdateProductSupplier);
                            
                            if (!(deleteUpdateProductSupplierIdAffected > 0))
                                return (IGenericResult<bool>)await _hubTransactionsRevertUseCase.
                                                                EditProductAndProductSupplierTransactionRevertAsync(
                                                                                    productBackup, 
                                                                                    productSupplierListBackup,
                                                                                    ReplyMessages.FAILED_INTERNALSERVERERROR_UNEXPECTEDERROR_RECORD
                                                                                );
                            
                        }


                        var updateProductPriceResponse = await _hubProductUseCases_StoredProceduresSqlServer
                                                            .EditProductSupplierUseCase_UpdatePriceAsync(
                                                                                    productEntityMapping.ProductoId
                                                                                );

                        if(!(updateProductPriceResponse > 0))
                            return (IGenericResult<bool>)await _hubTransactionsRevertUseCase.
                                                                EditProductAndProductSupplierTransactionRevertAsync(
                                                                                    productBackup,
                                                                                    productSupplierListBackup,
                                                                                    ReplyMessages.FAILED_INTERNALSERVERERROR_UNEXPECTEDERROR_RECORD
                                                                                );

                    }

                }

                return _genericResult.Success(ReplyMessages.OPERATION_FINISHED, true);
            }
            catch (Exception)
            {
                if (updateProductRowsAffected > 0)
                    return (IGenericResult<bool>)await _hubTransactionsRevertUseCase
                                                .EditProductAndProductSupplierTransactionRevertAsync(
                                                                                    productBackup,
                                                                                    productSupplierListBackup,
                                                                                    ReplyMessages.FAILED_INTERNALSERVERERROR_UNEXPECTEDERROR
                                                                                );
                

                return _genericResult.Failure_InternalServerError(ReplyMessages.FAILED_OPERATION, false, null);
            }
        }
    }
}
