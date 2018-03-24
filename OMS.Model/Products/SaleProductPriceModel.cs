using System;
using System.Collections.Generic;
using System.Text;

namespace OMS.Model
{
    public class SaleProductPriceModel
    {
        public int SaleProductId { get; set; }
        public int CustomerTypeId { get; set; }
        public decimal Price { get; set; }
    }
}
