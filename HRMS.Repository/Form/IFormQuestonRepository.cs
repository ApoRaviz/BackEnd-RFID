using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.Entity.Form;
using WIM.Core.Repository;

namespace HRMS.Repository.Form
{
    public interface IFormQuestionRepository : IRepository<FormQuestion>
    {
        IEnumerable<FormQuestion> GetFormQByFormTopicID(int id);
    }
}
