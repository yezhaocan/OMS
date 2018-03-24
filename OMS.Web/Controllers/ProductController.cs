using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OMS.Data.Domain;
using OMS.Model;
using OMS.Services.Common;
using OMS.Services.Products;
using OMS.WebCore;


namespace OMS.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ICommonService _commonService;
        public ProductController(IProductService productService,
            ICommonService commonService
            )
        {
            _productService = productService;
            _commonService = commonService;
        }

        /// <summary>
        /// 商品主页
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> List()
        {
            var Products = _productService.GetAllProducts();
            return View(Products);
        }

        public IActionResult GetListData(int pageIndex=1,int pageSize=20)
        {
            var data = _productService.GetProductList(pageSize, pageIndex);
            int totalCount = data.TotalCount;

            var result = new
            {
                draw = pageIndex,
                recordsTotal = totalCount,
                recordsFiltered = totalCount,
                data = data
            };

            return Json(result);
        }
        /// <summary>
        /// 添加商品
        /// </summary>
        /// <returns></returns>
        public IActionResult CreatedProduct()
        {
            //获取字典数据
            ViewBag.Country = new SelectList(_commonService.GetBaseDictionaryList(DictionaryType.Country), "Id", "Value");
            ViewBag.Types = new SelectList(_commonService.GetBaseDictionaryList(DictionaryType.ProductType), "Id", "Value");
            ViewBag.Area = new SelectList(_commonService.GetBaseDictionaryList(DictionaryType.Area), "Id", "Value");
            ViewBag.Grapes = new SelectList(_commonService.GetBaseDictionaryList(DictionaryType.Variety), "Id", "Value");
            ViewBag.Capacity = new SelectList(_commonService.GetBaseDictionaryList(DictionaryType.capacity), "Id", "Value");
            ViewBag.Packing = new SelectList(_commonService.GetBaseDictionaryList(DictionaryType.Packing), "Id", "Value");

            //ProductModel model = new ProductModel();
            //PrepSelectItem(model);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatedProduct(ProductModel model)
        {
            if (model!=null)
            {
                Product product = model.ToEntity();
                _productService.InsertProduct(product);
                SuccessNotification("商品添加成功！");
                return RedirectToAction("List");
            }
            else
            {
                ErrorNotification("信息填写有误！");
            }
            return View();
        }
        /// <summary>
        /// 编辑商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult EditProduct(int id)
        {
            var product = _productService.GetProductById(id);
            //if (product == null)
            //    return RedirectToAction("CreatedProduct");
            //ProductModel model = product.ToModel();
            //PrepSelectItem(model);
            //获取字典数据
            ViewBag.Country = new SelectList(_commonService.GetBaseDictionaryList(DictionaryType.Country), "Id", "Value", product.Country);
            ViewBag.Types = new SelectList(_commonService.GetBaseDictionaryList(DictionaryType.ProductType), "Id", "Value", product.Type);
            ViewBag.Area = new SelectList(_commonService.GetBaseDictionaryList(DictionaryType.Area), "Id", "Value", product.Area);
            ViewBag.Grapes = new SelectList(_commonService.GetBaseDictionaryList(DictionaryType.Variety), "Id", "Value", product.Grapes);
            ViewBag.Capacity = new SelectList(_commonService.GetBaseDictionaryList(DictionaryType.capacity), "Id", "Value", product.Capacity);
            ViewBag.Packing = new SelectList(_commonService.GetBaseDictionaryList(DictionaryType.Packing), "Id", "Value", product.Packing);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(Product product)
        {

            _productService.UpdateProduct(product);
            return RedirectToAction("List");
        }
        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DelProduct(int id)
        {
            _productService.DelProduct(id);
            InfoNotification("删除成功！");
            return RedirectToAction("List");
        }
        [HttpPost]
        public IActionResult GetProducts(string search,int pageSize,int pageIndex) {
            var data = _productService.GetProductList(pageSize, pageIndex,0, search);
            return Success(data);
        }
        [HttpPost]
        public IActionResult GetProductInfo(int productId, int priceTypeId)
        {
            var productModel = _productService.GetProductById(productId).ToModel();
            var saleProduct = _productService.GetSaleProduct(productId, 94, priceTypeId);
            productModel.SaleProductModel.Add(saleProduct.ToModel());            
            return Success(productModel);
        }
        private void PrepSelectItem(ProductModel model)
        {
            var dictionarys = _commonService.GetAllDictionarys();
            //商品类型
            foreach(var item in dictionarys.Where(x => x.Type == DictionaryType.ProductType))
            {
                model.Types.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString(), Selected = (item.Id == model.Type) });
            }
            //国家
            foreach (var item in dictionarys.Where(x => x.Type == DictionaryType.Country))
            {
                model.Countries.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString(), Selected = (item.Id == model.Type) });
            }
            //产区
            foreach (var item in dictionarys.Where(x => x.Type == DictionaryType.Area))
            {
                model.Areas.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString(), Selected = (item.Id == model.Type) });
            }
            //葡萄品种
            foreach (var item in dictionarys.Where(x => x.Type == DictionaryType.Variety))
            {
                model.GrapeItems.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString(), Selected = (item.Id == model.Type) });
            }
            //容量
            foreach (var item in dictionarys.Where(x => x.Type == DictionaryType.capacity))
            {
                model.Capacitys.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString(), Selected = (item.Id == model.Type) });
            }
            //包装方式
            foreach (var item in dictionarys.Where(x => x.Type == DictionaryType.Packing))
            {
                model.Packings.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString(), Selected = (item.Id == model.Type) });
            }
        }
    }
}