using AutoMapper;
using Mango.Services.Coupon.API.Data;
using Mango.Services.Coupon.API.Models.DTO;
using Mango.Services.Coupon.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace Mango.Services.Coupon.API.Controllers
{
    [Route("api/Coupon")]
    [ApiController]
    [Authorize]
    public class CouponsApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;

        public CouponsApiController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _response = new ResponseDTO();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDTO Get()
        {
            try
            {
                IEnumerable<Coupons> objlist = _db.Coupons.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDTO>>(objlist);

            }
            catch (Exception e)
            {

                _response.IsSuccess = false;
                _response.Message = e.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDTO GetById(int id)
        {
            try
            {
                Models.Coupons obj = _db.Coupons.First(x => x.CouponId == id);
                _response.Result = _mapper.Map<CouponDTO>(obj);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;

            }
            return _response;
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDTO GetByCode(string code)
        {
            try
            {
                Models.Coupons obj = _db.Coupons.FirstOrDefault(o => o.CouponCode.ToLower() == code.ToLower());
                if (obj == null) _response.IsSuccess = false;

                _response.Result = _mapper.Map<CouponDTO>(obj);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;

            }
            return _response;
        }

        [HttpPost]
        public ResponseDTO Post([FromBody] PostCouponDTO coupon)
        {
            try
            {
                var obj = _mapper.Map<Coupons>(coupon);
                _db.Coupons.Add(obj);
                _db.SaveChanges();
                if (obj == null) _response.IsSuccess = false;

                _response.Result = _mapper.Map<CouponDTO>(obj);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;

            }
            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ResponseDTO DeleteById(int id)
        {
            try
            {
                Coupons obj = _db.Coupons.First(x => x.CouponId == id);
                _db.Coupons.Remove(obj);
                _db.SaveChanges();
                if (obj == null) _response.Result = _mapper.Map<CouponDTO>(null);
                _response.Result = _mapper.Map<CouponDTO>(obj);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;

            }
            return _response;
        }

        [HttpDelete]
        [Route("GetByCode/{code}")]
        public ResponseDTO DeleteByCode(string code)
        {
            try
            {
                Coupons obj = _db.Coupons.First(x => x.CouponCode == code);
                _db.Coupons.Remove(obj);
                _db.SaveChanges();
                if (obj == null) _response.Result = _mapper.Map<CouponDTO>(null);
                _response.Result = _mapper.Map<CouponDTO>(obj);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;

            }
            return _response;
        }

        [HttpPut]
        public ResponseDTO Update([FromBody] CouponDTO coupon)
        {
            try
            {

                var obj = _mapper.Map<Coupons>(coupon);
                _db.Coupons.Update(obj);
                _db.SaveChanges();

                _response.Result = _mapper.Map<CouponDTO>(obj);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;

            }
            return _response;
        }

        [HttpPut]
        [Route("{id:int}")]
        public ResponseDTO UpdateById([FromRoute] int id, [FromBody] CouponDTO coupon)
        {
            try
            {
                var obj = _db.Coupons.FirstOrDefaultAsync(c => c.CouponId == id);
                var dto = _mapper.Map<Coupons>(obj);
                _db.Coupons.Update(dto);
                _db.SaveChanges();

                _response.Result = _mapper.Map<CouponDTO>(dto);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;

            }
            return _response;
        }

        // To Test My Node JS API
        [HttpGet]
        [Route("random")]
        public async Task<IActionResult> GetJoke()
        {
            string apiUrl = "http://localhost:3000/joke";
            try
            {
                HttpClient httpClient = new HttpClient();
                var response = await httpClient.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Failed to fetch the joke.");
                }

               
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var joke = JsonSerializer.Deserialize<object>(jsonResponse);

                return Ok(joke);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error calling the external API: {ex.Message}");
            }
        }

    }
}
