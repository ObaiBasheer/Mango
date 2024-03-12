using Mango.Web.Models;
using Mango.Web.Services.RequestProvider;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Mango.Web.Services.Coupon
{
    public class CouponService : ICouponService
    {
        private readonly IRequestProvider _requestProvider;

        public CouponService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }
        public async Task<ResponseDto> CreateCoupon(Models.Coupon.Coupon coupon)
        {
            RequestDto requestDto = new RequestDto()
            {
                URL = SD.CouponURLBase + $"api/v1/coupon/items/",
                Data = coupon

            };
            return await _requestProvider.PostAsync<ResponseDto>(requestDto);
        }

        public async Task DeleteCoupon(int id)
        {
            RequestDto requestDto = new RequestDto()
            {
                URL = SD.CouponURLBase + $"api/v1/coupon/items/{id}"

            };
             await _requestProvider.DeleteAsync(requestDto);
        }

        public async Task<ResponseDto> GetCouponByCode(string code)
        {
            RequestDto requestDto = new RequestDto()
            {
                URL = SD.CouponURLBase + $"api/v1/coupon/items/byCode/{code}"

            };
            return await _requestProvider.GetByCodeAsync<ResponseDto>(requestDto);
        }

        public async Task<ResponseDto> GetCouponById(int id)
        {
            RequestDto requestDto = new RequestDto()
            {
                URL = SD.CouponURLBase + $"api/v1/coupon/items/byId/{id}"

            };
            return await _requestProvider.GetByIdAsync<ResponseDto>(requestDto);
        }

        public async Task<ResponseDto> GetCouponsAsync()
        {
            RequestDto requestDto = new RequestDto()
            {
                URL = SD.CouponURLBase + "api/v1/coupon/items"

            };
           return await _requestProvider.GetAllAsync<ResponseDto>(requestDto);

        }

        public async Task<ResponseDto> UpdateCoupon(Models.Coupon.Coupon coupon)
        {
            RequestDto requestDto = new RequestDto()
            {
                URL = SD.CouponURLBase + $"api/v1/coupon/items/",
                Data = coupon

            };
            return await _requestProvider.PostAsync<ResponseDto>(requestDto);
        }
    }
}
