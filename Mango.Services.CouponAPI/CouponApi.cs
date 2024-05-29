using Mango.Services.CouponAPI.Dtos;
using Mango.Services.CouponAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FluentValidation;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mango.Services.CouponAPI
{
    public static class CouponApi
    {
        public static IEndpointRouteBuilder MapCouponAPI(this IEndpointRouteBuilder builder)
        {
            // Routes for querying catalog items.
            builder.MapGet("/items", GetAllItems);
            builder.MapGet("/items/byId/{id:int}", GetItemById);
            builder.MapGet("/items/byCode/{code}", GetItemByCode);

            // Routes for modifying catalog items.
            builder.MapPut("/items", UpdateItem).RequireAuthorization("AdminPolicy");
            builder.MapPost("/items", CreateItem).RequireAuthorization("AdminPolicy");
            builder.MapDelete("/items/{id:int}", DeleteItemById).RequireAuthorization("AdminPolicy");

            return builder;
        }

        public static async Task<Results<Ok<PaginatedItems<CouponDto>>, BadRequest<string>>> GetAllItems(
            [AsParameters] PaginationRequest paginationRequest,
            [AsParameters] CouponServices services
            )
        {
            try
            {
                var pageSize = paginationRequest.PageSize;
                var pageIndex = paginationRequest.PageIndex;

                if (pageSize <= 0 || pageIndex < 0)
                {
                    return TypedResults.BadRequest("Invalid pagination parameters.");
                }

                var totalItems = await services.Context.coupons.LongCountAsync();

                var itemsOnPage = await services.Context.coupons
                                    .OrderBy(x => x.CouponCode)
                                    .Skip(pageSize * pageIndex)
                                    .Take(pageSize)
                                    .ToListAsync();

                var itemsDto = services.mapper.Map<List<CouponDto>>(itemsOnPage);

                return TypedResults.Ok(new PaginatedItems<CouponDto>(pageIndex, pageSize, totalItems, itemsDto));
            }
            catch (Exception ex)
            {
                //logger.LogError(ex, "Error fetching items.");
                return TypedResults.BadRequest("An error occurred while fetching items.");
            }
        }

        public static async Task<Results<Ok<CouponDto>, NotFound, BadRequest<string>>> GetItemById(
            [AsParameters] CouponServices services, int id)
        {
            try
            {
                if (id <= 0)
                {
                    return TypedResults.BadRequest("Id is not valid.");
                }

                var coupon = await services.Context.coupons.SingleOrDefaultAsync(c => c.CouponId == id);

                if (coupon == null)
                {
                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(services.mapper.Map<CouponDto>(coupon));
            }
            catch (Exception ex)
            {
                //logger.LogError(ex, "Error fetching item by Id.");
                return TypedResults.BadRequest("An error occurred while fetching item by Id.");
            }
        }

        public static async Task<Results<Ok<PaginatedItems<CouponDto>>, BadRequest<string>>> GetItemByCode(
            [AsParameters] CouponServices services, [AsParameters] PaginationRequest paginationRequest, string code)
        {
            try
            {
                var pageIndex = paginationRequest.PageIndex;
                var pageSize = paginationRequest.PageSize;

                if (pageSize <= 0 || pageIndex < 0)
                {
                    return TypedResults.BadRequest("Invalid pagination parameters.");
                }

                var totalItems = await services.Context.coupons
                                    .Where(c => c.CouponCode.StartsWith(code))
                                    .LongCountAsync();

                var itemsOnPage = await services.Context.coupons
                                    .Where(c => c.CouponCode.StartsWith(code))
                                    .Skip(pageSize * pageIndex)
                                    .Take(pageSize)
                                    .ToListAsync();

                var itemsDto = services.mapper.Map<List<CouponDto>>(itemsOnPage);

                return TypedResults.Ok(new PaginatedItems<CouponDto>(pageIndex, pageSize, totalItems, itemsDto));
            }
            catch (Exception ex)
            {
                //logger.LogError(ex, "Error fetching items by code.");
                return TypedResults.BadRequest("An error occurred while fetching items by code.");
            }
        }

        [Authorize(Roles = "ADMIN")]
        public static async Task<Results<Created, BadRequest<string>>> CreateItem(
            [AsParameters] CouponServices services, [FromBody] CreateCouponDto createCoupon)
        {
            try
            {
                var validator = new CreateCouponDtoValidator();
                var validationResult = await validator.ValidateAsync(createCoupon);

                if (!validationResult.IsValid)
                {
                    return TypedResults.BadRequest(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                }

                var item = new Coupon
                {
                    CouponCode = createCoupon.CouponCode,
                    MinAmount = createCoupon.MinAmount,
                    DiscountAmount = createCoupon.DiscountAmount,
                    ExeprationDate = createCoupon.ExeprationDate,
                    LastUpdate = DateTime.UtcNow
                };

                await services.Context.coupons.AddAsync(item);
                await services.Context.SaveChangesAsync();
                return TypedResults.Created($"/api/v1/coupon/items/{item.CouponId}");
            }
            catch (Exception ex)
            {
                //logger.LogError(ex, "Error creating item.");
                return TypedResults.BadRequest("An error occurred while creating the item.");
            }
        }

        [Authorize(Roles = "ADMIN")]
        public static async Task<Results<Created, NotFound<string>, BadRequest<string>>> UpdateItem(
            [AsParameters] CouponServices services, [FromBody] UpdateCouponDto updateCoupon)
        {
            try
            {
                var validator = new UpdateCouponDtoValidator();
                var validationResult = await validator.ValidateAsync(updateCoupon);

                if (!validationResult.IsValid)
                {
                    return TypedResults.BadRequest(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                }

                var couponItem = await services.Context.coupons.SingleOrDefaultAsync(x => x.CouponId == updateCoupon.CouponId);

                if (couponItem == null)
                {
                    return TypedResults.NotFound($"Item with id {updateCoupon.CouponId} not found.");
                }

                services.Context.Entry(couponItem).CurrentValues.SetValues(updateCoupon);
                await services.Context.SaveChangesAsync();

                return TypedResults.Created($"/api/v1/coupon/items/{updateCoupon.CouponId}");
            }
            catch (Exception ex)
            {
                //logger.LogError(ex, "Error updating item.");
                return TypedResults.BadRequest("An error occurred while updating the item.");
            }
        }

        [Authorize(Roles = "ADMIN")]
        public static async Task<Results<NoContent, NotFound, BadRequest<string>>> DeleteItemById(
            [AsParameters] CouponServices services, int id)
        {
            try
            {
                var itemToDelete = await services.Context.coupons.SingleOrDefaultAsync(c => c.CouponId == id);

                if (itemToDelete == null)
                {
                    return TypedResults.NotFound();
                }

                services.Context.coupons.Remove(itemToDelete);
                await services.Context.SaveChangesAsync();
                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                //logger.LogError(ex, "Error deleting item.");
                return TypedResults.BadRequest("An error occurred while deleting the item.");
            }
        }
    }

}