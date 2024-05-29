using Mango.Web.Models;
using Mango.Web.Models.Product;
using Mango.Web.Services.RequestProvider;
using Mango.Web.Utility;

namespace Mango.Web.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IRequestProvider _baseService;
        public ProductService(IRequestProvider baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateProductsAsync(ProductDto productDto)
        {
            return await _baseService.PostAsync<ProductDto>(new RequestDto()
            {
                MethodType = SD.MethodType.POST,
                Data = productDto,
                URL = SD.ProductAPIBase + "api/products",
                ContentType = SD.ContentType.MultipartFormData
            });
        }

        public async Task<ResponseDto?> DeleteProductsAsync(int id)
        {
            return await _baseService.DeleteAsync(new RequestDto()
            {
                MethodType = SD.MethodType.DELETE,
                URL = SD.ProductAPIBase + "api/products" + id
            });
        }

        public async Task<ResponseDto?> GetAllProductsAsync()
        {
            var test = await _baseService.GetAllAsync<ResponseDto>(new RequestDto()
            {
                MethodType = SD.MethodType.GET,
                URL = SD.ProductAPIBase + "api/products"
            });
            var res = test.Result;
            return (ResponseDto)res!;
        }



        public async Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            var result = await _baseService.GetByIdAsync<ResponseDto>(new RequestDto()
            {
                MethodType = SD.MethodType.GET,
                URL = SD.ProductAPIBase + "api/products/" + id
            });
            if(result != null)
            {
                return new ResponseDto { IsSuccess = true, Message = "", Result = result.Result };

            }
            return new ResponseDto { IsSuccess = false, Message = "Failed", Result = null };
        }

        public async Task<ResponseDto?> UpdateProductsAsync(ProductDto productDto)
        {
            var result = await _baseService.PutAsync<ProductDto>(new RequestDto()
            {
                MethodType = SD.MethodType.PUT,
                Data = productDto,
                URL = SD.ProductAPIBase + "api/products",
                ContentType = SD.ContentType.MultipartFormData
            });

            if (result != null)
            {
                return new ResponseDto { IsSuccess = true, Message = "", Result = result };

            }
            return new ResponseDto { IsSuccess = false, Message = "Failed", Result = null };

        }
    }
}
