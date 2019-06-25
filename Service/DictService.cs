using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
  public  class DictService
    {
      public static string GetDictValue(string code, string parentCode)
      {
          var dict= CacheService.DictList.Where(c => c.ParentCode == parentCode && c.Code == code).FirstOrDefault();
          if (dict != null)
              return dict.Name;
          else
              return code;
      }
    }
}
