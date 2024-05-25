using System;
using System.Collections.Generic;

namespace Themgico.Entities
{
    public partial class News
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Author { get; set; }
        public string? Image { get; set; }
        public bool? Status { get; set; }
    }
}
