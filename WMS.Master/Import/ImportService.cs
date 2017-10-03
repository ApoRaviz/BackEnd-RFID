using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Helpers;
using WIM.Core.Common.Validation;
using WMS.Repository;

namespace WMS.Master.Import
{
    public class ImportService : IImportService
    {
        string pXmlDetail = "<row><ColumnName>{0}</ColumnName><Digits>{1}</Digits><DataType>{2}</DataType>" +
                            "<Mandatory>{3}</Mandatory><FixedValue>{4}</FixedValue>" +
                            "<Import>{5}</Import></row>";

        private MasterContext Db = MasterContext.Create();
        private GenericRepository<ImportDefinitionHeader_MT> repo;

        public ImportService()
        {
            repo = new GenericRepository<ImportDefinitionHeader_MT>(Db);
        }

        public List<ImportDefinitionHeader_MT> GetAllImportHeader(string forTable)
        {
            List<ImportDefinitionHeader_MT> import = Db.ImportDefinitionHeader_MT.Where(x => x.ForTable == forTable).ToList();
            return import;
        }

        public ImportDefinitionHeader_MT GetImportDefinitionByImportIDSys(int id, string include)
        {
            ImportDefinitionHeader_MT import = Db.ImportDefinitionHeader_MT.Find(id);
            if (string.IsNullOrEmpty(include))
            {
                return import;
            }

            string[] includes = include.Replace(" ", "").Split(',');
            foreach (string inc in includes)
            {
                Db.Entry(import).Collection(inc).Load();
            }

            return import;
        }

        public int? CreateImportDifinitionForItemMaster(ImportDefinitionHeader_MT data)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            int? ReportSysID = 0;

            foreach (ImportDefinitionDetail_MT d in data.detail)
                sb.AppendFormat(pXmlDetail, d.ColumnName, d.Digits, d.DataType, d.Mandatory, d.FixedValue, d.Import);

            using (var scope = new TransactionScope())
            {
                data.CreatedDate = DateTime.Now;
                data.UpdatedDate = DateTime.Now;
                data.UserUpdate = "1";

                //Repo.Insert(customer);
                try
                {
                    ReportSysID = Db.ProcCreateImportDefinition(data.ForTable, data.FormatName, data.Delimiter, data.MaxHeading, data.Encoding, data.SkipFirstRecode
                                              , data.CreatedDate, data.UpdatedDate, data.UserUpdate, sb.ToString()).FirstOrDefault();
                    Db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException e)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                scope.Complete();
                return ReportSysID;
            }
        }

        public bool UpdateImportForItemMaster(int ImportIDSys, ImportDefinitionHeader_MT data)
        {
            System.Text.StringBuilder sb = new StringBuilder();

            foreach (ImportDefinitionDetail_MT d in data.detail)
                sb.AppendFormat(pXmlDetail, d.ColumnName, d.Digits, d.DataType, d.Mandatory, d.FixedValue, d.Import);

            using (var scope = new TransactionScope())
            {
                data.UpdatedDate = DateTime.Now;
                data.UserUpdate = "1";

                try
                {
                    Db.ProcUpdateImportDefinition(data.ImportIDSys, data.FormatName, data.Delimiter, data.MaxHeading
                                              , data.Encoding, data.SkipFirstRecode, data.CreatedDate, data.UpdatedDate, data.UserUpdate, sb.ToString());
                    Db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException e)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
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

        public string ImportDataToTable(int ImportIDSys, string data)
        {
            string result = "";

            using (var scope = new TransactionScope())
            {
                try
                {
                    result = Db.ProcImportDataToTable(ImportIDSys, DateTime.Now, DateTime.Now, "1", data).FirstOrDefault();
                    Db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException e)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                scope.Complete();
                return result;
            }
        }

        public void InsertImportHistory(int ImportIDSys, string fileName, string result, bool success, string user)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    Db.ProcInsertImportHistory(ImportIDSys, fileName, result, success, DateTime.Now, user);
                    Db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException e)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                scope.Complete();
            }
        }

        public bool DeleteImport(int ImportIDSys)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    Db.ProcDeleteImportDefinition(ImportIDSys);
                    Db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateConcurrencyException e)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
                    throw ex;
                }

                scope.Complete();
            }

            return true;
        }
    }
}
