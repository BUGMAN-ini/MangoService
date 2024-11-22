using AutoMapper;
using Mango.Services.ShoppingCart.API.Data;
using Mango.Services.ShoppingCart.API.Models;
using Mango.Services.ShoppingCart.API.Models.Dto;
using Mango.Services.ShoppingCart.API.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;

        public ShoppingCartAPIController(IMapper mapper, AppDbContext db, IProductService productService)
        {
            _mapper = mapper;
            _db = db;
            _productService = productService;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDTO?> CartUpsert(CartDto cart)
        {
            try
            {
                var CartHeaderFromDb = await _db.CartHeaders.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == cart.CartHeader.UserId);
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
                    var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
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
                        cart.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cart.CartDetails.First().CartDetailId = cartDetailsFromDb.CartDetailId;
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

        [HttpDelete("Remove")]
        public async Task<ResponseDTO?> RemoveCart(int cartDetailsId)
        {
            try
            {
                CartDetails CartHeaderFromDb =  _db.CartDetails.First(u => u.CartDetailId == cartDetailsId);

                int TotalCountOfCartItem = _db.CartDetails.Where(u => u.CartHeaderId == CartHeaderFromDb.CartHeaderId).Count();
                _db.CartDetails.Remove(CartHeaderFromDb);

                if (TotalCountOfCartItem == 1)
                {
                    var cartheaderToRemove = await  _db.CartHeaders
                            .FirstOrDefaultAsync(u => u.CartHeaderId == CartHeaderFromDb.CartHeaderId);
                   
                    _db.CartHeaders.Remove(cartheaderToRemove);
                }
                await _db.SaveChangesAsync();
                _response.Result = true;

            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDTO?> GetCartByUserId(string userid)
        {

            try
            {
                CartDto cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(_db.CartHeaders.First(u=> u.UserId == userid)),
                };

                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailDto>>(_db.CartDetails.
                    Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId));

                IEnumerable<ProductDto> products = await _productService.GetProducts();

                foreach(var item in cart.CartDetails)
                {
                    cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }

                if(!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    CouponDTO coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);
                    if(coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.Discount = coupon.DiscountAmount;
                    }
                }

                _response.Result = cart;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response; ;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartdto)
        {
            try
            {
                var cart = await _db.CartHeaders.FirstAsync(u => u.UserId == cartdto.CartHeader.UserId);
                cart.CouponCode = cartdto.CartHeader.CouponCode;
                _db.CartHeaders.Update(cart);
                await _db.SaveChangesAsync();
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.ToString();
                _response.IsSuccess = false;
            }

            return _response;
        }
    }
}
