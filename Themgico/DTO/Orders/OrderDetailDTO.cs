using Themgico.DTO.Product;
using Themgico.Entities;

namespace Themgico.DTO.Orders
{
    public class OrderDetailDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public bool? Status { get; set; }
        public TimeSpan? OrderTime { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual ProductDTO Product { get; set; } = null!;
    }
}
