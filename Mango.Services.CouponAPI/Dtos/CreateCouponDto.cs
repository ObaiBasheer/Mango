namespace Mango.Services.CouponAPI.Dtos
{
    public class CreateCouponDto
    {
        public string CouponCode { get; set; } = string.Empty;
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }

        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

    }
}
