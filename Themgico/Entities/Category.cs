﻿using System;
using System.Collections.Generic;

namespace Themgico.Entities
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int CategoryId { get; set; }
        public string? CategoryDescription { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
