using DistribuidoraX.Domain.Abstractions.UseCases.ProductUseCases;
using DistribuidoraX.Domain.Objects.GenericObjects;
using DistribuidoraX.Domain.Objects.ProductObjects;
using DistribuidoraX.Shared.Dtos.GenericDtos;
using DistribuidoraX.Shared.Dtos.ProductDtos;
using DistribuidoraX.Shared.Responses;
using DistribuidoraX.Shared.Services.ProductServices;
using DistribuidoraX.Shared.Utilities;
using FluentValidation;

namespace DistribuidoraX.ApplicationServices.Services.ProductServices
{
    public class ProductQueriesService : IProductQueriesService
    {
        private readonly IGetProductListUseCase _getproductListUseCase;
        private readonly IGetProductUseCase _getProductUseCase;
        private readonly IGenericResult<List<ProductBaseDto>> _genericResultProductListDto;
        private readonly IGenericResult<ProductBaseDto> _genericResultProductDto;
        private readonly IProductValidationsUseCase _productValidationsUseCase;
        private readonly IValidator<ExistCodeParametersDto> _existCodeParametersDtoValidator;
        private readonly IGenericResult<bool> _genericResultBool;

        public ProductQueriesService(IGetProductListUseCase getproductListUseCase, IGetProductUseCase getProductUseCase, IGenericResult<List<ProductBaseDto>> genericResultProductListDto, IGenericResult<ProductBaseDto> genericResultProductDto, IProductValidationsUseCase productValidationsUseCase, IValidator<ExistCodeParametersDto> existCodeParametersDtoValidator, IGenericResult<bool> genericResultBool)
        {
            _getproductListUseCase = getproductListUseCase;
            _getProductUseCase = getProductUseCase;
            _genericResultProductListDto = genericResultProductListDto;
            _genericResultProductDto = genericResultProductDto;
            _productValidationsUseCase = productValidationsUseCase;
            _existCodeParametersDtoValidator = existCodeParametersDtoValidator;
            _genericResultBool = genericResultBool;
        }

        public async Task<IGenericResult<List<ProductBaseDto>>> GetListByFiltersAsync(SearchProductsFiltersBaseDto filtersDto)
        {
            try
            {
                ProductFiltersParametersObject productfiltersParameterObject = new()
                {
                    ProductCodeFilter = filtersDto.ProductCodeFilter,
                    TypeProductFilter = filtersDto.TypeProductFilter
                };

                var productBaseListDto = await _getproductListUseCase.GetListByFiltersAsync(productfiltersParameterObject);
                List<ProductBaseDto> response = [];
                if (productBaseListDto!.Count > 0)
                {
                    foreach (var product in productBaseListDto)
                    {
                        response.Add(new ProductBaseDto
                        {
                            ProductItem = product.ProductoId,
                            ProductName = product.Nombre,
                            ProductCode = product.ClaveInterna,
                            ProductPrice = product.Precio
                        });
                    }
                }

                return _genericResultProductListDto.Success(ReplyMessages.SUCCESSFULL_QUERY, response);
            }
            catch (Exception ex)
            {
                return _genericResultProductListDto.Failure_InternalServerError(ReplyMessages.FAILED_OPERATION, null, null);
            }
        }

        public async Task<IGenericResult<ProductBaseDto>> ProductByIdAsync(int id)
        {
            try
            {
                var product = await _getProductUseCase.GetItemById_StoredProceduresSqlServerAsyncAsync(id);
                if (product == null || product.ProductoId == 0)
                {
                    return _genericResultProductDto.Failure_BadRequest(ReplyMessages.EMPTYELEMENT_QUERY, null,null);
                }

                ProductBaseDto productResponse = new()
                {
                    ProductItem = product.ProductoId,
                    ProductName = product.Nombre,
                    ProductCode = product.ClaveInterna,
                    ProductPrice = product.Precio
                };

                return _genericResultProductDto.Success(ReplyMessages.SUCCESSFULL_QUERY, productResponse);
            }
            catch (Exception ex)
            {
                return _genericResultProductDto.Failure_InternalServerError(ReplyMessages.FAILED_OPERATION, null, null);
            }
        }

        public async Task<IGenericResult<bool>> ExistProductCodeAsync(ExistCodeParametersDto productCodeParametersDto)
        {
            try
            {
                var productCodeDtoValidationResult = await _existCodeParametersDtoValidator
                                                            .ValidateAsync(productCodeParametersDto);

                if (!productCodeDtoValidationResult.IsValid)
                    return _genericResultBool.Failure_BadRequest(
                                                            ReplyMessages.FAILED_BADREQUEST,
                                                            false,
                                                            productCodeDtoValidationResult.Errors[0].ErrorMessage
                                                            );

                ExistCodeParameters productCodeParameters = new()
                {
                    ItemId = productCodeParametersDto.ItemId,
                    CodeValue = productCodeParametersDto.CodeValue
                };

                var existProductCode = await _productValidationsUseCase.ExistProductCode_StoredProceduresSqlServerAsync(productCodeParameters);
                return _genericResultBool.Success(ReplyMessages.SUCCESSFULL_QUERY, existProductCode);
            }
            catch (Exception)
            {
                return _genericResultBool.Failure_InternalServerError(ReplyMessages.FAILED_OPERATION, false, null);
            }
            
        }

    }
}
