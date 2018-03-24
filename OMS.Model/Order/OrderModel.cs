using System;
using System.Collections.Generic;
using System.Text;
using OMS.Data.Domain;
using OMS.Model.B2B;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OMS.Model
{
    public class OrderModel : ModelBase
    {
        public OrderModel()
        {
            InvoiceTypes = new List<SelectListItem>() {
                new SelectListItem(){ Value="0",Text="不用发票" },
                new SelectListItem(){ Value="1",Text="普通个人发票" },
                new SelectListItem(){ Value="2",Text="普通单位发票" },
                new SelectListItem(){ Value="3",Text="专用发票" }
            };
        }
        public string SerialNumber { get; set; }
        public OrderType Type { get; set; }
        public int ShopId { get; set; }
        public string PSerialNumber { get; set; }
        public string OrgionSerialNumber { get; set; }
        public OrderState State { get; set; }
        public WriteBackState WriteBackState { get; set; }
        public int PayType { get; set; }
        public int PayMentType { get; set; }
        public PayState PayState { get; set; }
        public DateTime? TransDate { get; set; }
        public bool IsLocked { get; set; }
        public int LockMan { get; set; }
        public bool LockStock { get; set; }
        public decimal SumPrice { get; set; }
        public decimal PayPrice { get; set; }
        public int DeliveryTypeId { get; set; }
        public string DeliveryNumber { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string AddressDetail { get; set; }
        public int DistrictId { get; set; }
        /// <summary>
        /// 客户备注
        /// </summary>
        public string CustomerMark { get; set; }
        public InvoiceType InvoiceType { get; set; }
        /// <summary>
        /// 管理员备注
        /// </summary>
        public string AdminMark { get; set; }
        public string ToWarehouseMessage { get; set; }
        public int WarehouseId { get; set; }
        public int CustomerId { get; set; }
        public int PriceTypeId { get; set; }
        public int ApprovalProcessId { get; set; }
        public List<Customers> Customers { get; set; }
        public List<WareHouse> WareHouses { get; set; }
        public List<Dictionary> PriceType { get; set; }
        public List<ApprovalProcessModel> ApprovalProcessModel { get; set; }
        public List<Delivery> Delivery { get; set; }
        public List<Dictionary> PayTypes { get; set; }
        public List<Dictionary> PayMentTypes { get; set; }
        public List<SelectListItem> InvoiceTypes { get; set; }

        public InvoiceInfoModel InvoiceInfoModel { get; set; }
        /// <summary>
        /// 订单创建人
        /// </summary>
        public int? CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// 当前审核人
        /// </summary>
        public int Reviewer { get; set; }
        /// <summary>
        /// 客户（公司）
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 订单的总商品数
        /// </summary>
        public int OrderProductCount { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string StateStr { get; set; }

        /// <summary>
        /// 付款或者退款
        /// </summary>
        public bool IsPayOrRefund { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}