using System.ComponentModel.DataAnnotations;

namespace Mango.Services.CouponAPI.Dtos
{
    public class UpdateCouponDto
    {

        public int CouponId { get; set; }
        public string? CouponCode { get; set; } 

        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
