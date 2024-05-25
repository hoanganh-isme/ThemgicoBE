using System;
using System.Collections.Generic;

namespace Themgico.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public int? Tablenumber { get; set; }
        public string? Mode { get; set; }
        public int? Numberguest { get; set; }
        public DateTime Orderdate { get; set; }
        public string? Status { get; set; }
        public decimal? Total { get; set; }

        public virtual Table? TablenumberNavigation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
