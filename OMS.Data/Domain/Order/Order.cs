using System;
using System.Collections.Generic;
using System.Text;

namespace OMS.Data.Domain
{
    public class Order : EntityBase
    {
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
        public decimal PayPrice{ get; set; }
        public int DeliveryTypeId { get; set; }
        public string DeliveryNumber { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string AddressDetail { get; set; }
        public int DistrictId { get; set; }
        public string CustomerMark { get; set; }
        public InvoiceType InvoiceType { get; set; }
        public string AdminMark { get; set; }
        public string ToWarehouseMessage { get; set; }
        public int WarehouseId { get; set; }
        public int CustomerId { get; set; }
        public int PriceTypeId { get; set; }
        public int ApprovalProcessId { get; set; }
        public List<OrderApproval> OrderApproval { get; set; }
        public WareHouse WareHouse { get; set; }
        public Shop Shop { get; set; }
        public Delivery Delivery { get; set; }
        public List<OrderProduct> OrderProduct { get; set; }
        public List<OrderPayPrice> OrderPayPrice { get; set; }
        //订单表 关联大部分主外键关系，但是类似customer,ApprovalProcess则因为B2C订单没有这个字段所以不关联
        //还有就是不直接关联 dictionary字典表

        public InvoiceInfo InvoiceInfo { get; set; }

    }
}
