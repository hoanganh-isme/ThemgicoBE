using System;
using System.Collections.Generic;

namespace Themgico.Entities
{
    public partial class Payment
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? Amount { get; set; }
        public string? Phone { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Status { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Customer? PhoneNavigation { get; set; }
    }
}
