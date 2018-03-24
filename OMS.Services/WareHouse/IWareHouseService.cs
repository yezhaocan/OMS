using OMS.Data.Domain;
using System.Collections.Generic;

namespace OMS.Services
{
   public interface IWareHouseService
    {
        WareHouse GetById(int id);
        bool GetCountByName(string name);
        bool Add(WareHouse customer);
        List<WareHouse> GetAllWareHouseList();
        void UpdateWareHouse(WareHouse WareHouses);
        bool DelWareHouseById(int id);
    }
}
