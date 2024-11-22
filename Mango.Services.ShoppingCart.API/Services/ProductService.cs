using Mango.Services.ShoppingCart.API.Models.Dto;
using Mango.Services.ShoppingCart.API.Services.IServices;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCart.API.Services
{
    public class ProductService : IProductService
    {
        private IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/api/Products");
            var apicontext = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDTO>(apicontext);
            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(resp.Result));
            }
            return new List<ProductDto>();
        }
    }
}
