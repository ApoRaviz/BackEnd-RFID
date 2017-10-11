﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Entity.LayoutManagement
{
    [Table("LabelLayoutDetail_MT")]
    public class LabelLayoutDetail_MT
    {
        [Key]
        public int LabelIDSys { get; set; }
        public string Label_Code { get; set; }
        public string Label_Type { get; set; }
        public string Label_From { get; set; }
        public string Label_Item { get; set; }
        public string Font_Name { get; set; }
        public Nullable<int> Font_Size { get; set; }
        public Nullable<int> Label_Top { get; set; }
        public Nullable<int> Label_Left { get; set; }
        public Nullable<int> Label_Width { get; set; }
        public Nullable<int> Label_Height { get; set; }
        public string Label_BarcodeType { get; set; }
        public string Label_Text { get; set; }
        public Nullable<int> PxPerInch_Ratio { get; set; }

        public virtual LabelLayoutHeader_MT LabelLayoutHeader_MT { get; set; }
    }
}
