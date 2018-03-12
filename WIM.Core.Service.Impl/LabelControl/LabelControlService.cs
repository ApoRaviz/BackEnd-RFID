using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.ValueObject;
using WIM.Core.Context;
using WIM.Core.Entity.LabelManagement;
using WIM.Core.Entity.LabelManagement.LabelConfigs;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;

namespace WIM.Core.Service.Impl
{
    public class LabelControlService : Service, ILabelControlService
    {

        string[] ListLang = { "en", "th", "jp" };
        public LabelControlDto GetDto(string Lang, int ProjectID)
        {
            LabelControlDto label = new LabelControlDto();
            using (CoreDbContext Db = new CoreDbContext())
            {
                ILabelControlRepository repo = new LabelControlRepository(Db);
                label = repo.GetDto(Lang, ProjectID);
            }
            return label;

        }

        public LabelControlDto GetDto(string Lang)
        {
            return null;
        }

        public LabelControlDto CreateLabelControl(LabelControl labelData)
        {
            using (CoreDbContext db = new CoreDbContext())
            {
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        LabelControl labelControl = new LabelControl();
                        ILabelControlRepository repo = new LabelControlRepository(db);
                        foreach (string lang in ListLang)
                        {

                            labelData.Lang = lang;
                            labelControl = repo.Insert(labelData);
                            CreateFileJsonI18n(labelControl.LabelConfig, labelControl.ProjectIDSys + "_" + labelControl.Lang);
                        }
                        db.SaveChanges();
                        scope.Complete();
                        return AutoMapper.Mapper.Map<LabelControl, LabelControlDto>(labelControl);
                    }
                    catch (DbEntityValidationException)
                    {
                        scope.Dispose();
                        ValidationException ex = new ValidationException(ErrorEnum.E4012);
                        throw ex;
                    }
                    catch (DbUpdateException x)
                    {
                        scope.Dispose();
                        ValidationException ex = new ValidationException(ErrorEnum.E4012);
                        throw x;
                    }
                }
            }
        }

        public LabelControlDto UpdateLabelControl(LabelControl labelData)
        {
            using (CoreDbContext db = new CoreDbContext())
            {
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        LabelControl labelControl;
                        ILabelControlRepository repo = new LabelControlRepository(db);
                        labelControl = repo.Update(labelData);
                        db.SaveChanges();
                        scope.Complete();

                        CreateFileJsonI18n(labelControl.LabelConfig, labelControl.ProjectIDSys + "_" + labelControl.Lang);

                        return AutoMapper.Mapper.Map<LabelControl, LabelControlDto>(labelControl);
                    }

                    catch (DbEntityValidationException)
                    {
                        scope.Dispose();
                        ValidationException ex = new ValidationException(ErrorEnum.E4012);
                        throw ex;
                    }
                    catch (DbUpdateException x)
                    {
                        scope.Dispose();
                        ValidationException ex = new ValidationException(ErrorEnum.E4012);
                        throw x;
                    }
                }
            }
        }

        public void CreateFileJsonI18n(List<LabelConfig> LabelConfig, string FileName)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            Dictionary<string, string> projectConfig = LabelConfig.ToDictionary(obj => obj.Key, obj => obj.Value);
            String rootPath = HttpContext.Current.Server.MapPath(@"~\Results\i18n\lang\");
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            using (StreamWriter sw = new StreamWriter(rootPath + FileName + ".json"))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, projectConfig);
                }
            }

        }

        public bool AddLabelConfig(int ProjectIDSys, List<LabelConfig> LabelConfig)
        {
            using (CoreDbContext db = new CoreDbContext())
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    try
                    {

                        LabelControl labelControl = new LabelControl();
                        ILabelControlRepository repo = new LabelControlRepository(db);
                        foreach (string lang in ListLang)
                        {
                            labelControl = repo.Get(x => x.ProjectIDSys == ProjectIDSys && x.Lang == lang);
                            if (labelControl == null)
                            {
                                labelControl = new LabelControl
                                {
                                    ProjectIDSys = ProjectIDSys,
                                    Lang = lang,
                                    LabelConfig = LabelConfig
                                };
                                repo.Insert(labelControl);
                            }
                            else
                            {
                                List<LabelConfig> newLabelConfig = new List<LabelConfig>();
                                newLabelConfig.AddRange(labelControl.LabelConfig);
                                newLabelConfig.AddRange(LabelConfig);
                                labelControl.LabelConfig = newLabelConfig;
                                repo.Update(labelControl);
                            }
                        }
                        db.SaveChanges();
                        scope.Complete();
                        return true;
                    }
                    catch (DbEntityValidationException)
                    {
                        scope.Dispose();
                        ValidationException ex = new ValidationException(ErrorEnum.E4012);
                        throw ex;
                    }
                    catch (DbUpdateException x)
                    {
                        scope.Dispose();
                        ValidationException ex = new ValidationException(ErrorEnum.E4012);
                        throw x;
                    }
                }

            }
        }

        public LabelControlDto DelLabelConfig(int ProjectIDSys, string[] LabelConfig)
        {
            throw new NotImplementedException();
        }
    }
}
