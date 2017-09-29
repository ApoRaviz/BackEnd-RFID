using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Repository;
using WIM.Core.Common.Validation;
using Fuji.Repository;
using Fuji.Service.PrintLabel;
using WIM.Core.Repository.Impl;

namespace Fuji.Service.Impl.PrintLabel
{
    public class PrintLabelService : IPrintLabelService
    {
        private WIM_FUJI_DEVEntities db = new WIM_FUJI_DEVEntities();
        
        private IGenericRepository<LabelRunning> repo;

        public PrintLabelService()
        {
            repo = new GenericRepository<LabelRunning>(db);
        }        

        public int GetRunningByType(string type, int running)
        {
            int baseRunning = 0;
            bool isNotUpdateDate;
            LabelRunning label = (from l in db.LabelRunning
                         where l.Type.Equals(type)
                         select l
                         ).SingleOrDefault();

            if (isNotUpdateDate = (label.CreateDate.Date == DateTime.Now.Date))
            {
                baseRunning = label.Running;
            }
            else
            {
                baseRunning = 0;
            }            

            UpdateRunning(label, running, isNotUpdateDate);

            return baseRunning;            
        }

        private bool UpdateRunning(LabelRunning label, int running, bool isNotUpdateDate)
        {
            using (var scope = new TransactionScope())
            {
                if (isNotUpdateDate)
                {
                    label.Running += running;
                }
                else
                {
                    label.CreateDate = DateTime.Now.Date;
                    label.Running = running;
                }                

                repo.Update(label);
                
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                scope.Complete();
                return true;
            }
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
