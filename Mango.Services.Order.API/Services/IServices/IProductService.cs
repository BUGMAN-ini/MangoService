using Mango.Services.Order.API.Models.Dto;

namespace Mango.Services.Order.API.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
    }
}
