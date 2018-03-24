using OMS.Core;
using OMS.Data.Interface;

namespace OMS.Services
{
    public abstract class ServiceBase
    {
        protected readonly IDbAccessor _omsAccessor;
        protected readonly IWorkContext _workContext;
        protected ServiceBase(IDbAccessor omsAccessor, IWorkContext workContext)
        {
            _omsAccessor = omsAccessor;
            _workContext = workContext;
        }
    }
}
