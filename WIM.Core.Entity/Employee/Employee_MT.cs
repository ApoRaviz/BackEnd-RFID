﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Entity.Employee;
using WIM.Core.Entity.Person;
using WIM.Core.Entity.PositionConfigManagement;

namespace WIM.Core.Entity
{
    [Table("Employee_MT")]
    public class Employee_MT : BaseEntity
    {
        [Key]
        public string EmID { get; set; }
        public int PersonIDSys { get; set; }
        public string Area { get; set; }
        public Nullable<int> DepIDSys { get; set; }
        public string TelOffice { get; set; }
        public string TelEx { get; set; }
        public Nullable<System.DateTime> HiredDate { get; set; }
        public Nullable<System.DateTime> CompletionOfProbation { get; set; }
        public Nullable<int> PositionIDSys { get; set; }
        public Nullable<int> ProbationIDSys { get; set; }
        public Nullable<int> EmTypeIDSys { get; set; }
        public string ConfidentialConfigs { get; set; }


        [ForeignKey("PersonIDSys")]
        public virtual Person_MT Person_MT { get; set; }
        [ForeignKey("DepIDSys")]
        public virtual Departments Departments { get; set; }
        [ForeignKey("PositionIDSys")]
        public virtual Positions Positions { get; set; }
        [ForeignKey("ProbationIDSys")]
        public virtual Probation_MT Probation_MT { get; set; }
        [NotMapped]
        public virtual Resign Resign { get; set; }
        public virtual ICollection<HistoryWarning> HistoryWarnings { get; set; }
        //[NotMapped]
        //public PositionConfigManagement.PositionConfig PositionConfig { get; set; }


        [NotMapped]
        public EmployeeConfidentialConfig EmpConfidentialConfigs {
            get {
                if (!string.IsNullOrEmpty(ConfidentialConfigs))
                {
                    return JsonConvert.DeserializeObject<EmployeeConfidentialConfig>(StringHelper.Decompress(ConfidentialConfigs));
                }
                return null;
            }
            set {
                ConfidentialConfigs = StringHelper.Compress(JsonConvert.SerializeObject(value));
            }
        }
        


    }


    public class EmployeeConfidentialConfig
    {
        public PositionConfigManagement.PositionConfig PositionConfig  { get; set; }

        public EmployeeConfidentialConfig()
        {
            //PositionConfig = new PositionConfigManagement.PositionConfig.PositionConfig();
        }
    }
}

