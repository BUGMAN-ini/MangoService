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
                    ApiType = SD.ApiType.POST,
                    Url = SD.ProductAPIBase + "/api/Products/Add"
                });
        }

        public async Task<ResponseDTO?> DeleteProductAsync(int id)
        {
            return await _baseservice.SendAsync(
                new RequestDTO()
                {
                    ApiType = SD.ApiType.DELETE,
                    Url = SD.ProductAPIBase + "/api/Products" + id
                });
        }

        public async Task<ResponseDTO?> GetAllProductsAsync()
        {
            return await _baseservice.SendAsync(
                new RequestDTO()
                {
                    ApiType = SD.ApiType.GET,
                    Url = SD.ProductAPIBase + "/api/Products/All"
                });
        }

        public async Task<ResponseDTO?> GetProductAsync(int id)
        {
            return await _baseservice.SendAsync(
                new RequestDTO()
                {
                    ApiType = SD.ApiType.GET,
                    Url = SD.ProductAPIBase + "/api/Products" + id
                });
        }

        public async Task<ResponseDTO?> GetProductByIdAsync(int prodId)
        {
            return await _baseservice.SendAsync(
                 new RequestDTO()
                 {
                     ApiType = SD.ApiType.GET,
                     Url = SD.ProductAPIBase + "/api/Products"+ prodId
                 });
        }

        public async Task<ResponseDTO?> UpdateProductAsync(ProductDTO product)
        {
            return await _baseservice.SendAsync(
                new RequestDTO()
                {
                    ApiType = SD.ApiType.PUT,
                    Url = SD.ProductAPIBase + "/api/Products" + product
                });
        }
    }
}
