
using Mango.Web.Models;
using Mango.Web.Models.ShoppingCart;
using Mango.Web.Services.RequestProvider;
using Mango.Web.Services.ShoppingCart;
using Mango.Web.Utility;

namespace Mango.Web.Services.ShoppingCart
{
    public class CartService : ICartService
    {
        private readonly IRequestProvider _baseService;
        public CartService(IRequestProvider baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto)
        {
            return await _baseService.PostAsync<ResponseDto>(new RequestDto()
            {
                MethodType = SD.MethodType.POST,
                Data = cartDto,
                URL = SD.ShoppingCartAPIBase + "api/cart/ApplyCoupon",
                ContentType = SD.ContentType.Json,
            });
        }

        public async Task<ResponseDto?> GetCartByUserId(string userId)
        {
            return await _baseService.GetByIdAsync<ResponseDto>(new RequestDto()
            {
                MethodType = SD.MethodType.GET,
                URL = SD.ProductAPIBase + "api/cart/GetCart/" + userId
            });
        }

        public async Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId)
        {
            return await _baseService.DeleteAsync(new RequestDto()
            {
                MethodType = SD.MethodType.DELETE,
                URL = SD.ProductAPIBase + "api/cart/" + cartDetailsId
            });
        }

        public async Task<ResponseDto?> UpsertCartAsync(CartDto cartDto)
        {
            return await _baseService.PostAsync<ResponseDto>(new RequestDto()
            {
                MethodType = SD.MethodType.POST,
                Data = cartDto,
                URL = SD.ShoppingCartAPIBase + "api/cart/",
                ContentType = SD.ContentType.Json,
            });
        }
    }
}
