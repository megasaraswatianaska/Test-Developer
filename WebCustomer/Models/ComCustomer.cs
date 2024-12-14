using System;
using System.Collections.Generic;

namespace WebCustomer.Models
{
    public  class ComCustomer
    {
        public ComCustomer()
        {
            SoOrders = new HashSet<SoOrder>();
        }

        public int ComCustomerId { get; set; }
        public string? CustomerName { get; set; }

        public virtual ICollection<SoOrder> SoOrders { get; set; }
    }
}
