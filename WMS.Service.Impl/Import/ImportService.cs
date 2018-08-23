using Master.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Helpers;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.Utility.Validation;
using WMS.Context;
using WMS.Entity.ImportManagement;
using WMS.Repository.Impl.ImportDefinition;
using WMS.Repository.ImportDefinition;
using WMS.Service.Import;

namespace WMS.Service.Impl.Import
{
    public class ImportService : WIM.Core.Service.Impl.Service, IImportService
    {
        string pXmlDetail = "<row><ColumnName>{0}</ColumnName><Digits>{1}</Digits><DataType>{2}</DataType>" +
                            "<Mandatory>{3}</Mandatory><FixedValue>{4}</FixedValue>" +
                            "<Import>{5}</Import><IsHead>{6}</IsHead></row>";

        private IIdentity _identity;
        
        public ImportService()
        {
            _identity =  UtilityHelper.GetIdentity();
        }

        public List<ImportDefinitionHeader_MT> GetAllImportHeader(string forTable)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                IImportDefinitionRepository repo = new ImportDefinitionRepository(Db);
                List<ImportDefinitionHeader_MT> import = repo.GetMany(x => x.ForTable == forTable).ToList();
                return import;
            }

        }

        public ImportDefinitionHeader_MT GetImportDefinitionByImportIDSys(int id, string include)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                IImportDefinitionRepository repo = new ImportDefinitionRepository(Db);
                ImportDefinitionHeader_MT import = repo.GetByID(id);
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
        }

        public int? CreateImportDifinitionForItemMaster(ImportDefinitionHeader_MT data)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            int? ReportSysID = 0;



            foreach (ImportDefinitionDetail_MT d in data.detail)
            {
                d.IsActive = true;
                d.CreateAt = DateTime.Now;
                d.CreateBy = _identity.Name;
                d.UpdateAt = DateTime.Now;
                d.UpdateBy = _identity.Name;
                sb.AppendFormat(pXmlDetail, d.ColumnName, d.Digits, d.DataType, d.Mandatory, d.FixedValue, d.Import, d.IsHead);
            }

            using (var scope = new TransactionScope())
            {
                data.IsActive = true;
                data.CreateAt = DateTime.Now;
                data.CreateBy = _identity.Name;

                using (WMSDbContext Db = new WMSDbContext())
                {
                   
                    try
                    {
                        ReportSysID = Db.ProcCreateImportDefinition(data.ProjectIDSys,data.ForTable, data.FormatName, data.Delimiter, data.MaxHeading, data.Encoding, data.SkipFirstRecode
                                                  , data.CreateAt, data.CreateBy, sb.ToString());
                        Db.SaveChanges();
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
                    scope.Complete();
                    return ReportSysID;
                }
            }
        }

        public bool UpdateImportForItemMaster(int ImportIDSys, ImportDefinitionHeader_MT data)
        {
            System.Text.StringBuilder sb = new StringBuilder();

            foreach (ImportDefinitionDetail_MT d in data.detail)
            {
                d.IsActive = true;
                d.UpdateAt = DateTime.Now;
                d.UpdateBy = _identity.Name;
                sb.AppendFormat(pXmlDetail, d.ColumnName, d.Digits, d.DataType, d.Mandatory, d.FixedValue, d.Import, d.IsHead);
            }

            using (var scope = new TransactionScope())
            {
                data.UpdateAt = DateTime.Now;
                data.UpdateBy = _identity.Name;

                using (WMSDbContext Db = new WMSDbContext())
                {
                    try
                    {
                        Db.ProcUpdateImportDefinition(data.ImportDefHeadIDSys, data.FormatName, data.Delimiter, data.MaxHeading
                                                  , data.Encoding, data.SkipFirstRecode, data.CreateAt, data.UpdateAt, data.UpdateBy, sb.ToString());
                        Db.SaveChanges();
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
                    scope.Complete();
                    return true;
                }
            }
        }

        public string ImportDataToTable(int ImportIDSys, string data, string userUpdate)
        {
            string result = "";

            using (var scope = new TransactionScope())
            {
                using (WMSDbContext Db = new WMSDbContext())
                {
                    try
                    {
                        result = Db.ProcImportDataToTable(ImportIDSys, null, null, userUpdate, data);
                        Db.SaveChanges();
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
                    scope.Complete();
                    return result;
                }
            }
        }

        public void InsertImportHistory(int ImportIDSys, string fileName, string result, bool success, string user)
        {
            using (var scope = new TransactionScope())
            {
                using (WMSDbContext Db = new WMSDbContext())
                {
                    try
                    {

                        Db.ProcInsertImportHistory(ImportIDSys, fileName, result, success, null, user);
                        Db.SaveChanges();

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
                    scope.Complete();
                }
            }
        }

        public bool DeleteImport(int ImportIDSys)
        {
            using (var scope = new TransactionScope())
            {
                using (WMSDbContext Db = new WMSDbContext())
                {
                    
                    try
                    {
                        Db.ProcDeleteImportDefinition(ImportIDSys);
                        Db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new AppValidationException(e);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        scope.Dispose();
                        throw new AppValidationException(ErrorEnum.UPDATE_DATABASE_CONCURRENCY_PROBLEM);
                    }

                    scope.Complete();
                }
            }

            return true;
        }
    }
}
