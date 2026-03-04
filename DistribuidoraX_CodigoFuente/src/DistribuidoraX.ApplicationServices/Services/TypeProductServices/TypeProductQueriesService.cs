using DistribuidoraX.Domain.Abstractions.UseCases.TypeProductUseCases;
using DistribuidoraX.Shared.Dtos.TypeProductDtos;
using DistribuidoraX.Shared.Responses;
using DistribuidoraX.Shared.Services.TypeProductServices;
using DistribuidoraX.Shared.Utilities;

namespace DistribuidoraX.ApplicationServices.Services.TypeProductServices
{
    internal class TypeProductQueriesService : ITypeProductQueriesService
    {
        private readonly IGetTypeProductsUseCase _typeProductsUseCase;
        private readonly IGenericResult<List<TypeProductDto>> _genericResultTypeProductList;
        public TypeProductQueriesService(IGetTypeProductsUseCase typeProductsUseCase, IGenericResult<List<TypeProductDto>> genericResultTypeProductList)
        {
            _typeProductsUseCase = typeProductsUseCase;
            _genericResultTypeProductList = genericResultTypeProductList;
        }

        public async Task<IGenericResult<List<TypeProductDto>>> GetListAsync()
        {
            try
            {
                List<TypeProductDto> typeProductListDto = [];
                var typeProductList = await _typeProductsUseCase.GetListAsync();
                if (typeProductList.Count > 0)
                {
                    foreach (var typeProduct in typeProductList)
                    {
                        typeProductListDto.Add(new TypeProductDto
                        {
                            TypeProductItem = typeProduct.TipoProductoId,
                            TypeProductName = typeProduct.Nombre
                        });
                    }
                }

                if (typeProductListDto.Count == 0)
                {
                    return _genericResultTypeProductList.Success(ReplyMessages.EMPTY_QUERY, typeProductListDto);
                }

                return _genericResultTypeProductList.Success(ReplyMessages.SUCCESSFULL_QUERY, typeProductListDto);
            }
            catch (Exception ex)
            {
                return _genericResultTypeProductList.Failure_InternalServerError(ReplyMessages.FAILED_OPERATION, null,null);
            }
        }
    }
}
