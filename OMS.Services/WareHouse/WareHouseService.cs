using Microsoft.EntityFrameworkCore;
using OMS.Core;
using OMS.Data.Domain;
using OMS.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OMS.Services
{
   public class WareHouseService: ServiceBase,IWareHouseService
    {
        #region ctor
        public WareHouseService(IDbAccessor omsAccessor, IWorkContext workContext)
            : base(omsAccessor, workContext)
        {

        }
        #endregion
        public WareHouse GetById(int id)
        {
            return _omsAccessor.Get<WareHouse>().Where(x => x.Isvalid && x.Id == id).FirstOrDefault();
        }
        public bool GetCountByName(string name)
        {
           IQueryable<WareHouse> count= _omsAccessor.Get<WareHouse>().Where(x => x.Name.Equals(name));
            if (count.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Add(WareHouse wareHouse)
        {
            if (wareHouse == null)
                throw new ArgumentException("WareHouse");
            wareHouse.Isvalid = true;
            wareHouse.CreatedBy = _workContext.CurrentUser.Id;
            wareHouse.ModifiedTime = DateTime.Now;
            wareHouse.CreatedTime = DateTime.Now;
            _omsAccessor.Insert(wareHouse);
            _omsAccessor.SaveChanges();
            return true;
        }
        public List<WareHouse> GetAllWareHouseList()
        {
            return _omsAccessor.Get<WareHouse>().Where(x => x.Isvalid).ToList();
        }
        public void UpdateWareHouse(WareHouse wareHouses)
        {
            if (wareHouses == null)
                throw new ArgumentException("wareHouses");
            wareHouses.ModifiedBy = _workContext.CurrentUser.Id;
            _omsAccessor.Update(wareHouses);
            _omsAccessor.SaveChanges();
        }
       public bool DelWareHouseById(int id)
        {
            var delData = _omsAccessor.Get<WareHouse>().Where(x => x.Isvalid && x.Id == id).FirstOrDefault();
            if (delData == null)
                throw new ArgumentException("WareHouse");
            else
            _omsAccessor.DeleteById<WareHouse>(id);
            _omsAccessor.SaveChanges();
            return true;
        }
    }
}
