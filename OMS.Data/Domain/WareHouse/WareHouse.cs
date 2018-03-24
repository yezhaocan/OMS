using System;
using System.Collections.Generic;
using System.Text;

namespace OMS.Data.Domain
{
    public class WareHouse : EntityBase
    {
        public string Name { get; set; }
        public int DistrictId { get; set; }
        public List<Order> Order{ get; set; }
    }
}
