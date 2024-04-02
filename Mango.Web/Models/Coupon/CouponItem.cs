using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models.Coupon
{
    public class CouponItem
    {

        public int couponId { get; set; }
        public string? couponCode { get; set; }
        public int discountAmount { get; set; }
        public int minAmount { get; set; }
        public DateTime exeprationDate { get; set; }
        public DateTime lastUpdate { get; set; }


    }
}
