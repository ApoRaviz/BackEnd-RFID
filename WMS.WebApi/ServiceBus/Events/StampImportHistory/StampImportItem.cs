using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WIM.Core.Entity.importManagement;

namespace WMS.WebApi.ServiceBus.Events
{
    public class StampImportItem
    {
        public StampImportItem(int importDefHeadIDSys, string fileName, string result, bool success, string user)
        {
            this.ImportDefHeadIDSys = importDefHeadIDSys;
            this.FileName = fileName;
            this.Result = result;
            this.Success = success;
            this.User = user;
        }
        public int ImportDefHeadIDSys { get; set; }
        public string FileName { get; set; }
        public string Result { get; set; }
        public bool Success { get; set; }
        public string User { get; set; }
    }
}