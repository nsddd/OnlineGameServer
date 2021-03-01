using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Tools
{
    public class DictTool
    {
        public static Tvalue GetValue<Tkey, Tvalue>(Dictionary<Tkey, Tvalue> dict, Tkey key)
        {
            Tvalue value;
            bool isSuccess = dict.TryGetValue(key, out value);
            if (isSuccess)
            {
                return value;
            }
            else
            {
                return default(Tvalue);
            }
        }

    }
}
