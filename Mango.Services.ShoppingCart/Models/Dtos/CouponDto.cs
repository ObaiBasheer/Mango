namespace Mango.Services.ShoppingCartAPI.Models.Dtos
{
    public class CouponDto
    {
        public int couponId { get; set; }
        public string? couponCode { get; set; }
        public int discountAmount { get; set; }
        public int minAmount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime lastUpdate { get; set; }
    }
}
