using System.Collections.Generic;
using OMS.Data.Domain;
using OMS.Data.Interface;
using System.Linq;
using OMS.Core;

namespace OMS.Services.Common
{
    public class CommonService:ServiceBase, ICommonService
    {
        #region ctor
        public CommonService(IDbAccessor omsAccessor, IWorkContext workContext)
            : base(omsAccessor, workContext)
        {

        }
        #endregion

        public List<Dictionary> GetBaseDictionaryList(DictionaryType t)
        {
            return _omsAccessor.Get<Dictionary>().Where(p => p.Type == t && p.Isvalid).ToList();
        }

        public List<Dictionary> GetAllDictionarys()
        {
            return _omsAccessor.Get<Dictionary>().Where(p => p.Isvalid).ToList();
        }
    }
}
