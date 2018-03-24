using System;
using System.Collections.Generic;
using System.Text;

namespace OMS.Data.Domain
{
    public enum InvoiceType : Int16
    {
        /// <summary>
        /// 不需要
        /// </summary>
        NoNeedInvoice=0,
        /// <summary>
        /// 个人发票
        /// </summary>
        PersonalInvoice=1,
        /// <summary>
        /// 普通单位发票
        /// </summary>
        CompanyInvoice = 2,
        /// <summary>
        /// 专用发票
        /// </summary>
        SpecialInvoice=3

    }
}
