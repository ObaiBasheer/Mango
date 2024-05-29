﻿using FluentValidation;

namespace Mango.Services.CouponAPI.Dtos
{
    public class UpdateCouponDtoValidator : AbstractValidator<UpdateCouponDto>
    {
        public UpdateCouponDtoValidator()
        {
            RuleFor(x => x.CouponId).GreaterThan(0).WithMessage("Coupon ID is required.");
            RuleFor(x => x.CouponCode).NotEmpty().WithMessage("Coupon code is required.");
            RuleFor(x => x.MinAmount).GreaterThan(0).WithMessage("Minimum amount must be greater than zero.");
            RuleFor(x => x.DiscountAmount).GreaterThan(0).WithMessage("Discount amount must be greater than zero.");
            RuleFor(x => x.ExeprationDate).NotEmpty().WithMessage("Expiration date is required.");
        }
    }
}
