using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Fuji.Repository;
using WIM.Core.Common.Validation;
using Fuji.Service.ProgramVersion;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;

namespace Fuji.Service.Impl.ProgramVersion
{
    public class ProgramVersionService : IProgramVersionService
    {
        private WIM_FUJI_DEVEntities db = new WIM_FUJI_DEVEntities();
        
        private IGenericRepository<ProgramVersionHistory> repo;

        public ProgramVersionService()
        {
            repo = new GenericRepository<ProgramVersionHistory>(db);
        }        

        public ProgramVersionHistory GetProgramVersion(string programName)
        {
            return (from v in db.ProgramVersionHistory where v.ProgramName == programName select v).OrderByDescending(v => v.ID).Take(1).FirstOrDefault();            
        }

        public void HandleValidationException(DbEntityValidationException ex)
        {
            foreach (var eve in ex.EntityValidationErrors)
            {
                foreach (var ve in eve.ValidationErrors)
                {
                    throw new ValidationException(ve.PropertyName, ve.ErrorMessage);
                }
            }
        }

    }
}
