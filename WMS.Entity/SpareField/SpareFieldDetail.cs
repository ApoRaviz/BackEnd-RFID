using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WIM.Core.Entity;

namespace WMS.Entity.SpareField
{
    [Table("SpareFieldDetails")]
    public class SpareFieldDetail : BaseEntity
    {
    [Key]
	public int SpfdIDSys { get; set; }
	public int SpfdRefID { get; set; }
	public int SpfIDSys { get; set; }
	public string Value { get; set; }
	public string Type { get; set; }
    }
}
