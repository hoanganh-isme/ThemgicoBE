using Themgico.DTO.Category;
using Themgico.Entities;

namespace Themgico.DTO.Product
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? CategoryName { get; set; }
        public string? Image { get; set; }
        public bool? Status { get; set; }

        public virtual CategoryDTO? Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
