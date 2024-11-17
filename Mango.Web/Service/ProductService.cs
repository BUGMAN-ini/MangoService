using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseservice;

        public ProductService(IBaseService baseservice)
        {
            _baseservice = baseservice;
        }

        public async Task<ResponseDTO?> CreateProductAsync(ProductDTO couponDto)
        {
            return await _baseservice.SendAsync(
                new RequestDTO()
                {
                    ApiType = SD.ApiType.GET,
                    Url = SD.ProductAPIBase + "/api/Products"
                });
        }

        public async Task<ResponseDTO?> DeleteProductAsync(int id)
        {
            return await _baseservice.SendAsync(
                new RequestDTO()
                {
                    ApiType = SD.ApiType.GET,
                    Url = SD.ProductAPIBase + "/api/Products"
                });
        }

        public async Task<ResponseDTO?> GetAllProductsAsync()
        {
            return await _baseservice.SendAsync(
                new RequestDTO()
                {
                    ApiType = SD.ApiType.GET,
                    Url = SD.ProductAPIBase + "/api/Products"
                });
        }

        public async Task<ResponseDTO?> GetProductAsync(string couponId)
        {
            return await _baseservice.SendAsync(
                new RequestDTO()
                {
                    ApiType = SD.ApiType.GET,
                    Url = SD.ProductAPIBase + "/api/Products"
                });
        }

        public async Task<ResponseDTO?> GetProductByIdAsync(int couponId)
        {
            return await _baseservice.SendAsync(
                 new RequestDTO()
                 {
                     ApiType = SD.ApiType.GET,
                     Url = SD.ProductAPIBase + "/api/Products"
                 });
        }

        public async Task<ResponseDTO?> UpdateProductAsync(ProductDTO couponDto)
        {
            return await _baseservice.SendAsync(
                new RequestDTO()
                {
                    ApiType = SD.ApiType.GET,
                    Url = SD.ProductAPIBase + "/api/Products"
                });
        }
    }
}
