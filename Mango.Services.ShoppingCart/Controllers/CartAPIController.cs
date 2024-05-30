using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dtos;
using Mango.Services.ShoppingCartAPI.Services.Coupon;
using Mango.Services.ShoppingCartAPI.Services.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly AppDbContext _DbContext;
        private ResponseDto _response;
        private IMapper _mapper;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;

        public CartAPIController(AppDbContext DbContext, IMapper mapper, ResponseDto response, IProductService productService, ICouponService couponService)
        {
            _DbContext = DbContext;
            _mapper = mapper;
            _response = response;
            _productService = productService;
            _couponService = couponService;
        }
        [HttpPost]
        public async Task<ResponseDto> CartUpsert(CartDto cart)
        {
            try
            {
                //check if there a cart for this user or not
                var existCartHeader = _DbContext.CartHeaders.AsNoTracking().FirstOrDefault(u => u.UserId == cart.CartHeader!.UserId);
                if (existCartHeader != null)
                {
                    //the cart already exist and he want to add new Item or update existing one
                    //check if the cart has product
                    var existingCartDetails = _DbContext.CartDetails.AsNoTracking().FirstOrDefault(p => p.ProductId == cart.CartDetails!.First().ProductId && p.CartHeaderId == existCartHeader.CartHeaderId);

                    if (existingCartDetails == null)
                    {
                        //create a new cartDetails
                        cart.CartDetails!.First().CartHeaderId = existCartHeader.CartHeaderId;
                        await _DbContext.CartDetails.AddAsync(_mapper.Map<CartDetails>(cart.CartDetails!.First()));
                        await _DbContext.SaveChangesAsync();

                    }
                    else
                    {
                        //update cart Details quantity
                        cart.CartDetails!.First().Count += existingCartDetails.Count;
                        cart.CartDetails!.First().CartDetailsId = existingCartDetails.CartDetailsId;
                        cart.CartDetails!.First().CartHeaderId = existCartHeader.CartHeaderId;

                        _DbContext.CartDetails.Update(_mapper.Map<CartDetails>(cart.CartDetails!.First()));
                        await _DbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    //create a new cart for this user to put his Item in
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cart.CartHeader);
                    await _DbContext.CartHeaders.AddAsync(cartHeader);

                    //need To save the cart header to populate the id To cart details
                    await _DbContext.SaveChangesAsync();

                    //Add crat header Id to cart details
                    cart.CartDetails!.First().CartHeaderId = cartHeader.CartHeaderId;
                    await _DbContext.CartDetails.AddAsync(_mapper.Map<CartDetails>(cart.CartDetails!.First()));
                    await _DbContext.SaveChangesAsync();
                }

                _response.Result = cart;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }
        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CartDto cartDto = new CartDto() {
                    CartHeader = _mapper.Map<CartHeaderDto>(_DbContext.CartHeaders.FirstOrDefault(x=>x.UserId == userId))
                };
                cartDto.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(_DbContext.CartDetails.Where(x => x.CartHeaderId == cartDto.CartHeader.CartHeaderId));
                var products = await _productService.GetProductsAsync();
                foreach (var item in cartDto.CartDetails)
                {
                    item.Product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
                    cartDto.CartHeader.CartTotal += (item.Count * item.Product!.Price);
                }

                if(!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
                {
                    CouponDto coupon =await _couponService.CouponByCode(cartDto.CartHeader.CouponCode);
                    if(coupon != null && cartDto.CartHeader.CartTotal > coupon.minAmount)
                    {
                        cartDto.CartHeader.CartTotal -= coupon.minAmount;
                        cartDto.CartHeader.Discount = coupon.discountAmount;
                    }
                }
                _response.Result = cartDto;
                _response.IsSuccess = true;

            }
            catch (Exception ex)
            {

                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = _DbContext.CartHeaders.FirstOrDefault(x => x.UserId == cartDto.CartHeader!.UserId);
                cartHeaderFromDb!.CouponCode = cartDto.CartHeader!.CouponCode;
                _DbContext.CartHeaders.Update(cartHeaderFromDb);
                await _DbContext.SaveChangesAsync();
                _response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }

            return _response;
        }

       
        [HttpDelete]
        public async Task<ResponseDto> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                var cartDetails = _DbContext.CartDetails.FirstOrDefault(x=>x.CartDetailsId == cartDetailsId);
                var totalCountOfCartItem = _DbContext.CartDetails.Where(x=>x.CartHeaderId == cartDetails!.CartHeaderId).Count();
                _DbContext.CartDetails.Remove(cartDetails!);

                if(totalCountOfCartItem == 1) 
                {
                    var cartHeaderToRemove = await _DbContext.CartHeaders.FirstOrDefaultAsync(x => x.CartHeaderId == cartDetails!.CartHeaderId);
                    _DbContext.CartHeaders.Remove(cartHeaderToRemove!);

                }

            
                _DbContext.SaveChanges();
                _response.IsSuccess = true;

            }
            catch (Exception ex)
            {

                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }
    }
}
