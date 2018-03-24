using System;
using System.Collections.Generic;
using System.Text;

namespace OMS.Data.Domain
{
    public  class OrderProduct:EntityBase
    {
        public int OrderId { get; set; }
        public int SaleProductId { get; set; }
        public int Quantity { get; set; }
        public decimal OrginPrice { get; set; }
        public decimal Price { get; set; }
        public decimal SumPrice { get; set; }
        public Order Order { get; set; }
        public SaleProduct SaleProduct { get; set; }
    }
}
