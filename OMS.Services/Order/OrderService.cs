using System;
using System.Collections.Generic;
using System.Text;
using OMS.Core;
using OMS.Data.Domain;
using OMS.Data.Interface;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OMS.Model;

namespace OMS.Services.Order1
{
    public class OrderService : ServiceBase, IOrderService
    {
        #region ctor
        public OrderService(IDbAccessor omsAccessor, IWorkContext workContext) 
            : base(omsAccessor, workContext)
        {

        }

        #endregion

        #region B2B订单
        public bool InsertApprovalProcess(ApprovalProcess approvalProcess)
        {
            try
            {
                _omsAccessor.Insert(approvalProcess);
                _omsAccessor.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool DeleteApprovalProcess(int id)
        {
            try
            {
                var d = _omsAccessor.Get<ApprovalProcessDetail>().Where(p => p.ApprovalProcessId == id & p.Isvalid);
                _omsAccessor.DeleteRange(d);
                _omsAccessor.DeleteById<ApprovalProcess>(id);
                _omsAccessor.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool UpdateAPDetailSort(int apdId, string userIds)
        {
            try
            {
                var sort = userIds.Split(',');
                if (sort.Length > 0)
                {
                   var list=  _omsAccessor.Get<ApprovalProcessDetail>().Where(p => p.ApprovalProcessId == apdId & p.Isvalid);
                    for (int i = 0; i < sort.Length; i++)
                    {
                        var s = Convert.ToInt32(sort[i]);
                        foreach (var item in list)
                        {
                            if (s == item.UserId)
                            {
                                item.Sort = i + 1;
                                item.ModifiedTime = DateTime.Now;
                                item.ModifiedBy = _workContext.CurrentUser.Id;
                                _omsAccessor.Update(item);
                                break;
                            }
                        }
                    }
                }
                _omsAccessor.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw  e;
            }
        }
        public List<ApprovalProcess> GetAllApprovalProcessList()
        {
            return _omsAccessor.Get<ApprovalProcess>()
                .Where(p => p.Isvalid).OrderBy(p=>p.CreatedTime).Include(p => p.ApprovalProcessDetail).ThenInclude(p => p.User).ToList();
        }
        public int AddB2BOrder(OrderModel orderModel)
        {
            try
            {
                List<OrderApproval> orderApproval = new List<OrderApproval>();
                var approvalProcess = _omsAccessor.Get<ApprovalProcess>().Where(p => p.Id == orderModel.ApprovalProcessId && p.Isvalid).Include(p => p.ApprovalProcessDetail).FirstOrDefault();
                if (approvalProcess != null) {
                    foreach (var item in approvalProcess.ApprovalProcessDetail)
                    {
                        OrderApproval o = new OrderApproval
                        {
                            CreatedBy = _workContext.CurrentUser.Id,
                            UserId = item.UserId,
                            Sort = item.Sort,
                            State = OrderApprovalState.Unaudited
                        };
                        orderApproval.Add(o);
                    }
                }
                Order order = new Order
                {
                    SerialNumber = orderModel.SerialNumber,
                    Type = OrderType.B2B,
                    ShopId=0,
                    State=OrderState.ToBeConfirmed,
                    WriteBackState=WriteBackState.NoWrite,
                    IsLocked=false,
                    LockStock=false,
                    CustomerName=orderModel.CustomerName,
                    CustomerPhone=orderModel.CustomerPhone,
                    CustomerMark = orderModel.CustomerMark,
                    AddressDetail=orderModel.AddressDetail,
                    WarehouseId=orderModel.WarehouseId,
                    CustomerId=orderModel.CustomerId,
                    PriceTypeId=orderModel.PriceTypeId,
                    DeliveryTypeId=orderModel.DeliveryTypeId,
                    OrderApproval= orderApproval,
                    ApprovalProcessId=orderModel.ApprovalProcessId,
                    CreatedBy = _workContext.CurrentUser.Id,
                    InvoiceType = orderModel.InvoiceType,
                };
                _omsAccessor.Insert(order);
                _omsAccessor.SaveChanges();
                return order.Id;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public bool UpdateB2BOrder(OrderModel orderModel)
        {
            try
            {
                //重新审核，如果已审核，解除库存锁定
                var order = Re_Review(orderModel.Id,orderModel.ApprovalProcessId);

                order.CustomerName = orderModel.CustomerName;
                order.CustomerPhone = orderModel.CustomerPhone;
                order.CustomerMark = orderModel.CustomerMark;
                order.AddressDetail = orderModel.AddressDetail;
                order.WarehouseId = orderModel.WarehouseId;
                order.CustomerId = orderModel.CustomerId;
                order.PriceTypeId = orderModel.PriceTypeId;
                order.DeliveryTypeId = orderModel.DeliveryTypeId;
                order.ApprovalProcessId = orderModel.ApprovalProcessId;
                order.InvoiceType = orderModel.InvoiceType;
                order.ModifiedBy = _workContext.CurrentUser.Id;
                order.ModifiedTime = DateTime.Now;
                _omsAccessor.Update(order);
                _omsAccessor.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 修改订单，需要重新审核
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        public Order Re_Review(int orderId,int approvalProcessId=0) {

            var order = _omsAccessor.Get<Order>().Where(p => p.Id == orderId && p.Isvalid).Include(p => p.OrderProduct).FirstOrDefault();
            if (order == null) { return new Order(); }
            if (approvalProcessId == 0) { approvalProcessId = order.ApprovalProcessId; }
            List<OrderApproval> orderApproval = _omsAccessor.Get<OrderApproval>().Where(p => p.OrderId == orderId && p.Isvalid && p.State != OrderApprovalState.Failure).ToList();
            orderApproval.Each(p => p.State = OrderApprovalState.Failure);
            var approvalProcess = _omsAccessor.Get<ApprovalProcess>().Where(p => p.Id == approvalProcessId && p.Isvalid).Include(p => p.ApprovalProcessDetail).FirstOrDefault();
            if (approvalProcess != null)
            {
                foreach (var item in approvalProcess.ApprovalProcessDetail)
                {
                    OrderApproval o = new OrderApproval
                    {
                        CreatedBy = _workContext.CurrentUser.Id,
                        UserId = item.UserId,
                        Sort = item.Sort,
                        State = OrderApprovalState.Unaudited
                    };
                    orderApproval.Add(o);
                }
            }
            order.State = OrderState.ToBeConfirmed;//修改订单，需要重新待审核
            if (order.LockStock)
            {
                //如果锁定库存，需先释放库存
                 foreach (var item in order.OrderProduct)
                {
                    if (item.SaleProduct != null)
                    {
                        item.SaleProduct.LockStock = item.SaleProduct.LockStock + item.Quantity;
                        item.SaleProduct.AvailableStock = item.SaleProduct.AvailableStock - item.Quantity;
                    }
                }
                 //已解锁
                order.LockStock = false;
            }
            order.OrderApproval = orderApproval;
            return order;
        }
        public bool AddOrderProduct(OrderProduct orderProduct)
        {
            try
            {
                _omsAccessor.Insert(orderProduct);                
                _omsAccessor.SaveChanges();
                var order = Re_Review(orderProduct.OrderId);
                if (order != null && order.Type == OrderType.B2B)
                {
                    order.SumPrice = _omsAccessor.Get<OrderProduct>().Where(p => p.OrderId == orderProduct.OrderId && p.Isvalid).Sum(p => p.SumPrice);
                    _omsAccessor.Update(order);
                    _omsAccessor.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool UpdateOrderProduct(OrderProduct orderProduct)
        {
            try
            {
                _omsAccessor.Update(orderProduct);
                _omsAccessor.SaveChanges();
                var order = Re_Review(orderProduct.OrderId);
                if (order != null && order.Type == OrderType.B2B)
                {
                    order.SumPrice = _omsAccessor.Get<OrderProduct>().Where(p => p.OrderId == orderProduct.OrderId && p.Isvalid).Sum(p => p.SumPrice);
                    _omsAccessor.Update(order);
                    _omsAccessor.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public PageList<OrderProduct> GetOrderProductByOrderId(int orderId, int pageIndex, int pageSize, string search="")
        {
            var result = _omsAccessor.Get<OrderProduct>().Where(p => p.OrderId == orderId && p.Isvalid 
            && (string.IsNullOrEmpty(search) || p.SaleProduct.Product.Name.Contains(search))).Include(x=>x.SaleProduct).ThenInclude(x=>x.Product);
            return new PageList<OrderProduct>(result, pageIndex, pageSize);
        }
        public PageList<OrderModel> GetOrderListByType(OrderModel orderMode,int pageIndex, int pageSize)
        {
            List<OrderModel> list = new List<OrderModel>();
            var result = _omsAccessor.Get<Order>().Where(p => p.Isvalid
            && p.Type == orderMode.Type
            && (string.IsNullOrEmpty(orderMode.SerialNumber) || p.SerialNumber.Contains(orderMode.SerialNumber))
            && (!orderMode.StartTime.HasValue || p.CreatedTime > orderMode.StartTime.Value)
            && (!orderMode.EndTime.HasValue || p.CreatedTime < orderMode.EndTime.Value)
            && (orderMode.CustomerId ==0|| p.CustomerId == orderMode.CustomerId)
            ).Include(p => p.OrderProduct);
            foreach (var item in result.Skip((pageIndex - 1) * pageSize).Take(pageSize))//分页
            {
                OrderModel orderModel = new OrderModel
                {
                    Id = item.Id,
                    SerialNumber = item.SerialNumber,
                    CreatedTime = item.CreatedTime,
                    Company = _omsAccessor.GetById<Customers>(item.CustomerId)?.Name,
                    OrderProductCount = item.OrderProduct.Where(p => p.Isvalid).Sum(p => p.Quantity),
                    SumPrice = item.SumPrice,
                    StateStr = GetOrderType(item.State),
                };
                list.Add(orderModel);
            }
            return new PageList<OrderModel>(list, pageIndex, pageSize,result.Count());
        }
        private static string GetOrderType(OrderState orderState)
        {
            switch (orderState) {
                case OrderState.ToBeTurned:return "待转单";
                case OrderState.ToBeConfirmed:return "待审核";
                case OrderState.Confirmed:return "已审核";
                case OrderState.FinancialConfirmation:return "财务已确认";
                case OrderState.returned:return "被退回";
            };
            return "待转单";
        }
        public Order GetOrderById(int orderId)
        {
            var result = _omsAccessor.Get<Order>().Where(p => p.Id == orderId && p.Isvalid).Include(p=>p.OrderApproval).Include(p=>p.InvoiceInfo).FirstOrDefault();

            return result;
        }
        public OrderProduct GetOrderProductById(int id)
        {
            return _omsAccessor.Get<OrderProduct>().Where(p => p.Id == id).Include(x => x.SaleProduct).ThenInclude(x=>x.Product).FirstOrDefault();
        }
        public bool DeleteOrderProductById(int id)
        {
            try
            {
                _omsAccessor.DeleteById<OrderProduct>(id);
                _omsAccessor.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int CheckOrderProductCount(int orderId, int productId)
        {
            //B2B订单一个订单商品只能有一条记录
           return  _omsAccessor.Get<OrderProduct>().Where(p => p.OrderId == orderId && p.SaleProduct.ProductId == productId && p.Isvalid).Count();
        }
        public bool ApprovalOrder(int orderId, bool state,out string msg)
        {
            var list = _omsAccessor.Get<OrderApproval>().Where(p => p.OrderId == orderId && p.State == OrderApprovalState.Unaudited).OrderBy(p => p.Sort);
            var orderApproval = list.FirstOrDefault();
            if (orderApproval == null)
            {
                msg = "当前订单不需要审核";
                return false;
            }
            else {
                if (orderApproval.UserId == _workContext.CurrentUser.Id)
                {
                    if (state)
                    {
                        //通过
                        orderApproval.State = OrderApprovalState.Audited;
                        var order = _omsAccessor.GetById<Order>(orderId);
                        if (list.Count() == 1&& !order.LockStock)
                        {
                            order.State = OrderState.Confirmed;
                            order.LockStock = true;
                            //进入财务确认
                            _omsAccessor.Update(order);
                           var orderProduct = _omsAccessor.Get<OrderProduct>().Where(p => p.OrderId == orderId && p.Isvalid).Include(p => p.SaleProduct);
                           foreach (var item in orderProduct)
                            {
                                if (item.SaleProduct != null) {
                                    item.SaleProduct.LockStock = item.SaleProduct.LockStock + item.Quantity;
                                    item.SaleProduct.AvailableStock = item.SaleProduct.AvailableStock - item.Quantity;
                                }
                                //锁定库存
                                _omsAccessor.Update(item.SaleProduct);
                            }
                        }
                        _omsAccessor.Update(orderApproval);
                        _omsAccessor.SaveChanges();
                        msg = "订单审核成功";
                        return true;
                    }
                    else {
                        //退回
                        var order = _omsAccessor.GetById<Order>(orderId);
                        order.State = OrderState.returned;
                        _omsAccessor.Update(order);
                        _omsAccessor.SaveChanges();
                        msg = "订单退回成功";
                        return true;
                    }

                }
                else {
                    msg = "订单当前审核人不是该账号！";
                    return false;
                }
            }
        }

        public bool ConfirmOrder(int orderId, out string msg)
        {
            if (_workContext.CurrentUser.Id == 1)//权限系统完成之后，添加财务角色判断
            {  //确认人，判断
                var order = _omsAccessor.GetById<Order>(orderId);
                if (order.State == OrderState.Confirmed)
                {
                    order.State = OrderState.FinancialConfirmation;
                    _omsAccessor.Update(order);
                    _omsAccessor.SaveChanges();
                    msg = "订单确认成功";
                    return true;
                }
                else {
                    msg = "该订单不需要确认";
                    return false;
                }
            }
            else {
                msg = "该账号没有确认权限";
                return false;
            }
        }
        public bool BookKeeping(OrderModel orderModel, out string msg)
        {
            var order = _omsAccessor.Get<Order>().Where(p=>p.Id==orderModel.Id).Include(p=>p.OrderPayPrice).FirstOrDefault();
            if (order == null) {
                msg = "订单信息错误，请核实！";
                return false;
            }
            if (order.PayPrice == 0)
            {
                if (!orderModel.IsPayOrRefund)
                {
                    msg = "当前订单未支付，不支持退款！";
                    return false;
                }
                else {
                    if (orderModel.PayPrice > order.SumPrice) {
                        msg = "支付金额大于订单总金额，操作失败！";
                        return false;
                    }
                }
                order.PayType = orderModel.PayType;
                order.PayMentType = orderModel.PayMentType;
                order.PayPrice = orderModel.PayPrice;
                OrderPayPrice orderPayPrice = new OrderPayPrice
                {
                    OrderId = order.Id,
                    IsPay = orderModel.IsPayOrRefund,
                    PayType = orderModel.PayType,
                    PayMentType = orderModel.PayMentType,
                    Price = orderModel.PayPrice,
                    Mark = orderModel.AdminMark,
                };
                order.OrderPayPrice.Add(orderPayPrice);
            }
            else
            {
                if (orderModel.IsPayOrRefund)
                {//付款
                    if (orderModel.PayPrice + order.PayPrice > order.SumPrice)
                    {
                        msg = "支付金额大于订单总金额，操作失败！";
                        return false;
                    }
                    order.PayPrice += orderModel.PayPrice;
                }
                else {
                    //退款
                    if (orderModel.PayPrice  > order.PayPrice)
                    {
                        msg = "退款金额大于已支付金额，操作失败";
                        return false;
                    }
                    order.PayPrice -= orderModel.PayPrice;
                }
                OrderPayPrice orderPayPrice = new OrderPayPrice
                {
                    OrderId = order.Id,
                    IsPay = orderModel.IsPayOrRefund,
                    PayType = orderModel.PayType,
                    PayMentType = orderModel.PayMentType,
                    Price = orderModel.PayPrice,
                    Mark = orderModel.AdminMark,
                };
                order.OrderPayPrice.Add(orderPayPrice);
            }
            _omsAccessor.Update(order);
            _omsAccessor.SaveChanges();
            msg = "操作成功";
            return true;
        }
        public bool DeleteOrder(int orderId, out string msg)
        {
            var order = _omsAccessor.GetById<Order>(orderId);
            if (order == null)
            {
                msg = "删除失败";
                return false;
            }
            if (_workContext.CurrentUser.Id != order.CreatedBy)
            {
                msg = "当前账号没有权限删除该订单，只能由创建者删除！";
                return false;
            }
            order.Isvalid = false;
            _omsAccessor.Update(order);
            _omsAccessor.SaveChanges();
            msg = "删除成功";
            return true;
        }

        public Customers GetDefaultInvoiceInfo(int orderId)
        {
            var order = _omsAccessor.GetById<Order>(orderId);
            if (order == null) { return new Customers(); }
            return _omsAccessor.GetById<Customers>(order.CustomerId);
        }
        public bool SubmitOrderInvoiceInfo(InvoiceInfoModel  invoiceInfoModel,int orderId)
        {
            try
            {
                InvoiceInfo invoiceInfo = new InvoiceInfo
                {
                    OrderId = orderId,
                    CustomerEmail = invoiceInfoModel.CustomerEmail,
                    Title = invoiceInfoModel.Title,
                    TaxpayerID = invoiceInfoModel.TaxpayerID,
                    RegisterAddress = invoiceInfoModel.RegisterAddress,
                    RegisterTel = invoiceInfoModel.RegisterTel,
                    BankAccount = invoiceInfoModel.BankAccount,
                    BankOfDeposit = invoiceInfoModel.BankOfDeposit,
                    CreatedBy = _workContext.CurrentUser.Id

                };

                _omsAccessor.Insert(invoiceInfo);
                _omsAccessor.SaveChanges();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion

        #region 物流
        public List<Delivery> GetAllDeliveryList()
        {
           return _omsAccessor.Get<Delivery>().Where(p => p.Isvalid).ToList();
        }
        #endregion

    }
}
