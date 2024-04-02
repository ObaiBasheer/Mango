using Mango.Web.Models;
using Mango.Web.Models.Coupon;
using Mango.Web.Services.Coupon;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static Mango.Web.Models.Coupon.CouponDto;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        public async Task<IActionResult> CouponIndex()
        {
			var responseDto = new ResponseDto();
			try
			{
                List<CouponItem> list = new();

                List<CouponItem>? response = (List<CouponItem>?)await _couponService.GetAllCouponsAsync();

				if (response != null )
				{
					list =response;
				}
				else
				{
                    TempData["error"] = "Error in get All data";
                }

				return View(list);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred: {ex.Message}");
			}
		}
		public IActionResult CouponCreate()
		{
			return View();
		}

		[HttpPost]
        public async Task<IActionResult> CouponCreate(CouponItem coupon)
        {
			if (ModelState.IsValid)
			{
				coupon.lastUpdate = DateTime.UtcNow;
				coupon.exeprationDate = DateTime.UtcNow.AddDays(3);
                ResponseDto? response = await _couponService.CreateCouponsAsync(coupon);

                if (response!.IsSuccess)
                {
                    TempData["success"] = "Coupon created successfully";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
               
            }
            return View(coupon);
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
            CouponItem? response = await _couponService.GetCouponByIdAsync(couponId);

            if (response != null)
            {
                return View(response);
            }
            else
            {
                TempData["error"] = "The coupon is not found";
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponItem couponItem)
        {
            var response = await _couponService.DeleteCouponsAsync(couponItem.couponId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response?.Message;
                return RedirectToAction(nameof(CouponIndex));
            }
            return View(couponItem);
        }

    }
}
