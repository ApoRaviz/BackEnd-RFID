﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.ProjectManagement;

namespace WIM.Core.Common.ValueObject
{
    public class RoleUserDto
    {
        public RoleUserDto()
        {
            Project_MT = new ProjectDto();
        }

        public string RoleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSysAdmin { get; set; }
        public ProjectDto Project_MT { get; set; }
        public List<UserRoleDto> Users { get; set; }
    }
}
