using Mango.Web.Exceptions;
using Mango.Web.Models;
using Mango.Web.Models.Coupon;
using Mango.Web.Services.RequestProvider;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Authentication;

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

           return await _requestProvider.DeleteAsync(new RequestDto()
            {
                MethodType = SD.MethodType.DELETE,
                URL = SD.CouponURLBase + "api/v1/coupon/items/" + id
            });
           
        }

		public async Task<ResponseDto> GetAllCouponsAsync()
		{
            try
            {
                // Make the API request to get the data
                var Item = await _requestProvider.GetAllAsync<CouponRoot>(new RequestDto()
                {
                    MethodType = SD.MethodType.GET,
                    URL = SD.CouponURLBase + "api/v1/coupon/items"
                });

                // Deserialize the response into CouponRoot object
                //CouponRoot couponRoot = JsonConvert.DeserializeObject<CouponRoot>(Convert.ToString(Item.Data)!)!;

                // Access the list of coupon items
                //List<CouponItem> couponItems = (List<CouponItem>)Item.Data!;

                // Create your desired response
                var response = new ResponseDto { IsSuccess = Item.IsSuccess, Message = Item.Message, Result = Item.Result };

                return response;
           }
            catch (ServiceAuthenticationException ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine("An error occurred: " + ex.Message);

                // Return failure response
               return new ResponseDto { IsSuccess = false, Message = ex.Message, Result = null };
            }
            catch(Exception ex)
            {
                return new ResponseDto { IsSuccess = false, Message = "Failed", Result = null };
            }


        }
        //      public async Task<IEnumerable<CouponItem>> GetCouponByCodeAsync(string couponCode)
        //{
        //	try
        //	{
        //              CouponRoot coupon = await _requestProvider.GetByCodeAsync<CouponRoot>(new RequestDto()
        //              {
        //                  MethodType = SD.MethodType.GET,
        //                  URL = SD.CouponURLBase + "api/v1/coupon/items/byCode/" + couponCode
        //              });

        //              return coupon?.Data ?? null!;
        //          }
        //	catch 
        //	{

        //              return Enumerable.Empty<CouponItem>();
        //          }

        //}
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
