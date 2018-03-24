namespace OMS.Data.Domain
{
    public enum DictionaryType:int
    {
        /// <summary>
        /// 商品类型，A,B,C类,辅料，合作商商品
        /// </summary>
        ProductType=1,
        /// <summary>
        /// 国家
        /// </summary>
        Country=2,
        /// <summary>
        /// 产区
        /// </summary>
        Area=3,
        /// <summary>
        /// 葡萄品种
        /// </summary>
        Variety=4,
        /// <summary>
        /// 容量
        /// </summary>
        capacity=5,
        /// <summary>
        /// 包装方式
        /// </summary>
        Packing=6,
        /// <summary>
        /// 渠道类型（现货、跨境、期酒）
        /// </summary>
        Channel=7,
        /// <summary>
        /// 平台
        /// </summary>
        Platform=8,
        /// <summary>
        /// 价格类型
        /// </summary>
        PriceType=9,
        /// <summary>
        /// 客户类型
        /// </summary>
        CustomerType=10,
        /// <summary>
        /// 付款类型（款到发货，货到付款...）
        /// </summary>
        PayStyle=11,
        /// <summary>
        /// 付款类型（微信，支付宝，Pos机）
        /// </summary>
        PayType = 12,
        /// <summary>
        /// 货物类型（销售品，赠品，辅料）
        /// </summary>
        GoodsType=13
    }
}
