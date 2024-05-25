using System;
using System.Collections.Generic;

namespace Themgico.Entities
{
    public partial class Table
    {
        public Table()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public int? Tablenumber { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
