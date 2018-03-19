using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.FileManagement
{
    [Table("File_MT")]
    public class File_MT : BaseEntity
    {
        [Key]
        public int FileIDSys { get; set; }
        public string FileName { get; set; }
        public string LocalName { get; set; }
        public string PathFile { get; set; }
        public string FileRefID { get; set; }
    }
}
