using System;
using System.Collections.Generic;

namespace WebCustomer.Models
{
    public  class SoItem
    {
        public long? SoItemId { get; set; }
        public long SoOrderId { get; set; }
        public string ItemName { get; set; } = null!;
        public int Quantity { get; set; }
        public double Price { get; set; }


        public virtual SoOrder SoOrder { get; set; } = null!;
    }
}
