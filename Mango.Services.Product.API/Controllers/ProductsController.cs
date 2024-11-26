using AutoMapper;
using Mango.Services.Products.API.Data;
using Mango.Services.Products.API.Models;
using Mango.Services.Products.API.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.Products.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ResponseDTO _responseDTO;
        private readonly IMapper _mapper;

        public ProductsController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _responseDTO = new();
            _mapper = mapper;
        }

        [HttpGet("All")]
        public ResponseDTO Get()
        {
            try
            {
                IEnumerable<Product> objlist = _db.Products.ToList();
                _responseDTO.Result = _mapper.Map<IEnumerable<ProductDto>>(objlist);

            }
            catch (Exception e)
            {

                _responseDTO.IsSuccess = false;
                _responseDTO.Message = e.Message;
            }
            return _responseDTO;
        }

        [HttpGet("{id:int}")]
        public ResponseDTO Get(int id)
        {
            try
            {
                var obj = _db.Products.FirstOrDefault(x => x.ProductId == id);
                if (obj == null) _responseDTO.IsSuccess = false;

                _responseDTO.Result = _mapper.Map<Product>(obj);

            }
            catch (Exception e)
            {

                _responseDTO.IsSuccess = false;
                _responseDTO.Message = e.Message;
            }
            return _responseDTO;
        }

        [HttpGet]
        [Route("GetByName/{Name}")]
        public ResponseDTO Get(string Name)
        {
            try
            {
                var obj = _db.Products.FirstOrDefault(x => x.Name == Name);
                if (obj == null) _responseDTO.IsSuccess = false;

                _responseDTO.Result = _mapper.Map<Product>(obj);
            }
            catch (Exception e)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = e.Message;
            }

            return _responseDTO;
        }

        [HttpPost("Add")]
        //[Authorize(Roles = "ADMIN")]
        public ResponseDTO Post([FromBody] Product product)
        {
            try
            {
                var obj = _mapper.Map<Product>(product);
                _db.Products.Add(obj);
                _db.SaveChanges();
                if(product.ImageUrl != null)
                {
                    string fileName = product.ProductId + Path.GetExtension(product.ImageUrl);
                    string filePath = @"wwwroot\ProductImages\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var filestrem = new FileStream(filePathDirectory,FileMode.Create))
                    {
                        product.Image.CopyTo(filestrem);
                    }
                    var baseurl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    product.ImageUrl = baseurl + "/ProductImages" + filePath;
                    product.ImageLocalPath = filePath;
                }
                else
                {
                    product.ImageUrl = null;
                }
                _db.Products.Update(product);
                _db.SaveChanges();
                _responseDTO.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception e)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = e.Message;

            }
            return _responseDTO;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDTO DeleteById(int id)
        {
            try
            {
                Product obj = _db.Products.First(x => x.ProductId == id);
                _db.Products.Remove(obj);
                _db.SaveChanges();
                if (obj == null) _responseDTO.Result = _mapper.Map<Product>(null);
                _responseDTO.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception e)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = e.Message;

            }
            return _responseDTO;
        }

        [HttpDelete]
        [Route("GetByName/{Name}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDTO DeleteByCode(string Name)
        {
            try
            {
                Product obj = _db.Products.First(x => x.Name == Name);
                if(!string.IsNullOrEmpty(obj.ImageLocalPath))
                {
                    var oldfilepath = Path.Combine(Directory.GetCurrentDirectory(), obj.ImageLocalPath);
                    FileInfo file = new FileInfo(oldfilepath);
                    if (file.Exists)
                    {
                        file.Delete();  
                    }
                }
                _db.Products.Remove(obj);
                _db.SaveChanges();
                if (obj == null) _responseDTO.Result = _mapper.Map<ProductDto>(null);
                _responseDTO.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception e)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = e.Message;

            }
            return _responseDTO;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDTO Update([FromBody] Product product)
        {
            try
            {

                var obj = _mapper.Map<Product>(product);
                if (product.ImageUrl != null)
                {
                    string fileName = product.ProductId + Path.GetExtension(product.ImageUrl);
                    string filePath = @"wwwroot\ProductImages\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var filestrem = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        product.Image.CopyTo(filestrem);
                    }
                    var baseurl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    product.ImageUrl = baseurl + "/ProductImages" + filePath;
                    product.ImageLocalPath = filePath;
                }
                else
                {
                    product.ImageUrl = null;
                }
                _db.Products.Update(product);
                _db.SaveChanges();

                _responseDTO.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception e)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = e.Message;

            }
            return _responseDTO;
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDTO UpdateById([FromRoute] int id, [FromBody] ProductDto product)
        {
            try
            {
                var obj = _db.Products.FirstOrDefaultAsync(c => c.ProductId == id);
                var dto = _mapper.Map<Product>(obj);
                _db.Products.Update(dto);
                _db.SaveChanges();

                _responseDTO.Result = _mapper.Map<ProductDto>(dto);
            }
            catch (Exception e)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = e.Message;

            }
            return _responseDTO;
        }


    }
}
