using DistribuidoraX.Shared.Dtos.ProductSupplierDtos;
using DistribuidoraX.Shared.Responses;
using DistribuidoraX.Shared.Services.ProductServices;
using DistribuidoraX.SitioWeb.Responses;
using Newtonsoft.Json;
using System.Text;

namespace DistribuidoraX.SitioWeb.Services.ProductServices
{
    public class ProductCommandsService : IProductCommandsService
    {
        private static string? _baseUrl;
        public ProductCommandsService()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            _baseUrl = builder.GetSection("ApiSettings:baseUrl").Value;
        }

        public async Task<IGenericResult<bool>> DeleteProductoAsync(int productItem)
        {
            var cliente = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl!)
            };

            var responseApi = await cliente.DeleteAsync($"api/Products/Item/{productItem}");
            var jsonRespuesta = await responseApi.Content.ReadAsStringAsync();
            GenericResult<bool> resultado = JsonConvert.DeserializeObject<GenericResult<bool>>(jsonRespuesta)!;

            return resultado;
        }

        public async Task<IGenericResult<bool>> SaveProductAsync(ProductSupplierRequestDto productSupplierRequestDto)
        {
            var cliente = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl!)
            };

            var content = new StringContent(JsonConvert.SerializeObject(productSupplierRequestDto), Encoding.UTF8, "application/json");
            var requestResponseApi = await cliente.PostAsync("api/Products/Item", content);

            var jsonRespuesta = await requestResponseApi.Content.ReadAsStringAsync();
            GenericResult<bool> resultado = JsonConvert.DeserializeObject<GenericResult<bool>>(jsonRespuesta)!;

            return resultado;
        }

        public async Task<IGenericResult<bool>> UpdateProductAsync(ProductSupplierRequestDto productSupplierRequestDto, int productItem)
        {
            var cliente = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl!)
            };

            var content = new StringContent(JsonConvert.SerializeObject(productSupplierRequestDto), Encoding.UTF8, "application/json");
            var requestResponseApi = await cliente.PutAsync($"api/Products/Item/{productItem}", content);

            var jsonRespuesta = await requestResponseApi.Content.ReadAsStringAsync();
            GenericResult<bool> resultado = JsonConvert.DeserializeObject<GenericResult<bool>>(jsonRespuesta)!;

            return resultado;
        }
    }
}
