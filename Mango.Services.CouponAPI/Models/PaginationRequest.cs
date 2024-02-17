namespace Mango.Services.CouponAPI.Models
{
    public record PaginationRequest(int PageSize = 10, int PageIndex = 0);
}
