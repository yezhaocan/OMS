var productsComponent = function () {
   
    return {
        init: function () {
           // showProducts1();
        }
    }
}();
jQuery(document).ready(function () {
    // productsComponent.init();
    //showProducts1();
    //showOrderProducts();
});
var submit1 = function (ismotify) {
    var isMotify = ismotify || false;
    var billNo = $("#billNo").val();//单据编号
    var customerId = parseInt($("#customer").select2("data")[0].id);//客户
    var wareHousesId = parseInt($("#wareHouses").select2("data")[0].id);//仓库
    var deliveryId = parseInt($("#delivery").select2("data")[0].id);//物流方式
    var priceTypeId = parseInt($("#priceType").select2("data")[0].id);//价格类型
    var contact = $("#contact").val();//联系人
    var mobile = $("#mobile").val().trim();//电话
    var address = $("#address").val();//地址
    var descr = $("#descr").val();//描述
    var invoiceTypeId = parseInt($("#invoiceType").select2("data")[0].id);//发票类型
    var apId = parseInt($("#ap").select2("data")[0].id);
    var OrderModel = {
        SerialNumber: billNo,
        CustomerId: customerId,
        WarehouseId: wareHousesId,
        PriceTypeId: priceTypeId,
        CustomerName: contact,
        CustomerPhone: mobile,
        AddressDetail: address,
        CustomerMark: descr,
        ApprovalProcessId: apId,
        DeliveryTypeId: deliveryId,
        InvoiceType: invoiceTypeId
    }
    if (!(customerId > 0)) {
        alertWarn("请选择客户！");
        return false;
    }
    if (!(wareHousesId > 0)) {
        alertWarn("请选择仓库！");
        return false;
    }
    if (!(deliveryId > 0)) {
        alertWarn("请选择物流方式！");
        return false;
    }
    if (!(priceTypeId > 0)) {
        alertWarn("请选择价格类型！");
        return false;
    }
    if (contact == "") {
        alertWarn("联系人不能为空！");
        return false;
    }
    if (mobile == "") {
        alertWarn("电话不能为空！");
        return false;
    }
    var reg = /^(0|86|17951)?(13[0-9]|15[012356789]|17[01678]|18[0-9]|14[57])[0-9]{8}$/;
    var reg2 = /^(0[0-9]{2,3}\-)([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$/;
    if (!reg.test(mobile) && !reg2.test(mobile))
    {
        alertWarn("电话有误请核实！");
        return false;
    }
    if (address == "") {
        alertWarn("地址不能为空！");
        return false;
    }
    if (!(apId > 0)) {
        alertWarn("请选择审核流程");
        return false;
    }
    if (!(invoiceTypeId >= 0)) {
        alertWarn("请选择发票类型");
        return false;
    }
    OrderModel.InvoiceType = invoiceTypeId - 1;//发票类型0为不需要发票，这个插件不能使用为0的值，或者是读取的时候没办法判断value是默认还是0
    if (isMotify) {
        OrderModel.Id = parseInt($("#orderId").val());
        $oms.ajax({
            url: "/B2BOrder/UpdateB2BOrder",
            data: { OrderModel: OrderModel, },
            success: function (data) {
                if (data.isSucc) {
                    alertSuccess("修改成功");
                    edit("#submit_id");
                    if (invoiceTypeId != 0) {
                        $("#invoice_info").removeClass("display-hide");
                    }
                    else {
                        $("#invoice_info").addClass("display-hide");
                    }
                }
            }
        });
    }
    else {
        $oms.ajax({
            url: "/B2BOrder/AddB2BOrder",
            data: { OrderModel: OrderModel, },
            success: function (data) {
                if (data.isSucc) {
                    alertSuccess("保存成功")
                    $("#product_info").removeClass("display-hide");
                    $("#submit_info").removeClass("display-hide");
                    edit("#submit_id");
                    if (invoiceTypeId != 0) {
                        $("#invoice_info").removeClass("display-hide");
                    }
                    $("#orderId").val(data.count);//记录orderId
                }
            }
        });
    }
}
var showProducts1 = function (searchKey) {
    $oms.paginator({
        pageLimitId: "pageLimit",
        url: "/product/GetProducts",
        data: { search: searchKey, pageIndex: 1, pageSize: 10 },
        success: function (data) {
            var html = "";
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    html += "<tr>" +
                        "<td>" + data[i].code + "</td >" +
                        "<td>" + data[i].name + "</td>" +
                        "<td>" + data[i].nameEn + "</td>" +
                        "<td>" +
                        "<a class=\"delete\" href=\"javascript:;\" onclick=\"getProductInfo('" + data[i].id + "')\"> 添加 </a>" +
                        "</td>" +
                        "</tr >";
                }
            }
            $("#tbody").html(html);
        }
    });
}
var showOrderProducts = function (Id) {
    var search = $("#search").val();
    var orderId = Id || $("#orderId").val();
    $oms.paginator({
        pageLimitId: "pageLimit2",
        url: "/B2BOrder/GetOrderProducts",
        data: { search: search, orderId: orderId },//default 1,10
        success: function (data) {
            if (data.length > 0) {
                var html = "";
                for (var i = 0; i < data.length; i++) {
                    html += "<tr>" +
                        "<td>" + data[i].productName + "</td >" +
                        "<td>" + data[i].productCode + "</td>" +
                        "<td>" + data[i].orginPrice + "</td>" +
                        "<td>" + data[i].price + "</td>" +
                        "<td>" + data[i].quantity + "</td>" +
                        "<td>" + data[i].sumPrice + "</td>" +
                        "<td>" +
                        "<div class=\"btn-group\">" +
                        "<button class=\"btn btn-xs green dropdown-toggle\" type=\"button\" data-toggle=\"dropdown\" aria-expanded=\"false\"> Actions" +
                        "<i class=\"fa fa-angle-down\"></i>" +
                        "</button>" +
                        "<ul class=\"dropdown-menu pull-left\" role=\"menu\">" +
                        "<li>" +
                        "<a  onclick=\"motifyOrderProduct('" + data[i].id + "')\">" +
                        "<i class=\"icon-pencil\"></i> 修改 </a>" +
                        "</li>" +
                        "<li>" +
                        "<a href=\"javascript:;\" onclick=\"deleteOrderProduct('" + data[i].id + "')\">" +
                        "<i class=\"fa fa-remove\"></i> 删除 </a>" +
                        "</li>" +
                        "</ul>" +
                        "</div>" +
                        "</td>" +
                        "</tr >";
                }
                $("#tbody2").html(html);
            }
            else {
                $("#tbody2").html("");
            }
        }
    });
}
var selectProducts = function () {
    var searchKey = $("#searchKey").val();
    showProducts1(searchKey);
}
var getProductInfo = function (id) {
    var productId = id;
    var orderId = parseInt($("#orderId").val());
    var priceTypeId = parseInt($("#priceType").select2("data")[0].id);
    $oms.ajax({
        url: "/B2BOrder/CheckOrderProductCount",
        data: { orderId: orderId, productId: productId },
        success: function (data) {
            if (data.isSucc) {
                if (data.count == 0) {
                    $("#stack2").modal();
                    $oms.ajax({
                        url: "/Product/GetProductInfo",
                        data: { productId: productId, priceTypeId: priceTypeId },
                        success: function (data) {
                            if (data.isSucc) {
                                if (data.data) {
                                    $("#pName").text(data.data.name);
                                    $("#code").text(data.data.code);
                                }
                                if (data.data.saleProductModel[0]) {
                                    $("#order_product_id").val(data.data.saleProductModel[0].id);
                                    $("#availableStock").text(data.data.saleProductModel[0].availableStock);
                                    $("#stock").text(data.data.saleProductModel[0].stock);
                                    $("#lockStock").text(data.data.saleProductModel[0].lockStock);
                                    if (data.data.saleProductModel[0].saleProductPriceModel[0]) {
                                        $("#order_product_orginPrice").val(data.data.saleProductModel[0].saleProductPriceModel[0].price);

                                        f = Math.round(parseFloat(parseFloat($("#order_product_orginPrice").val())*1.00) * 100) / 100;
                                        $("#order_product_price").val(f);
                                    }
                                    else {
                                        alertWarn("该商品未更新价格类型下价格信息");
                                    }
                                }
                                else {
                                    alertWarn("该商品未更新库存信息");
                                }

                            }
                        }
                    });
                }
                else {
                    alertWarn("该商品已添加")
                }
            }
        }
    });
}
var motifyOrderProduct = function (id) {
    var isCreator = $("#isCreator").val();
    if (isCreator == "false") {
        alertWarn("当前账号不是创建人，没有权限修改订单商品");
        return false;
    }
    $oms.ajax({
        url: "/B2BOrder/GetOrderProductInfo",
        data: { id: id },
        success: function (data) {
            if (data.isSucc) {
                $("#pName").text(data.data.productName);
                $("#code").text(data.data.productCode);
                $("#order_product_id").val(data.data.saleProductModel.id);
                $("#availableStock").text(data.data.saleProductModel.availableStock);
                $("#stock").text(data.data.saleProductModel.stock);
                $("#lockStock").text(data.data.saleProductModel.lockStock);
                $("#order_product_orginPrice").val(data.data.orginPrice);
                $("#order_product_price").val(data.data.price);
                $("#order_product_quantity").val(data.data.quantity); 
                $("#order_product_sumprice").val(data.data.sumPrice);
                f = Math.round(parseFloat((data.data.price / data.data.orginPrice)) * 100) / 100;
                $("#discount").val(f);
                $("#stack2").modal();
                $("#order_product_ok").attr("onclick", "submitOrderProduct('" + id + "')");
            }
        }
    })
}
var deleteOrderProduct = function (id) {
    var isCreator = $("#isCreator").val();
    if (isCreator == "false") {
        alertWarn("当前账号不是创建人，没有权限删除订单商品");
        return false;
    }
    $oms.ajax({
        url: "/B2BOrder/DeleteOrderProductById",
        data: { id: id },
        success: function (data) {
            if (data.isSucc) {
                alertSuccess("删除成功");
                showOrderProducts();
            }
        }
    })
}
var calculationPrice = function () {
    var discount = parseFloat($("#discount").val());
    var orginPrice = parseFloat($("#order_product_orginPrice").val());
    var price = parseFloat(discount * orginPrice);
    f = Math.round(price * 100) / 100;
    if (f > 0) {
        $("#order_product_price").val(f);
    }

}
var submitOrderProduct = function (id) {
    var orderProductId = id || 0;
    var orderId = $("#orderId").val();
    var saleProductId = $("#order_product_id").val();
    var quantity = $("#order_product_quantity").val();
    var orginPrice = $("#order_product_orginPrice").val();
    var sumPrice = $("#order_product_sumprice").val();
    var price = $("#order_product_price").val();
    var OrderProductModel = {
        OrderId: orderId,
        SaleProductId: saleProductId,
        Quantity: quantity,
        OrginPrice: orginPrice,
        SumPrice: sumPrice,
        Price: price,
    }
    if (orginPrice == 0) {
        alertWarn("请确认该渠道下商品原价");
        return false;
    }
    if (quantity == "") {
        alertWarn("数量不能为空");
        return false;
    }
    if (price == "") {
        alertWarn("价格不能为空");
        return false;
    }
    if (sumPrice != price * quantity) {
        alertWarn("总金额错误");
        return false;
    }
    var availableStock = $("#availableStock").text();
    if (availableStock == 0) {
        alertWarn("请确认商品剩余可售库存！");
        return false;
    }
    if (orderProductId == 0) {
        $oms.ajax({
            url: "/B2BOrder/AddB2BOrderProduct",
            data: { OrderProductModel: OrderProductModel },
            success: function (data) {
                if (data.isSucc) {
                    alertSuccess("添加成功");
                    $("#stack2").modal("hide");
                    $("#productsModal").modal("hide");
                    $(".order-product").removeClass("display-hide");
                    showOrderProducts(orderId);
                }
            }
        });
    }
    else {
        $oms.ajax({
            url: "/B2BOrder/UpdateOrderSaleProduct",
            data: { OrderProductModel: OrderProductModel },
            success: function (data) {
                if (data.isSucc) {
                    alertSuccess("修改成功");
                    $("#stack2").modal("hide");
                    showOrderProducts(orderId);
                }
            }
        });
    }
}
var approvalOrder = function (e) {
    var orderId = $("#orderId").val();
    var that = e;
    isContinue(function () {
        //通过执行方法
        $oms.ajax({
            url: "/B2BOrder/ApprovalOrder",
            data: { orderId: orderId, state: "true" },
            success: function (data) {
                if (data.isSucc) {
                    alertSuccess("审核成功");
                    $(that).addClass("display-hide");
                }
                else {
                    alertError(data.msg);
                }
            }

        })

    }, function () {
        //回退执行方法
        $oms.ajax({
            url: "/B2BOrder/ApprovalOrder",
            data: { orderId: orderId, state: "false" },
            success: function (data) {
                if (data.isSucc) {
                    alertSuccess("回退成功");
                    $(that).addClass("display-hide");
                }
                else {
                    alertError(data.msg);
                }
            }

        })

    }, "审核当前订单！", "退回", "通过", "red", "green");
}
var calculationSumPrice = function (e) {
    var price = parseFloat($("#order_product_price").val().trim());
    if (price > 0) {
        var q = parseFloat($(e).val().trim());
        if (q > 0) {
            var sumPrice = price * q;
            f = Math.round(sumPrice * 100) / 100;
            $("#order_product_sumprice").val(f);
        }
    }
}
var confirm = function (e) {
    var orderId = $("#orderId").val();
    var that = e;
    isContinue(function () {
        //通过执行方法
        $oms.ajax({
            url: "/B2BOrder/ConfirmOrder",
            data: { orderId: orderId, state: "true" },
            success: function (data) {
                if (data.isSucc) {
                    alertSuccess("确认成功");
                    $(that).addClass("display-hide");
                }
                else {
                    alertError(data.msg);
                }
            }

        })
    }, null, "确认当前订单！", null, "确认", null, null);
}
var submitApproval = function (e) {
    var orderId = $("#orderId").val();
    var that = e;
    isContinue(function () {
        //通过执行方法
        $oms.ajax({
            url: "/B2BOrder/SubmitApproval",
            data: { orderId: orderId},
            success: function (data) {
                if (data.isSucc) {
                    alertSuccess("提交成功");
                    $(that).addClass("display-hide");
                }
                else {
                    alertError(data.msg);
                }
            }

        })
    }, null, "确认提交审核！", null, "确认", null, null);
}

