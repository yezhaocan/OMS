﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OMS.Model
{
    public class InvoiceInfoModel:ModelBase
    {
        public int OrderId { get; set; }
        public string CustomerEmail { get; set; }
        public string Title { get; set; }
        public string TaxpayerID { get; set; }
        public string RegisterAddress { get; set; }
        public string RegisterTel { get; set; }
        public string BankOfDeposit { get; set; }
        public string BankAccount { get; set; }
        public string InvoiceNo { get; set; }
    }
}
