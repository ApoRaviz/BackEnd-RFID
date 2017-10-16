using Fuji.Entity.ItemManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.ValueObject
{
    public class FujiDataImportSerialDetail
    {
        public FujiDataImportSerialDetail(int totalRecord, IEnumerable<ImportSerialDetail> items)
        {
            this.TotalRecord = totalRecord;
            this.Items = items;
        }
        public int TotalRecord { get; set; }
        public IEnumerable<ImportSerialDetail> Items { get; set; }
    }
}
