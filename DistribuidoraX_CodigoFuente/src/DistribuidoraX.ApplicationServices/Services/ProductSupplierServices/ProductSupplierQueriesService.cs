using DistribuidoraX.Domain.Abstractions.UseCases.ProductSupplierUseCases;
using DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases;
using DistribuidoraX.Domain.Abstractions.UseCases.SupplierUseCases;
using DistribuidoraX.Domain.Abstractions.UseCases.TypeProductUseCases;
using DistribuidoraX.Domain.Objects.GenericObjects;
using DistribuidoraX.Shared.Dtos.GenericDtos;
using DistribuidoraX.Shared.Dtos.ProductSupplierDtos;
using DistribuidoraX.Shared.Dtos.TypeProductDtos;
using DistribuidoraX.Shared.Responses;
using DistribuidoraX.Shared.Services.ProductSupplierServices;
using DistribuidoraX.Shared.Utilities;
using FluentValidation;

namespace DistribuidoraX.ApplicationServices.Services.ProductSupplierServices
{
    public class ProductSupplierQueriesService : IProductSupplierQueriesService
    {
        private readonly IGetTypeProductsUseCase _getTypeProductsUseCase;
        private readonly IGetProductUseCase _getProductUseCase;
        private readonly IGetProductSuppliersUseCase _getProductSuppliersUseCase;
        private readonly IGenericResult<FullProductSupplierDto> _genericResultFullProductSupplierDto;
        private readonly IGetSupplierUseCase _getSupplierUseCase;
        private readonly IProductSupplierValidationsUseCase _productSupplierValidationsUseCase;
        private readonly IValidator<ExistCodeParametersDto> _existCodeParametersDtoValidator;
        private readonly IGenericResult<bool> _genericResultBool;

        public ProductSupplierQueriesService(IGetTypeProductsUseCase getTypeProductsUseCase,
            IGetProductUseCase getProductUseCase, IGetProductSuppliersUseCase getProductSuppliersUseCase,
            IGenericResult<FullProductSupplierDto> genericResultFullProductSupplierDto, IGetSupplierUseCase getSupplierUseCase, IProductSupplierValidationsUseCase productSupplierValidationsUseCase, IValidator<ExistCodeParametersDto> existCodeParametersDtoValidator, IGenericResult<bool> genericResultBool)
        {
            _getTypeProductsUseCase = getTypeProductsUseCase;
            _getProductUseCase = getProductUseCase;
            _getProductSuppliersUseCase = getProductSuppliersUseCase;
            _genericResultFullProductSupplierDto = genericResultFullProductSupplierDto;
            _getSupplierUseCase = getSupplierUseCase;
            _productSupplierValidationsUseCase = productSupplierValidationsUseCase;
            _existCodeParametersDtoValidator = existCodeParametersDtoValidator;
            _genericResultBool = genericResultBool;
        }

        public async Task<IGenericResult<FullProductSupplierDto>> GetListByProductIdAsync(int productId)
        {
            try
            {
                
                if (productId <= 0)
                {
                    return _genericResultFullProductSupplierDto.Failure_BadRequest(ReplyMessages.OPERATION_FINISHED, null, ReplyMessages.FAILED_PARAMETERNOTVALID);
                }

                var product = await _getProductUseCase.GetItemById_StoredProceduresSqlServerAsyncAsync(productId);
                if (product == null || product.ProductoId == 0)
                {
                    return _genericResultFullProductSupplierDto.Failure_BadRequest(ReplyMessages.OPERATION_FINISHED, null, ReplyMessages.EMPTYELEMENT_QUERY);
                }

                var typeProductList = await _getTypeProductsUseCase.GetListAsync();
                List<TypeProductDto> typeProductListDto = [];
                foreach(var typeProduct in typeProductList) 
                {
                    typeProductListDto.Add(new TypeProductDto { 
                        TypeProductItem = typeProduct.TipoProductoId,
                        TypeProductName = typeProduct.Nombre
                    });
                }

                var productSupplierList = await _getProductSuppliersUseCase.GetListByProductIdAsync(productId);
                List<SupplierProductDto> supplierProductListDto = [];
                foreach (var productSupplier in productSupplierList)
                {
                    var supplierEntity = await _getSupplierUseCase.GetByIdAsync(productSupplier.ProveedorId);
                    supplierProductListDto.Add(new SupplierProductDto
                    {
                        SupplierProductItem = productSupplier.ProductoProveedorId,
                        SupplierProductCode = productSupplier.ClaveProductoProveedor,
                        SupplierProductCost = productSupplier.Costo,
                        SupplierItem = productSupplier.ProveedorId,
                        SupplierName = supplierEntity.Nombre
                    });
                }

                FullProductSupplierDto fullProductSupplierDto = new()
                {
                    ProductItem = product.ProductoId,
                    ProductName = product.Nombre,
                    ProductCode = product.ClaveInterna,
                    ProductActive = product.Activo,
                    TypeProductId = product.TipoProductoId,
                    TypeProductList = typeProductListDto,
                    ProductSupplierList = supplierProductListDto
                };


                return _genericResultFullProductSupplierDto.Success(ReplyMessages.SUCCESSFULL_QUERY,fullProductSupplierDto);
            }
            catch (Exception ex)
            {
                return _genericResultFullProductSupplierDto.Failure_InternalServerError(ReplyMessages.FAILED_OPERATION, null, null);
            }
        }

        public async Task<IGenericResult<bool>> ExistProductSupplierCodeAsync(ExistCodeParametersDto productSupplierCodeParametersDto)
        {
            try
            {
                var productSupplierCodeParametersDtoValidationResult = await _existCodeParametersDtoValidator
                                                            .ValidateAsync(productSupplierCodeParametersDto);

                if (!productSupplierCodeParametersDtoValidationResult.IsValid)
                    return _genericResultBool.Failure_BadRequest(
                                                            ReplyMessages.FAILED_BADREQUEST,
                                                            false,
                                                            productSupplierCodeParametersDtoValidationResult.Errors[0].ErrorMessage
                                                            );

                ExistCodeParameters productCodeParameters = new()
                {
                    ItemId = productSupplierCodeParametersDto.ItemId,
                    CodeValue = productSupplierCodeParametersDto.CodeValue
                };

                var existProductCode = await _productSupplierValidationsUseCase.ExistProductSupplierCode_StoredProceduresSqlServerAsync(productCodeParameters);
                return _genericResultBool.Success(ReplyMessages.SUCCESSFULL_QUERY, existProductCode);
            }
            catch (Exception)
            {
                return _genericResultBool.Failure_InternalServerError(ReplyMessages.FAILED_OPERATION, false, null);
            }
        }
    }
}
