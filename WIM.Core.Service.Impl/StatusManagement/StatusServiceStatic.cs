using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Service.Impl.StatusManagement
{
    public static class StatusServiceStatic
    {
        public static StatusService GetStatusClass()
        {
            return new StatusService();
        }

        public static IEnumerable<string> GetStatusBySubmoduleName(string submoduleName)
        {
            return GetStatusClass().GetStatusBySubmoduleName(submoduleName);
        }

        public static string GetStatusBySubmoduleNameAndStatusTitle<T>(string submoduleName, T item)
        {
            return GetStatusClass().GetStatusBySubmoduleNameAndStatusTitle<T>(submoduleName, item);
        }

        public static string GetStatusBySubmoduleIDAndStatusTitle<T>(int submoduleIDSys, T item)
        {
            return GetStatusClass().GetStatusBySubmoduleIDSysAndStatusTitle<T>(submoduleIDSys, item);
        }
    }
}
