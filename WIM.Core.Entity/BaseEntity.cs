using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            IsActive = true;
            CreateAt = DateTime.Now;
            UpdateAt = DateTime.Now;
        }

        public Nullable<bool> IsActive { get; set; }
        public string CreateBy { get; set; }
        public Nullable<DateTime> CreateAt { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateAt { get; set; }
    }
}
