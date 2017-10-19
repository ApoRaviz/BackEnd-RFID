using Fuji.Entity.ItemManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.ValueObject
{
    public class FujiDataImportSerialHead
    {
        public FujiDataImportSerialHead(int totalRecord, IEnumerable<ImportSerialHead> items)
        {
            this.TotalRecord = totalRecord;
            this.Items = items;
        }
        public int TotalRecord { get; set; }
        public IEnumerable<ImportSerialHead> Items { get; set; }
    }
}
