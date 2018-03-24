﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OMS.Model
{ 
    public class ProductModel
    {
        public ProductModel()
        {
            Types = new List<SelectListItem>();
            Countries = new List<SelectListItem>();
            Areas = new List<SelectListItem>();
            GrapeItems = new List<SelectListItem>();
            Capacitys = new List<SelectListItem>();
            Packings = new List<SelectListItem>();
            SaleProductModel = new List<SaleProductModel>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage ="不能为空")]
        public string Name { get; set; }
        public string NameEn { get; set; }
        /// <summary>
        /// 商品类型（数据源-字典表-type=1）
        /// </summary>
        public int Type { get; set; }
        public IList<SelectListItem> Types { get; set; }
        public string Cover { get; set; }
        /// <summary>
        /// 国家（数据源-字典表-type=2）
        /// </summary>
        public int Country { get; set; }
        public IList<SelectListItem> Countries { get; set; }
        /// <summary>
        /// 产区（数据源-字典表-type=3）
        /// </summary>
        public int Area { get; set; }
        public IList<SelectListItem> Areas { get; set; }
        /// <summary>
        /// 葡萄品种（数据源-字典表-type=4）
        /// </summary>
        public string Grapes { get; set; }
        public IList<SelectListItem> GrapeItems { get; set; }
        /// <summary>
        /// 容量（数据源-字典表-type=5）
        /// </summary>
        public int Capacity { get; set; }
        public IList<SelectListItem> Capacitys { get; set; }
        //[Required(ErrorMessage = "不能为空")]
        public string Year { get; set; }
        /// <summary>
        /// 包装方式（数据源-字典表-type=6）
        /// </summary>
        public string Packing { get; set; }
        public IList<SelectListItem> Packings { get; set; }
        /// <summary>
        /// 所属系列Id
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 商品唯一编码
        /// </summary>
        public string Code { get; set; }
        public List<SaleProductModel> SaleProductModel { get; set; }
    }
}
