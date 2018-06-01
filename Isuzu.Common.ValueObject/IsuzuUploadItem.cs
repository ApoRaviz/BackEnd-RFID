using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isuzu.Common.ValueObject
{
    public class IsuzuUploadItem
    {
        public int FileIDSys { get; set; }
        public string FileName { get; set; }
        public string LocalName { get; set; }
        public string PathFile { get; set; }
        public string FileRefID { get; set; }
    }
}
