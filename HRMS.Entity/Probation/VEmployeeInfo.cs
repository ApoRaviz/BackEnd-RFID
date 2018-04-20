using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WIM.Core.Entity;

namespace HRMS.Entity.Probation
{
    [Table("VEmployeeInfos")]
    public class VEmployeeInfo : BaseEntity
    {
        [Key]
        public int PersonIDSys { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string NameEn { get; set; }
        public string SurnameEn { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string IdentificationNo { get; set; }
        public string PassportNo { get; set; }
        public string Religion { get; set; }
        public string Nationality { get; set; }
        public string TaxNo { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string EmID { get; set; }
        public int? DepIDSys { get; set; }
        public string DepartmentAcronym { get; set; }
        public string DepName { get; set; }
        public string DepNameEn { get; set; }
        public int? PositionIDSys { get; set; }
        public int? PositionTypeIDSys { get; set; }
        public string PositionAcronym { get; set; }
        public string PositionName { get; set; }
        public string PositionNameEn { get; set; }
        public int EmTypeIDSys { get; set; }
        public string EmTypeName { get; set; }
        public string EmTypeNameEn { get; set; }
        public DateTime? HiredDate { get; set; }
        public DateTime? CompletionOfProbation { get; set; } 
        public string TelOffice { get; set; }
        public string TelEx { get; set; }


    }

}
