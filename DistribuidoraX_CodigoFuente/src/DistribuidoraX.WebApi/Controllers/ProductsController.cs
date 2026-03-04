using Azure;
using DistribuidoraX.Shared.Dtos.GenericDtos;
using DistribuidoraX.Shared.Dtos.ProductDtos;
using DistribuidoraX.Shared.Dtos.ProductSupplierDtos;
using DistribuidoraX.Shared.Services.ProductServices;
using Microsoft.AspNetCore.Mvc;

namespace DistribuidoraX.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductQueriesService _productQueriesService;
        private readonly IProductCommandsService _productCommandsService;
        public ProductsController(IProductQueriesService productQueriesService, IProductCommandsService productCommandsService)
        {
            _productQueriesService = productQueriesService;
            _productCommandsService = productCommandsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsByFiltersAsync([FromQuery] string? productCodeFilter, string? typeProductFilter)
        {
            SearchProductsFiltersBaseDto filters = new()
            {
                ProductCodeFilter = productCodeFilter,
                TypeProductFilter = typeProductFilter
            };

            var response = await _productQueriesService.GetListByFiltersAsync(filters);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("Item/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _productQueriesService.ProductByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("ExistCode")]
        public async Task<IActionResult> AddProductAsync([FromBody] ExistCodeParametersDto productCodeParametersDto)
        {
            var response = await _productQueriesService.ExistProductCodeAsync(productCodeParametersDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("Item")]
        public async Task<IActionResult> AddProductAsync([FromBody] ProductSupplierRequestDto productSupplierRequestDto)
        {
            var response = await _productCommandsService.SaveProductAsync(productSupplierRequestDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("Item/{id}")]
        public async Task<IActionResult> EditProductAsync([FromBody] ProductSupplierRequestDto productSupplierRequestDto, int id)
        {
            var response = await _productCommandsService.UpdateProductAsync(productSupplierRequestDto, id);
            return StatusCode(response.StatusCode, response);
        }


        [HttpDelete("Item/{id}")]
        public async Task<IActionResult> EliminarActualizarProducto(int id)
        {
            var response = await _productCommandsService.DeleteProductoAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
