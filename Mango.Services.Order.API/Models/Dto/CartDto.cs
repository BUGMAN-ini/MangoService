﻿namespace Mango.Services.Order.API.Models.Dto
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; }
        public IEnumerable<CartDetailDto>? CartDetails { get; set; }
    }
}
