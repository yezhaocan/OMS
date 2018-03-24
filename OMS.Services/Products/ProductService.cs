using Microsoft.EntityFrameworkCore;
using OMS.Core;
using OMS.Data.Domain;
using OMS.Data.Interface;
using OMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMS.Services.Products
{
    public class ProductService : ServiceBase, IProductService
    {
        #region ctor
        public ProductService(IDbAccessor omsAccessor, IWorkContext workContext)
            : base(omsAccessor, workContext)
        {

        }
        #endregion

        public Product GetProductById(int id)
        {
            if (id == 0)
                return null;
            var pro = _omsAccessor.GetById<Product>(id);
            return pro;
        }
        public SaleProduct GetSaleProduct(int productId, int channelId = 94, int priceTypeId = 104)
        {
            var saleProduct = _omsAccessor.Get<SaleProduct>().Where(p => p.ProductId == productId && p.Channel == channelId && p.Isvalid)
                .Include(p => p.SaleProductPrice).FirstOrDefault();
            //筛选价格类型
            if (saleProduct != null && saleProduct.SaleProductPrice != null)
            {
                saleProduct.SaleProductPrice = saleProduct.SaleProductPrice.Where(p => p.CustomerTypeId == priceTypeId && p.Isvalid).ToList();
            }
            return saleProduct;
        }
        public void InsertProduct(Product product)
        {
            if (product == null)
                throw new ArgumentException("Product");
            product.CreatedBy = _workContext.CurrentUser.Id;
            _omsAccessor.Insert(product);
            _omsAccessor.SaveChanges();
        }
        public void UpdateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentException("Product");

            product.ModifiedBy = _workContext.CurrentUser.Id;
            product.ModifiedTime = DateTime.Now;
            _omsAccessor.Update(product);
            _omsAccessor.SaveChanges();

        }
        public bool GetProductByName(string name)
        {
            if (name != null)
            {
                _omsAccessor.Get<Product>().Where(p => p.Isvalid && p.Name == name).FirstOrDefault();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DelProduct(int id)
        {
            Product product = _omsAccessor.Get<Product>().Where(p => p.Isvalid && p.Id == id).FirstOrDefault();
            if (product != null)
            {
                _omsAccessor.Delete(product);
                _omsAccessor.SaveChanges();
                return true;
            }
            else
            {
                throw new ArgumentException("Product");
            }
        }
        public IQueryable GetAllProducts()
        {
            var ProductList = from p in _omsAccessor.Get<Product>()
                                  //join d in _omsAccessor.Get<Dictionary>() on p.Country equals d.Id
                              where p.Isvalid
                              select p;
            return ProductList;
        }
        public PageList<Product> GetProductList(int pageSize, int pageIndex, int TypeId = 0, string searchStr = "")
        {
            var query = _omsAccessor.Get<Product>().Where(x => x.Isvalid);
            if (TypeId != 0)
            {
                query = query.Where(x => x.Type == TypeId);
            }
            if (!string.IsNullOrEmpty(searchStr))
            {
                query = query.Where(x => x.Name.Contains(searchStr) || x.NameEn.Contains(searchStr)||x.Code.Contains(searchStr));
            }
            return new PageList<Product>(query,pageIndex,pageSize);
        }

    }
}
