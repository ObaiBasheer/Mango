using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models.Coupon
{
    public class Coupon
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; } = string.Empty;

        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
        public DateTime ExeprationDate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
