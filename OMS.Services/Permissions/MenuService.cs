using OMS.Core;
using OMS.Data.Domain;
using OMS.Data.Domain.Permissions;
using OMS.Data.Implementing;
using OMS.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMS.Services.Permissions
{
    public class MenuService : ServiceBase, IMenuService
    {

        #region ctor
        public MenuService(IDbAccessor omsAccessor, IWorkContext workContext)
                    : base(omsAccessor, workContext)
        {
        }       

        #endregion
        public Menu GetMenuById(int menuId)
        {
            return _omsAccessor.Get<Menu>().Where(x => x.Isvalid && x.Id == menuId).FirstOrDefault();
        }

        /// <summary>
        /// 根据用户Id获取菜单列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public IQueryable<Menu> GetMenusByUserId(int userId)
        {

            var menus = from m in _omsAccessor.Get<Menu>()
                        join rm in _omsAccessor.Get<RoleMenu>() on m.Id equals rm.MenuId
                        join r in _omsAccessor.Get<Role>() on rm.RoleId equals r.Id
                        join ur in _omsAccessor.Get<UserRole>() on r.Id equals ur.RoleId
                        join u in _omsAccessor.Get<User>() on ur.UserId equals u.Id
                        where u.Id == userId && m.Isvalid 
                        orderby m.Sort
                        select m;

            return menus;
        }

        /// <summary>
        /// 获取根菜单
        /// </summary>
        /// <returns></returns>
        public IQueryable<Menu> GetBaseMenus()
        {
            return _omsAccessor.Get<Menu>().Where(x => x.Isvalid && x.ParentId == null).OrderBy(t=>t.Sort);
        }

        public void AddMenu(Menu menu)
        {
            menu.CreatedBy = _workContext.CurrentUser.Id;
            menu.CreatedTime = DateTime.Now;
            _omsAccessor.Insert(menu);
            _omsAccessor.SaveChanges();
        }

        public void UpdateMenu(Menu menu)
        {
            menu.ModifiedBy = _workContext.CurrentUser.Id;
            menu.ModifiedTime = DateTime.Now;
            _omsAccessor.Update(menu);
            _omsAccessor.SaveChanges();
        }

        public IQueryable<Menu> GetMenus()
        {
            return _omsAccessor.Get<Menu>().Where(x=>x.Isvalid).OrderBy(t=>t.Sort);
        }

        public Menu GetMenuByName(string name)
        {
            return _omsAccessor.Get<Menu>().Where(x => x.Isvalid && x.Name == name).FirstOrDefault();
        }

        public Menu GetMenuByCode(string code)
        {
            return _omsAccessor.Get<Menu>().Where(x => x.Isvalid && x.Code == code).FirstOrDefault();
        }

        public Menu GetMenuByUrl(string url)
        {
            return _omsAccessor.Get<Menu>().Where(x => x.Isvalid && x.Url == url).FirstOrDefault();
        }

        public IQueryable<Menu> GetChildMenus(int parentId)
        {
            return _omsAccessor.Get<Menu>().Where(x => x.Isvalid && x.ParentId == parentId);
        }
    }
}
