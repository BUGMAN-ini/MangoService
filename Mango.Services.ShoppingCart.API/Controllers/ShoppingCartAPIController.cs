using AutoMapper;
using Mango.Services.ShoppingCart.API.Data;
using Mango.Services.ShoppingCart.API.Models;
using Mango.Services.ShoppingCart.API.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace Mango.Services.ShoppingCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartAPIController : ControllerBase
    {
        private readonly ResponseDTO _response = new();
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public ShoppingCartAPIController(IMapper mapper, AppDbContext db)
        {
            _mapper = mapper;
            _db = db;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDTO?> CartUpsert(CartDto cart)
        {
            try
            {
                var CartHeaderFromDb = await _db.CartHeaders.FirstOrDefaultAsync(u => u.UserId == cart.CartHeader.UserId);
                if (CartHeaderFromDb == null)
                {
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cart.CartHeader);
                    await _db.CartHeaders.AddAsync(cartHeader);
                    await _db.SaveChangesAsync();
                    cart.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    await _db.CartDetails.AddAsync(_mapper.Map<CartDetails>(cart.CartDetails.First()));
                    await _db.SaveChangesAsync();
                }
                else 
                {
                    var cartDetailsFromDb = await _db.CartDetails.FirstOrDefaultAsync(
                        u => u.ProductId == cart.CartDetails.First().ProductId &&
                        u.CartHeaderId == CartHeaderFromDb.CartHeaderId);

                    if(cartDetailsFromDb == null)
                    {
                        cart.CartDetails.First().CartHeaderId = CartHeaderFromDb.CartHeaderId;
                        await _db.CartDetails.AddAsync(_mapper.Map<CartDetails>(cart.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        cart.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cart.CartDetails.First().CartHeaderId += cartDetailsFromDb.CartHeaderId;
                        cart.CartDetails.First().CartDetailId += cartDetailsFromDb.CartDetailId;
                        _db.CartDetails.Update(_mapper.Map<CartDetails>(cart.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                }
                _response.Result = cart;

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
