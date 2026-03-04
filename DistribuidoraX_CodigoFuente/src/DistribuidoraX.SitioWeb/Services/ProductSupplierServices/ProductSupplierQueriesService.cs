using DistribuidoraX.Shared.Dtos.GenericDtos;
using DistribuidoraX.Shared.Dtos.ProductSupplierDtos;
using DistribuidoraX.Shared.Responses;
using DistribuidoraX.Shared.Services.ProductSupplierServices;
using DistribuidoraX.SitioWeb.Responses;
using Newtonsoft.Json;
using System.Text;

namespace DistribuidoraX.SitioWeb.Services.ProductSupplierServices
{
    public class ProductSupplierQueriesService : IProductSupplierQueriesService
    {
        private static string? _baseUrl;
        public ProductSupplierQueriesService()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            _baseUrl = builder.GetSection("ApiSettings:baseUrl").Value;
        }

        public async Task<IGenericResult<FullProductSupplierDto>> GetListByProductIdAsync(int productId)
        {
            var cliente = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl!)
            };

            var responseApi = await cliente.GetAsync($"api/ProductSuppliers/Product/{productId}");
            var jsonRespuesta = await responseApi.Content.ReadAsStringAsync();
            GenericResult<FullProductSupplierDto> resultado = JsonConvert.DeserializeObject<GenericResult<FullProductSupplierDto>>(jsonRespuesta)!;

            return resultado;
        }

        public async Task<IGenericResult<bool>> ExistProductSupplierCodeAsync(ExistCodeParametersDto productSupplierCodeParametersDto)
        {
            var cliente = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl!)
            };

            var content = new StringContent(JsonConvert.SerializeObject(productSupplierCodeParametersDto), Encoding.UTF8, "application/json");
            var requestResponseApi = await cliente.PostAsync("api/ProductSuppliers/ExistCode", content);

            var jsonRespuesta = await requestResponseApi.Content.ReadAsStringAsync();
            GenericResult<bool> resultado = JsonConvert.DeserializeObject<GenericResult<bool>>(jsonRespuesta)!;

            return resultado;
        }
    }
}
