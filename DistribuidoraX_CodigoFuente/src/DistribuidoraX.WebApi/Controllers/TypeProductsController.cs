using DistribuidoraX.Shared.Services.TypeProductServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DistribuidoraX.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeProductsController : ControllerBase
    {
        private readonly ITypeProductQueriesService _typeProductQueriesService;
        public TypeProductsController(ITypeProductQueriesService typeProductQueriesService)
        {
            _typeProductQueriesService = typeProductQueriesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync()
        {
            var response = await _typeProductQueriesService.GetListAsync();
            return StatusCode(response.StatusCode, response);
        }
    }
}
