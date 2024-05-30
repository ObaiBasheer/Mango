using Mango.Services.ShoppingCartAPI.Models.Dtos;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Mango.Services.ShoppingCartAPI.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ProductService(IHttpClientFactory httpClientFactory)
        {

            _httpClientFactory = httpClientFactory;

        }
        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var client = _httpClientFactory.CreateClient("Product");
            HttpResponseMessage responseMessage = await client.GetAsync($"api/products");
            var content = await responseMessage.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResponseDto>(content);

            if(result!.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(result.Result)!)!;
            }
            return new List<ProductDto>();   

        }
    }
}
