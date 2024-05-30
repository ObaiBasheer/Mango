using Mango.Services.ShoppingCartAPI.Models.Dtos;

namespace Mango.Services.ShoppingCartAPI.Services.Coupon
{
    public interface ICouponService
    {
        Task<CouponDto> CouponByCode(string code);
    }
}
