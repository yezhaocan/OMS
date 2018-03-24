using System;
using System.Collections.Generic;
using System.Text;

namespace OMS.Model
{
    public  class OrderProductModel:ModelBase
    {
        public int OrderId { get; set; }
        public int SaleProductId { get; set; }
        public int Quantity { get; set; }
        public decimal OrginPrice { get; set; }
        public decimal Price { get; set; }
        public decimal SumPrice { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public SaleProductModel SaleProductModel { get; set; }
    }
}
