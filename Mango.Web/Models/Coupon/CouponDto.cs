using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models.Coupon
{
    public class CouponDto
    {
        public class Datum
        {
            public int couponId { get; set; }
            public string? couponCode { get; set; }
            public int discountAmount { get; set; }
            public int minAmount { get; set; }
            public DateTime exeprationDate { get; set; }
            public DateTime lastUpdate { get; set; }
        }

      
            public int pageIndex { get; set; }
            public int pageSize { get; set; }
            public int count { get; set; }
            public List<Datum>? data { get; set; }
        
     
    }
}
