﻿namespace Mango.Services.CouponAPI.Dtos
{
    public class CouponDto
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; } = string.Empty;
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
        public DateTime ExeprationDate { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
