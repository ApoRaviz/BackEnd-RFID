﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Person;

namespace WIM.Core.Common.ValueObject
{
   public class UserDto
    {
        public UserDto()
        {
            this.Customer_MT = new HashSet<CustomerDto>();
        }
        public string UserID { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }

        public virtual ICollection<CustomerDto> Customer_MT { get; set; }
        public virtual PersonDto Person_MT { get; set; }
    }
}
