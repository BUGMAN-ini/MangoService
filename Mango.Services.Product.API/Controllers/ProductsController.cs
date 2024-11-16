using AutoMapper;
using Azure;
using Mango.Services.Coupon.API.Models.DTO;
using Mango.Services.Product.API.Data;
using Mango.Services.Product.API.ProductMain;
using Mango.Services.Product.API.ProductMain.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.Products.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ResponseDTO _responseDTO;
        private readonly IMapper _mapper;

        public ProductsController(AppDbContext db, ResponseDTO responseDTO, IMapper mapper)
        {
            _db = db;
            _responseDTO = responseDTO;
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDTO Get()
        {
            try
            {
                IEnumerable<Product> objlist = _db.Products.ToList();
                _responseDTO.Result = _mapper.Map<IEnumerable<ProductDto>>(objlist);

            }
            catch (Exception e)
            {

                _response.IsSuccess = false;
                _response.Message = e.Message;
            }
            return _response;
        }



    }
}
