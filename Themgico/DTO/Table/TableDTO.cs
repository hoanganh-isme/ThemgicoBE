using Themgico.Entities;

namespace Themgico.DTO.Table
{
    public class TableDTO
    {
        public int Id { get; set; }
        public int? Tablenumber { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
