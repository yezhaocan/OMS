using OMS.Data.Domain;
using System.Collections.Generic;

namespace OMS.Services.Common
{
    public interface ICommonService
    {
        List<Dictionary> GetBaseDictionaryList(DictionaryType t);
        List<Dictionary> GetAllDictionarys();
    }
}
