using System;
using System.Collections.Generic;

namespace Themgico.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Payments = new HashSet<Payment>();
        }

        public string? Name { get; set; }
        public string Phone { get; set; } = null!;
        public decimal? Point { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
