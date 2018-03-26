using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OMS.Services.Common;
using OMS.Data.Domain;
using OMS.Model;
using OMS.Core.Tools;
using OMS.Services.Customer;
using OMS.Services.Order1;
using OMS.Services.Account;
using OMS.Model.B2B;
using OMS.WebCore;
using OMS.Core;
using OMS.Services;

namespace OMS.Web.Controllers
{
    public class B2BOrderController : BaseController
    {
        public readonly ICommonService _commonService;
        public readonly ICustomerService _customerService;
        public readonly IOrderService _orderService;
        public readonly IUserService _userService;
        private readonly IWareHouseService _wareHouseService;
        public B2BOrderController(ICommonService commonService
            , ICustomerService customerService
            , IOrderService orderService
            , IUserService userService
            , IWareHouseService wareHouseService) {
            _commonService = commonService;
            _customerService = customerService;
            _orderService = orderService;
            _userService = userService;
            _wareHouseService = wareHouseService;
        }
        #region Order
        public IActionResult B2BSalesBill()
        {
            ViewBag.Customer = _customerService.GetAllCustomerList();
            return View();
        }
        public IActionResult AddSalesBill(int orderId = 0)
        {
            var result = _orderService.GetAllApprovalProcessList();
            List<ApprovalProcessModel> list = new List<ApprovalProcessModel>();
            foreach (var item in result)
            {
                list.Add(item.ToModel());
            }
            OrderModel orderModel = new OrderModel();
            if (orderId == 0)
            {
                orderModel.SerialNumber = CommonTools.GetSerialNumber("PF");

            }
            else {
                orderModel = _orderService.GetOrderById(orderId).ToModel();
            }
            orderModel.Customers = _customerService.GetAllCustomerList();
            orderModel.WareHouses = _wareHouseService.GetAllWareHouseList();
            orderModel.PriceType = _commonService.GetBaseDictionaryList(DictionaryType.PriceType);
            orderModel.ApprovalProcessModel = list;
            orderModel.Delivery = _orderService.GetAllDeliveryList();
            orderModel.PayTypes = _commonService.GetBaseDictionaryList(DictionaryType.PayStyle);
            orderModel.PayMentTypes = _commonService.GetBaseDictionaryList(DictionaryType.PayType);
            return View(orderModel);
        }
        [HttpPost]
        public IActionResult SelectCustomer(int customerId)
        {
            var data = _customerService.GetById(customerId);
            return Success(data);
        }
        [HttpPost]
        public IActionResult AddB2BOrder(OrderModel orderModel)
        {
            var orderId = _orderService.AddB2BOrder(orderModel);
            return Success("",orderId);//传回新增订单Id继续操作
        }
        [HttpPost]
        public IActionResult UpdateB2BOrder(OrderModel orderModel)
        {
            _orderService.UpdateB2BOrder(orderModel);
            return Success();
        }
        [HttpPost]
        public IActionResult AddB2BOrderProduct(OrderProductModel orderProductModel)
        {
            _orderService.AddOrderProduct(orderProductModel.ToEntity());
            return Success();
        }
        [HttpPost]
        public IActionResult UpdateB2BOrderProduct(OrderProductModel orderProductModel)
        {
            _orderService.UpdateOrderProduct(orderProductModel.ToEntity());
            return Success();
        }
        public IActionResult CheckOrderProductCount(int orderId, int productId)
        {
            var count = _orderService.CheckOrderProductCount(orderId, productId);
            return Success("",count);
        }
        [HttpPost]
        public IActionResult GetOrderProducts(string search,int orderId, int pageSize, int pageIndex)
        {
            var data = _orderService.GetOrderProductByOrderId(orderId, pageIndex, pageSize, search);
            //entity To Model
            var orderProductModel = new PageList<OrderProductModel>(data.Select(x => { return x.ToModel(); }), data.PageIndex, data.PageSize, data.TotalCount);
            return Success(orderProductModel);
        }
        [HttpPost]
        public IActionResult GetOrderProductInfo(int id)
        {
            var data = _orderService.GetOrderProductById(id).ToModel(); ;
            return Success(data);
        }
        [HttpPost]
        public IActionResult DeleteOrderProductById(int id)
        {
            _orderService.DeleteOrderProductById(id);
            return Success();
        }
        [HttpPost]
        public IActionResult GetOrderList(OrderModel orderModel, int pageIndex, int pageSize)
        {
            orderModel.Type = OrderType.B2B;
            var data= _orderService.GetOrderListByType(orderModel,pageIndex, pageSize);
            return Success(data);
        }
        [HttpPost]
        public IActionResult SubmitApproval(int orderId)
        {
            var result = _orderService.SubmitApproval(orderId, out string msg);
            if (result)
            {
                return Success();
            }
            else
            {
                return Error(msg);
            }
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ApprovalOrder(int orderId, bool state)
        {
            var result= _orderService.ApprovalOrder(orderId, state, out string msg);
            if (result)
            {
                return Success();
            }
            else {
                return Error(msg);
            }
        }
        /// <summary>
        /// 财务确认
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ConfirmOrder(int orderId)
        {
            var result = _orderService.ConfirmOrder(orderId, out string msg);
            if (result)
            {
                return Success();
            }
            else
            {
                return Error(msg);
            }
        }
        [HttpPost]
        public IActionResult BookKeeping(OrderModel orderModel)
        {
            var result = _orderService.BookKeeping(orderModel, out string msg);
            if (result)
            {
                return Success();
            }
            else
            {
                return Error(msg);
            }
        }
        [HttpPost]
        public IActionResult DeleteOrder(int orderId)
        {
            var result = _orderService.DeleteOrder(orderId,out string msg);
            if (result)
            {
                return Success();
            }
            else
            {
                return Error(msg);
            }
        }
        [HttpPost]
        public IActionResult GetDefaultInvoiceInfo(int orderId)
        {
            var data = _orderService.GetDefaultInvoiceInfo(orderId).ToModel();
            return Success(data);
        }
        public IActionResult SubmitOrderInvoiceInfo(InvoiceInfoModel invoiceInfoModel,int orderId)
        {
            _orderService.SubmitOrderInvoiceInfo(invoiceInfoModel, orderId);
            return Success();

        }
        #endregion
        #region 审核
        public IActionResult ApprovalProcess()
        {
            ViewBag.Users = _userService.GetAllUserList().ToList();
            var result= _orderService.GetAllApprovalProcessList();
            List<ApprovalProcessModel> list = new List<ApprovalProcessModel>();
            foreach (var item in result)
            {
                list.Add(item.ToModel());
            }
            ViewBag.List = list;
            return View();
        }
        [HttpPost]
        public IActionResult AddApprovalProcess(string name, string ids)
        {
            if (IsNullOrEmpty(name)|| IsNullOrEmpty(ids))
                return Error();
            var userIds = ids.Split(",");
            var l = 1;
            List<ApprovalProcessDetail> list = new List<ApprovalProcessDetail>();
            foreach (var it in userIds)
            {
                var uId = Convert.ToInt32(it);
                ApprovalProcessDetail approvalProcessDetail = new ApprovalProcessDetail
                {
                    UserId = uId,
                    Sort = l,
                    CreatedBy = WorkContext.CurrentUser.Id,
                    ModifiedBy = WorkContext.CurrentUser.Id,
                    ModifiedTime =DateTime.Now,
                    User=_userService.GetById(uId)
                };
                list.Add(approvalProcessDetail);
                l++;
            }
            ApprovalProcess approvalProcess = new ApprovalProcess
            {
                Name = name,
                CreatedBy = WorkContext.CurrentUser.Id,
                ModifiedBy = WorkContext.CurrentUser.Id,
                ModifiedTime = DateTime.Now,
                ApprovalProcessDetail = list
            };
            _orderService.InsertApprovalProcess(approvalProcess);
            var data = approvalProcess.ToModel();
            return Success(data);
        }

        [HttpPost]
        public IActionResult UpdateAPDetailSort(int id, string sort)
        {
            _orderService.UpdateAPDetailSort(id, sort);
            return Success();
        }
        [HttpPost]
        public IActionResult DeleteApprovalProcess (int id)
        {
            _orderService.DeleteApprovalProcess(id);
            return Success();
        }
        #endregion
    }
}