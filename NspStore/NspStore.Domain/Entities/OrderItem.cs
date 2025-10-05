﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NspStore.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
