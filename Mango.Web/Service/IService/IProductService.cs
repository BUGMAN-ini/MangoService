using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDTO?> GetAllProductsAsync();
        Task<ResponseDTO?> GetProductAsync(string couponId);
        Task<ResponseDTO?> GetProductByIdAsync(int couponId);
        Task<ResponseDTO?> CreateProductAsync(ProductDTO couponDto);
        Task<ResponseDTO?> UpdateProductAsync(ProductDTO couponDto);
        Task<ResponseDTO?> DeleteProductAsync(int id);
    }
}
