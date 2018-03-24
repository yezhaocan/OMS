using OMS.Core;
using OMS.Data.Domain;
using System.Collections.Generic;
using System.Linq;

namespace OMS.Services.Account
{
    public interface IUserService
    {
        User GetById(int id);
        User GetByUserName(string userName);
        User GetUserByName(string name);
        void Update(User user);
        IQueryable<User> GetAllUserList();
        IPageList<User> GetUsersByPage(string searchValue, int pageIndex, int pageSize);
        User CreateUser(User user);
        User UpdateUser(User user);
        /// <summary>
        /// 软删除用户
        /// </summary>
        /// <param name="users"></param>
        void SoftDeleteUserRange(List<User> users);

    }
}