using OMS.Core;
using OMS.Data.Domain;
using OMS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OMS.Services.Order1
{
    public interface IOrderService
    {
        /// <summary>
        /// 添加审核流程
        /// </summary>
        /// <param name="approvalProcess"></param>
        /// <returns></returns>
        bool InsertApprovalProcess(ApprovalProcess approvalProcess);
        /// <summary>
        /// 删除审核流程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteApprovalProcess(int id);
        /// <summary>
        /// 更新审核人员排序
        /// </summary>
        /// <param name="apdId"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        bool UpdateAPDetailSort(int apdId, string userIds);
        /// <summary>
        /// 获取审核流程
        /// </summary>
        /// <returns></returns>
        List<ApprovalProcess> GetAllApprovalProcessList();
        /// <summary>
        /// 添加B2B订单
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns>新生成订单Id</returns>
        int AddB2BOrder(OrderModel orderModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        bool UpdateB2BOrder(OrderModel orderModel);
        /// <summary>
        /// 添加订单商品
        /// </summary>
        /// <param name="orderProduct"></param>
        /// <returns></returns>
        bool AddOrderProduct(OrderProduct orderProduct);
        /// <summary>
        /// 添加订单商品
        /// </summary>
        /// <param name="orderProduct"></param>
        /// <returns></returns>
        bool UpdateOrderProduct(OrderProduct orderProduct);
        /// <summary>
        /// 获取订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Order GetOrderById(int orderId);
        /// <summary>
        /// 订单商品分页
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        PageList<OrderProduct> GetOrderProductByOrderId(int orderId, int pageIndex, int pageSize, string search="");
        /// <summary>
        /// 订单分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageList<OrderModel> GetOrderListByType(OrderModel orderMode,int pageIndex, int pageSize);
        /// <summary>
        /// 根据orderProductId获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OrderProduct GetOrderProductById(int id);
        /// <summary>
        /// 删除订单商品根据orderproductid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteOrderProductById(int id);
        /// <summary>
        /// 审核订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="state"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool ApprovalOrder(int orderId, bool state,out string msg);
        /// <summary>
        /// 财务确认订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool ConfirmOrder(int orderId, out string msg);
        /// <summary>
        /// 财务记账
        /// </summary>
        /// <param name="orderModel"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool BookKeeping(OrderModel orderModel, out string msg);
        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        bool DeleteOrder(int orderId, out string msg);
        int CheckOrderProductCount(int orderId, int productId);
        /// <summary>
        /// 根据订单获取客户信息，默认发票信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Customers GetDefaultInvoiceInfo(int orderId);
        /// <summary>
        /// 插入订单发票
        /// </summary>
        /// <param name="invoiceInfo"></param>
        /// <returns></returns>
        bool SubmitOrderInvoiceInfo(InvoiceInfoModel invoiceInfoModel, int orderId);
        /// <summary>
        /// 物流
        /// </summary>
        /// <returns></returns>
        List<Delivery> GetAllDeliveryList();
    }
}
