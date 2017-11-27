using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;

namespace WIM.Core.Common.Helpers
{
    public class ApiHashTableHelper
    {
        public static Hashtable apiTable;

        public static void Initialize()
        {
            using (CoreDbContext db = new CoreDbContext())
            {
                apiTable = new Hashtable();

                // #JobComment
                var api = (from row in db.Api_MT
                           select row).ToList();

                foreach (var a in api)
                {
                    apiTable.Add(a.ApiIDSys, a.Api);
                }
            }
        }
    }
}
