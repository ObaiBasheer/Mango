using Mango.Web.Models;
using Mango.Web.Models.Coupon;
using Mango.Web.Services.Coupon;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
			try
			{
                CouponRoot root = new();
                List<CouponItem> list = new();

               var  response = await _couponService.GetAllCouponsAsync();

                if (response.Equals != null && response.Result != null )
				{

                    root = (CouponRoot)response.Result;
                    list = (List<CouponItem>)root.Data!;

                }
				else
				{
                    TempData["error"] = response.Message;
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
                TempData["success"] = "The Record Is Deleted";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = response?.Message;

            }
            return View(couponItem);
        }

    }
}
