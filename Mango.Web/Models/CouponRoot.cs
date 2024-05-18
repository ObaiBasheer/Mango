using Mango.Web.Models.Coupon;

namespace Mango.Web.Models
{
    public class Datum
    {
        public int couponId { get; set; }
        public string couponCode { get; set; }
        public int discountAmount { get; set; }
        public int minAmount { get; set; }
        public DateTime exeprationDate { get; set; }
        public DateTime lastUpdate { get; set; }
    }

    public class CouponRoot
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int count { get; set; }
        public IEnumerable<CouponItem>? Data { get; set; }
    }
}