var bookKeeping = function () {
    var orderId = parseInt($("#orderId").val());
    var payType = parseInt($("#payType").select2("data")[0].id);
    var payMentType = parseInt($("#payMentType").select2("data")[0].id);
    var payPrice = parseInt($("#payPrice").val());
    var isPay = $('#radio1').is(':checked');
    var OrderModel = {
        Id: orderId,
        PayType: payType,
        PayMentType: payMentType,
        PayPrice: payPrice,
        IsPayOrRefund: isPay
    }
    if (!(payType > 0)) {
        alertWarn("请选择支付方式");
        return false;
    }
    if (!(payMentType > 0)) {
        alertWarn("请选择汇款方式！");
        return false;
    }

    if (!(payPrice > 0)) {
        alertWarn("请选择汇款方式！");
        return false;
    }
    var sumPrice = parseInt($("#sumPrice").val());
    var paiedPrice = parseInt($("#paiedPrice").val());
    if (isPay) {
        if (payPrice + paiedPrice > sumPrice) {
            alertWarn("支付金额加上已支付金额大于订单总金额，请核实！");
            return false;
        }
    }
    else {
        if (paiedPrice < payPrice) {
            alertWarn("退款金额大于已支付金额，请核实！");
            return false;
        }
    }
    isContinue(function () {
        //通过执行方法
        $oms.ajax({
            url: "/B2BOrder/BookKeeping",
            data: { OrderModel: OrderModel},
            success: function (data) {
                if (data.isSucc) {
                    alertSuccess("操作成功");
                    if (isPay) {
                        $("#paiedPrice").val(payPrice + paiedPrice);
                    }
                    else {
                        $("#paiedPrice").val(paiedPrice - payPrice);
                    }
                }
                else {
                    alertError(data.msg);
                }
            }

        })
    }, null, "对当前订单进行记账！", null, "确认", null, null);
}
var getDefaultInvoiceInfo = function (e) {
    var orderId = parseInt($("#orderId").val());
    var isDefault = $(e).is(':checked');
    if (isDefault) {
        $oms.ajax({
            url: "/B2BOrder/GetDefaultInvoiceInfo",
            data: { orderId: orderId },
            success: function (data) {
                if (data.isSucc) {
                    $("#e_mail").val(data.data.customerEmail);
                    $("#title").val(data.data.title);
                    $("#taxpayerID").val(data.data.taxpayerId);
                    $("#registerAddress").val(data.data.registerAddress);
                    $("#registerTel").val(data.data.registerTel);
                    $("#bankOfDeposit").val(data.data.bankOfDeposit);
                    $("#bankAccount").val(data.data.bankAccount);
                }
            }

        })
    }
}
var submitOrderInvoiceInfo = function () {

    var invoiceType = parseInt($("#invoiceType").select2("data")[0].id);//发票类型
    if (invoiceType == 0) {
        $("#invoiceInfo").modal("hide");
        return false;
    }
    var customerEmail = $("#e_mail").val().trim();
    var title = $("#title").val().trim();
    var taxpayerId = $("#taxpayerID").val().trim();
    var registerAddress = $("#registerAddress").val().trim();
    var registerTel = $("#registerTel").val().trim();
    var bankOfDeposit = $("#bankOfDeposit").val().trim();
    var bankAccount = $("#bankAccount").val().trim();
    if (customerEmail == "") {
        alertWarn("邮箱不能为空！");
        return false;
    }
    var reg_mail = /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+(.[a-zA-Z0-9_-])+/;
    if (!reg_mail.test(customerEmail))
    {
        alertWarn("邮箱有误，请核实！");
        return false;
    }
    if (title == "") {
        alertWarn("发票抬头不能为空！");
        return false;
    }
    if (invoiceType != 1) {
        if (taxpayerId == "") {
            alertWarn("纳税人识别码不能为空！");
            return false;
        }
        if (invoiceType != 2) {
            if (registerAddress == "") {
                alertWarn("注册地址不能为空！");
                return false;
            }
            if (registerTel == "") {
                alertWarn("注册电话不能为空！");
                return false;
            }
            if (bankOfDeposit == "") {
                alertWarn("开户银行不能为空！");
                return false;
            }
            if (bankAccount == "") {
                alertWarn("银行账号不能为空！");
                return false;
            }
            var reg = /^(0|86|17951)?(13[0-9]|15[012356789]|17[01678]|18[0-9]|14[57])[0-9]{8}$/;
            var reg2 = /^(0[0-9]{2,3}\-)([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$/;
            if (!reg.test(mobile) && !reg2.test(mobile)) {
                alertWarn("电话有误请核实！");
                return false;
            }
        }

    }
    var InvoiceInfoModel = {
        CustomerEmail: customerEmail,
        Title: title,
        TaxpayerID: taxpayerId,
        RegisterAddress: registerAddress,
        RegisterTel: registerTel,
        BankOfDeposit: bankOfDeposit,
        BankAccount: bankAccount
    }
    var orderId = parseInt($("#orderId").val());
    $oms.ajax({
        url: "/B2BOrder/SubmitOrderInvoiceInfo",
        data: { invoiceInfoModel: InvoiceInfoModel, orderId: orderId},
        success: function (data) {
            if (data.isSucc) {
                alertSuccess("保存成功");
                $("#invoiceInfo").modal("hide");
            }
        }

    })
}