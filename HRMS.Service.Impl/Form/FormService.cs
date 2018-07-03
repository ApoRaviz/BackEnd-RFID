using HRMS.Common.ValueObject;
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
using System.Net.Http;
using System.Transactions;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.Utility.Validation;
using Microsoft.Reporting.WebForms;
using System.IO;
using HRMS.Common.ValueObject.ReportEvaluation;

namespace HRMS.Service.Impl.Form
{
    public class FormService : WIM.Core.Service.Impl.Service, IFormService
    {
        public FormService()
        {
        }

        public IEnumerable<FormQuestion> GetFormQuestionByFormTopicID(int id)
        {
            IEnumerable<FormQuestion> FormQuestion;
            using (HRMSDbContext Db = new HRMSDbContext())
            {
                IFormQuestionRepository repo = new FormQuestionRepository(Db);
                FormQuestion = repo.GetFormQByFormTopicID(id);
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

        public IEnumerable<EvaluationTable> GetEvaluatedFormDetailByID(int id)
        {
            IEnumerable<EvaluationTable> EvaluatedFormDetail;
            using (HRMSDbContext Db = new HRMSDbContext())
            {
                IEvaluatedRepository repo = new EvaluatedRepository(Db);
                EvaluatedFormDetail = repo.GetFormDetailList(id);
            }
            return EvaluatedFormDetail;
        }
        public IEnumerable<EvaluatedReport> GetEvaluatedFormByID(int id)
        {
            IEnumerable<EvaluatedReport> EvaluatedForm;
            using (HRMSDbContext Db = new HRMSDbContext())
            {
                IEvaluatedRepository repo = new EvaluatedRepository(Db);
                EvaluatedForm = repo.GetFormReportList(id);
            }
            return EvaluatedForm;
        }
        public StreamContent GetReportStream(IEnumerable<EvaluatedReport> item1, IEnumerable<EvaluationTable> item2)
        {
            byte[] bytes;
            Warning[] warnings;
            string[] streamids;
            string mimeType, encoding, extension;

            //foreach (var item in item1)
            //{
            //    item.ValueOld = item.ValueArr[0];
            //    item.ValueNew = item.ValueArr[1];
            //}



            
 






            using (var reportViewer = new ReportViewer())
            {
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = "Report/EvaluationFormHeader.rdlc";
                reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(localReport_SubreportProcessing);

                reportViewer.LocalReport.Refresh();
                reportViewer.LocalReport.EnableExternalImages = true;




                ReportDataSource rds1 = new ReportDataSource
                {
                    Name = "evaluatedReport",
                    Value = item1
                };

                ReportDataSource rds3 = new ReportDataSource
                {
                    Name = "eva",
                    Value = item2
                };



                void localReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)


                {
                    ReportDataSource rds2 = new ReportDataSource
                    {
                        Name = "eva",
                        Value = item2
                    };


                    e.DataSources.Add(rds2);


                }


                reportViewer.LocalReport.DataSources.Add(rds1);
                reportViewer.LocalReport.DataSources.Add(rds3);


                bytes = reportViewer.LocalReport.Render("Pdf", null, out mimeType, out encoding, out extension, out streamids, out warnings);

            }

            Stream stream = new MemoryStream(bytes);
            return new StreamContent(stream);

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
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
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
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
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
                    throw new AppValidationException(ve.PropertyName, ve.ErrorMessage);
                }
            }
        }
    }

}
