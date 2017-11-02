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
    public class ProgramVersionService : IProgramVersionService
    {
        private FujiDbContext Db { get; set; }
        private ProgramVersionRepository programRepo;
        private IIdentity Identity;

        public ProgramVersionService(IIdentity identity)
        {
            Db = FujiDbContext.Create();
            programRepo = new ProgramVersionRepository(new FujiDbContext());
            Identity = identity;
        }        

        public ProgramVersionHistory GetProgramVersion(string programName)
        {
            return (from v in Db.ProgramVersionHistory where v.ProgramName == programName select v).OrderByDescending(v => v.ID).Take(1).FirstOrDefault();            
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
