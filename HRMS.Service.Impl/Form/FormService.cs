using HRMS.Context;
using HRMS.Entity.Evaluate;
using HRMS.Entity.Form;
using HRMS.Repository.Evaluate;
using HRMS.Repository.Form;
using HRMS.Repository.Impl;
using HRMS.Service.Form;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Transactions;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.Utility.Validation;

namespace HRMS.Service.Impl.Form
{
    public class FormService : WIM.Core.Service.Impl.Service, IFormService
    {
        public FormService()
        {
        }

        public IEnumerable<FormQuestion> GetFormQuestion()
        {
            IEnumerable<FormQuestion> FormQuestion;
            using (HRMSDbContext Db = new HRMSDbContext())
            {
                IFormQuestionRepository repo = new FormQuestionRepository(Db);
                FormQuestion = repo.Get();
            }
            return FormQuestion;
        }

        public IEnumerable<FormDetail> GetFormDetailByEvaID(int id)
        {
            IEnumerable<FormDetail> FormDetail;
            using (HRMSDbContext Db = new HRMSDbContext())
            {
                IFormDetailRepository repo = new FormDetailRepository(Db);
                FormDetail = repo.Get1(id);
            }
            return FormDetail;
        }

        public IEnumerable<object> GetEvaluated()
        {
            IEnumerable<object> Evaluated;
            using (HRMSDbContext Db = new HRMSDbContext())
            {
                IEvaluatedRepository repo = new EvaluatedRepository(Db);
                Evaluated = repo.GetList();
            }
            return Evaluated;
        }

        public Evaluated GetEvaluatedByEvaID(int id)
        {
            Evaluated Evaluated;
            using (HRMSDbContext Db = new HRMSDbContext())
            {
                IEvaluatedRepository repo = new EvaluatedRepository(Db);
                Evaluated = repo.Get(c => c.EvaluatedIDSys == id);
            }
            return Evaluated;
        }


        public bool UpdateFormDetail(IEnumerable<FormDetail> formDetail)
        {


            using (var scope = new TransactionScope())
            {

                try
                {

                    using (HRMSDbContext Db = new HRMSDbContext())
                    {
                        IFormDetailRepository repo = new FormDetailRepository(Db);
                        foreach (var x in formDetail)
                        {
                            if (x.FormDetailIDSys == 0)
                            {
                                repo.Insert(x);
                            }
                            else
                            {
                                repo.Update(x);
                            }
                        }

                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                    throw ex;
                }
            }
            return true;
        }

        public Evaluated UpdateEvaluated(Evaluated evaluated)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (HRMSDbContext Db = new HRMSDbContext())
                    {
                        IEvaluatedRepository repo = new EvaluatedRepository(Db);
                        if (evaluated.EvaluatedIDSys == 0)
                        {
                            evaluated = repo.Insert(evaluated);
                        }
                        else
                        {
                            repo.Update(evaluated);
                        }
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new ValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    throw new ValidationException(ErrorEnum.E4012);
                }
                return evaluated;
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
