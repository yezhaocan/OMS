using System;
using System.Collections.Generic;
using System.Text;

namespace OMS.Model
{
    public class SaleProductModel:ModelBase
    {
        public int Channel { get; set; }
        public int ProductId { get; set; }
        /// <summary>
        /// 锁定库存
        /// </summary>
        public int LockStock { get; set; }
        /// <summary>
        /// 总库存
        /// </summary>
        public int Stock { get; set; }
        /// <summary>
        /// 可售库存
        /// </summary>
        public int AvailableStock { get; set; }
        public List<SaleProductPriceModel> SaleProductPriceModel { get; set; }
    }
}
