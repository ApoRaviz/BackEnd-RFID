using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Validation;
using Fuji.Service.ProgramVersion;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using Fuji.Context;
using Fuji.Entity.ProgramVersion;
using Fuji.Repository.Impl.ProgramVersion;
using System.Security.Principal;

namespace Fuji.Service.Impl.ProgramVersion
{
    public class ProgramVersionService : WIM.Core.Service.Impl.Service,IProgramVersionService
    {

        public ProgramVersionService()
        {
        }        

        public ProgramVersionHistory GetProgramVersion(string programName)
        {
            ProgramVersionHistory item;
            using (FujiDbContext Db = new FujiDbContext())
            {
                item = (from v in Db.ProgramVersionHistory where v.ProgramName == programName select v).OrderByDescending(v => v.ID).Take(1).FirstOrDefault();
            }
            return item;
                
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
