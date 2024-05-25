using Themgico.DTO.Customer;
using Themgico.Entities;

namespace Themgico.DTO.Payment
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? Amount { get; set; }
        public string? Phone { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Status { get; set; }

        public virtual Order? Order { get; set; }
        public virtual CustomerDTO? PhoneNavigation { get; set; }
    }
}
