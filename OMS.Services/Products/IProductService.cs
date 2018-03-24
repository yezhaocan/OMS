using OMS.Core;
using OMS.Data.Domain;
using OMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMS.Services.Products
{
    public interface IProductService
    {
        Product GetProductById(int id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="channelId">默认94,现货渠道</param>
        /// <param name="priceTypeId">默认103，标准价</param>
        /// <returns></returns>
        SaleProduct GetSaleProduct(int productId, int channelId = 94, int priceTypeId = 103);
        void InsertProduct(Product product);
        void UpdateProduct(Product product);
        bool GetProductByName(string name);
        bool DelProduct(int id);
        IQueryable GetAllProducts();
        PageList<Product> GetProductList(int pageSize, int pageIndex, int TypeId = 0,string searchStr="");
    }
}
