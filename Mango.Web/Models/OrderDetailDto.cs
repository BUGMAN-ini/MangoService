using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Web.Models
{
    public class OrderDetailDto
    {
        public int OrderDetailId { get; set; }
        public int OrderHeaderId { get; set; }
        public int ProductId { get; set; }
        public ProductDTO? Product { get; set; }
        public int Count { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
    }
}
