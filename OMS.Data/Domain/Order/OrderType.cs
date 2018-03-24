using System;
using System.Collections.Generic;
using System.Text;

namespace OMS.Data.Domain
{
    public enum OrderType 
    {
        /// <summary>
        /// b2b订单
        /// </summary>
        B2B=1,
        /// <summary>
        /// B2C现货
        /// </summary>
        B2C_XH=2,
        /// <summary>
        /// B2C跨境
        /// </summary>
        B2C_KJ=3,
        /// <summary>
        /// B2C期酒
        /// </summary>
        B2C_QJ=4,
        /// <summary>
        /// B2C合作商
        /// </summary>
        B2C_HZS=5
    }
}
