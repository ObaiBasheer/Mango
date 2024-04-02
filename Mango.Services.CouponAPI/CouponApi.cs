using Mango.Services.CouponAPI.Dtos;
using Mango.Services.CouponAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            builder.MapPut("/items", UpdateItem);
            builder.MapPost("/items", CreateItem);
            builder.MapDelete("/items/{id:int}", DeleteItemById);

            return builder;
        }

        public static async Task<Results<Ok<PaginatedItems<CouponDto>>, BadRequest<string>>> GetAllItems(
            [AsParameters] PaginationRequest paginationRequest,
            [AsParameters] CouponServices services
            )
        {
            var pageSize = paginationRequest.PageSize;
            var pageIndex = paginationRequest.PageIndex;

            var totalItems = await services.Context.coupons.LongCountAsync();

            var itemsOnPage = await services.Context.coupons
                                .OrderBy(x=>x.CouponCode)
                                .Skip(pageSize * pageIndex)
                                .Take(pageSize)
                                .ToListAsync();

            var itemsDto = services.mapper.Map<List<CouponDto>>(itemsOnPage);
           

            return TypedResults.Ok(new PaginatedItems<CouponDto>(pageIndex, pageSize, totalItems, itemsDto));
        }

        public static async Task<Results<Ok<CouponDto>, NotFound, BadRequest<string>>> GetItemById([AsParameters] CouponServices services, int Id)
        {
            if(Id <= 0)
            {
                return TypedResults.BadRequest("Id is not valid.");
            }

            var coupon = await services.Context.coupons.SingleOrDefaultAsync(c=>c.CouponId == Id);

            if(coupon == null)
            {
                return TypedResults.NotFound();
            }


            return TypedResults.Ok(services.mapper.Map<CouponDto>(coupon));

        }

        public static async Task<Ok<PaginatedItems<CouponDto>>> GetItemByCode([AsParameters] CouponServices services, [AsParameters] PaginationRequest paginationRequest , string code)
        {
            var pageIndex = paginationRequest.PageIndex;
            var pageSize = paginationRequest.PageSize;

            var totalItem = await services.Context.coupons.Where(c=>c.CouponCode.StartsWith(code)).LongCountAsync();

            var itemsOnPage =  await services.Context.coupons.Where(c=>c.CouponCode.StartsWith(code))
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var dto = services.mapper.Map<List<CouponDto>>(itemsOnPage);

            return TypedResults.Ok(new PaginatedItems<CouponDto>(pageIndex, pageSize, totalItem, dto));
        }

        public static async Task<Created> CreateItem([AsParameters] CouponServices services,[FromBody] CreateCouponDto createCoupon)
        {
            try
            {
                var item = new Coupon
                {
                    CouponCode = createCoupon.CouponCode,
                    MinAmount = createCoupon.MinAmount,
                    DiscountAmount = createCoupon.DiscountAmount,
                    ExeprationDate = createCoupon.ExeprationDate,
                    LastUpdate = createCoupon.LastUpdate,
                };

                await services.Context.coupons.AddAsync(item);
                await services.Context.SaveChangesAsync();
                return TypedResults.Created($"/api/v1/coupon/items/{item.CouponId}");
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
           


        }

        public static async Task<Results<Created, NotFound<string>>> UpdateItem([AsParameters]CouponServices services, UpdateCouponDto updateCoupon)
        {
            var couponItem = await services.Context.coupons.SingleOrDefaultAsync(x => x.CouponId == updateCoupon.CouponId);

            if(couponItem == null )
            {
                return TypedResults.NotFound($"Item with id {updateCoupon.CouponId} not found.");
            }

            var trackItem = services.Context.coupons.Entry(couponItem);

            // Update current product
            trackItem.CurrentValues.SetValues(trackItem);

            await services.Context.SaveChangesAsync();

            return TypedResults.Created($"/api/v1/coupon/items/{updateCoupon.CouponId}");
        }

        public static async Task<Results<NoContent, NotFound>> DeleteItemById([AsParameters] CouponServices services, int Id)
        {
            var itemToDelete = services.Context.coupons.SingleOrDefault(c=>c.CouponId == Id);

            if(itemToDelete == null )
            {
              return TypedResults.NotFound();
            }

            services.Context.coupons.Remove(itemToDelete);
            await services.Context.SaveChangesAsync();
            return TypedResults.NoContent();
        }
    }
}
