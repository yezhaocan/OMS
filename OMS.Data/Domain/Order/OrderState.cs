using System;
using System.Collections.Generic;
using System.Text;

namespace OMS.Data.Domain
{
    public enum OrderState
    {
        /// <summary>
        /// 待转单（B2C）
        /// </summary>
        ToBeTurned = 0,
        /// <summary>
        /// 待确认（B2B待审核）
        /// </summary>
        ToBeConfirmed = 1,
        /// <summary>
        /// 已确认（B2B已审核）
        /// </summary>
        Confirmed = 2,
        /// <summary>
        /// 财务确认（B2B,已确认）
        /// </summary>
        FinancialConfirmation =3,
        /// <summary>
        /// 被退回（B2B被退回）
        /// </summary>
        returned=4
    }
}
