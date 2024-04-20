using Mango.Web.Models.Coupon;

namespace Mango.Web.Models
{
    public class CouponRoot
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long Count { get; set; }

        public IEnumerable<CouponItem> Data { get; set; }

      
    }
}
