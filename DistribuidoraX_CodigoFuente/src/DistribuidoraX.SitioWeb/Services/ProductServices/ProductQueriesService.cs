using DistribuidoraX.Shared.Dtos.GenericDtos;
using DistribuidoraX.Shared.Dtos.ProductDtos;
using DistribuidoraX.Shared.Responses;
using DistribuidoraX.Shared.Services.ProductServices;
using DistribuidoraX.SitioWeb.Responses;
using Newtonsoft.Json;
using System.Text;

namespace DistribuidoraX.SitioWeb.Services.ProductServices
{
    public class ProductQueriesService : IProductQueriesService
    {
        private static string? _baseUrl;
        public ProductQueriesService()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            _baseUrl = builder.GetSection("ApiSettings:baseUrl").Value;
        }

        public async Task<IGenericResult<List<ProductBaseDto>>> GetListByFiltersAsync(SearchProductsFiltersBaseDto filters)
        {
            var cliente = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl!)
            };

            var responseApi = await cliente.GetAsync("api/Products?productCodeFilter=" + filters.ProductCodeFilter + "&typeProductFilter=" + filters.TypeProductFilter);
            var jsonRespuesta = await responseApi.Content.ReadAsStringAsync();
            GenericResult<List<ProductBaseDto>> resultado = JsonConvert.DeserializeObject<GenericResult<List<ProductBaseDto>>>(jsonRespuesta)!;

            return resultado;
        }

        public async Task<IGenericResult<ProductBaseDto>> ProductByIdAsync(int id)
        {
            var cliente = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl!)
            };

            var responseApi = await cliente.GetAsync($"api/Products/Item/{id}");
            var jsonRespuesta = await responseApi.Content.ReadAsStringAsync();
            GenericResult<ProductBaseDto> resultado = JsonConvert.DeserializeObject<GenericResult<ProductBaseDto>>(jsonRespuesta)!;

            return resultado;
        }

        public async Task<IGenericResult<bool>> ExistProductCodeAsync(ExistCodeParametersDto existProductCodeDto)
        {
            var cliente = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl!)
            };

            var content = new StringContent(JsonConvert.SerializeObject(existProductCodeDto), Encoding.UTF8, "application/json");
            var requestResponseApi = await cliente.PostAsync("api/Products/ExistCode", content);

            var jsonRespuesta = await requestResponseApi.Content.ReadAsStringAsync();
            GenericResult<bool> resultado = JsonConvert.DeserializeObject<GenericResult<bool>>(jsonRespuesta)!;

            return resultado;
        }
    }
}
