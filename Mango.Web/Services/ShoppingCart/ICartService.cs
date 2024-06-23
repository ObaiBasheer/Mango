using Mango.Web.Models;
using Mango.Web.Models.Product;
using Mango.Web.Models.ShoppingCart;

namespace Mango.Web.Services.ShoppingCart
{
    public interface ICartService
    {
        Task<ResponseDto?> GetCartByUserId(string userId);
        Task<ResponseDto?> UpsertCartAsync(CartDto cartDto);
        Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId);
        Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto);
    }
}
