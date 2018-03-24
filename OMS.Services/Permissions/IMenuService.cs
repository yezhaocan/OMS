using OMS.Data.Domain.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMS.Services.Permissions
{
    public interface IMenuService
    {
        IQueryable<Menu> GetMenusByUserId(int userId);
        Menu GetMenuById(int menuId);
        IQueryable<Menu> GetBaseMenus();
        IQueryable<Menu> GetChildMenus(int parentId);
        void AddMenu(Menu menu);
        void UpdateMenu(Menu menu);
        IQueryable<Menu> GetMenus();
        Menu GetMenuByName(string name);
        Menu GetMenuByCode(string code);
        Menu GetMenuByUrl(string url);

         
    }
}
