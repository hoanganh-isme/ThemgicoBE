using Themgico.DTO.Payment;
using Themgico.DTO.Table;
using Themgico.Entities;

namespace Themgico.DTO.Orders
{
    public class OrdersDTO
    {

        public int Id { get; set; }
        public int? TableId { get; set; }
        public string? Mode { get; set; }
        public DateTime? Orderdate { get; set; }
        public string? Status { get; set; }
        public decimal? Total { get; set; }

        public virtual TableDTO? Table { get; set; }
        public virtual ICollection<OrderDetailDTO> OrderDetails { get; set; }
        public virtual ICollection<PaymentDTO> Payments { get; set; }
    }
}
