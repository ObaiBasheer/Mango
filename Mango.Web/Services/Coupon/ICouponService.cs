using Mango.Web.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Mango.Web.Services.Coupon
{
    public interface ICouponService
    {
        Task<ResponseDto> GetCouponsAsync();
        Task<ResponseDto> GetCouponById(int id);
        Task<ResponseDto> GetCouponByCode(string code);
        Task<ResponseDto> CreateCoupon(Models.Coupon.Coupon coupon);
        Task<ResponseDto> UpdateCoupon(Models.Coupon.Coupon coupon);
        Task DeleteCoupon(int id);
    }
}
