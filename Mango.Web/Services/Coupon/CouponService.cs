using Mango.Web.Models;
using Mango.Web.Models.Coupon;
using Mango.Web.Services.RequestProvider;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;

namespace Mango.Web.Services.Coupon
{
    public class CouponService : ICouponService
    {
        private readonly IRequestProvider _requestProvider;

        public CouponService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

		public async Task<ResponseDto?> CreateCouponsAsync(CouponItem couponDto)
		{
			try
			{
                var coupon = await _requestProvider.PostAsync<CouponRoot>(new RequestDto()
                {
                    MethodType = SD.MethodType.POST,
                    Data = couponDto,
                    URL = SD.CouponURLBase + "api/v1/coupon/items"
                });

                return new ResponseDto { IsSuccess = true, Message = "Coupon Created Successfully" };
            }
			catch (Exception ex)
			{
                return new ResponseDto { IsSuccess = false, Message = ex.Message };
            }
			
		}

		public async Task<ResponseDto?> DeleteCouponsAsync(int id)
		{
			try
			{
                await _requestProvider.DeleteAsync(new RequestDto()
                {
                    MethodType = SD.MethodType.DELETE,
                    URL = SD.CouponURLBase + "api/v1/coupon/items/" + id
                });
                return new ResponseDto { IsSuccess = true, Message = "Coupon Deleted Successfully" };
            }
			catch (Exception ex)
			{

                return new ResponseDto { IsSuccess = false, Message = ex.Message };

            }



        }

		public async Task<IEnumerable<CouponItem>> GetAllCouponsAsync()
		{
			try
			{
                var couponItem = await _requestProvider.GetAllAsync<CouponRoot>(new RequestDto()
                {
                    MethodType = SD.MethodType.GET,
                    URL = SD.CouponURLBase + "api/v1/coupon/items"
                });

				return couponItem.Data!;
            }
			catch 
			{

				return  Enumerable.Empty<CouponItem>();
			}
			
		}
        public async Task<IEnumerable<CouponItem>> GetCouponByCodeAsync(string couponCode)
		{
			try
			{
                CouponRoot coupon = await _requestProvider.GetByCodeAsync<CouponRoot>(new RequestDto()
                {
                    MethodType = SD.MethodType.GET,
                    URL = SD.CouponURLBase + "api/v1/coupon/items/byCode/" + couponCode
                });

                return coupon?.Data ?? null!;
            }
			catch 
			{

                return Enumerable.Empty<CouponItem>();
            }
			
		}
		public async Task<CouponItem> GetCouponByIdAsync(int id)
		{
			try
			{
                var couponItem = await _requestProvider.GetByIdAsync<CouponItem>(new RequestDto()
                {
                    MethodType = SD.MethodType.GET,
                    URL = SD.CouponURLBase + "api/v1/coupon/items/byId/" + id
                });

                return couponItem;
            }
			catch 
			{

                return null!;
            }
			
		}

		public async Task<ResponseDto?> UpdateCouponsAsync(CouponItem couponItem)
		{
			try
			{
                var coupon = await _requestProvider.PutAsync<CouponRoot>(new RequestDto()
                {
                    MethodType = SD.MethodType.PUT,
                    Data = couponItem,
                    URL = SD.CouponURLBase + "api/v1/items"
                });

                return new ResponseDto { IsSuccess = true, Message = "Coupon Updated Successfully" };
            }
			catch (Exception ex)
			{

                return new ResponseDto { IsSuccess = false, Message = ex.Message };

            }

        }
	}
}
