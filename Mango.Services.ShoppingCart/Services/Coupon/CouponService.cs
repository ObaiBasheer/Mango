using Mango.Services.ShoppingCartAPI.Models.Dtos;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Services.Coupon
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CouponDto> CouponByCode(string code)
        {
            var client = _httpClientFactory.CreateClient("Coupon");
            var httpResponse =await client.GetAsync($"/api/v1/coupon/items/byCode/{code}");
            var content = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CouponDto>(content);

            if (result != null)
            {
                return result;
            }
            return new CouponDto();
        }
    }
}
