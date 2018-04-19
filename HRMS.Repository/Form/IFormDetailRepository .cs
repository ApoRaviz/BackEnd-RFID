using HRMS.Entity.Evaluate;
using HRMS.Entity.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository;

namespace HRMS.Repository.Form
{
    public interface IFormDetailRepository : IRepository<FormDetail>
    {
        IEnumerable<FormDetail> Get1(int id);
    }
}

