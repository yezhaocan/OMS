using System;
using System.Collections.Generic;
using System.Text;

namespace OMS.Data.Domain
{
    public class Dictionary:EntityBase
    {
        public DictionaryType Type { get; set; }
        public string Value { get; set; }
    }
}
