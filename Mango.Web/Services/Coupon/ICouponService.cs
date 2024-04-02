using Mango.Web.Models;
using Mango.Web.Models.Coupon;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Mango.Web.Services.Coupon
{
    public interface ICouponService
    {
        Task<IEnumerable<CouponItem>> GetCouponByCodeAsync(string couponCode);
		Task<IEnumerable<CouponItem>> GetAllCouponsAsync();
		Task<CouponItem> GetCouponByIdAsync(int id);
		Task<ResponseDto?> CreateCouponsAsync(CouponItem couponDto);
		Task<ResponseDto?> UpdateCouponsAsync(CouponItem couponDto);
		Task<ResponseDto?> DeleteCouponsAsync(int id);
	}
}
