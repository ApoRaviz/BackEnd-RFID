using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using WIM.Core.Common.Helpers;
using WIM.Core.Common.Utility.Attributes;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Context;
using WIM.Core.Entity;
using WIM.Core.Common.Utility.Extentions;
using WIM.Core.Entity.Logs;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.Validation;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Metadata.Edm;

namespace WIM.Core.Repository.Impl
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected DbContext Context;
        internal DbSet<TEntity> DbSet;
        internal DbSet<GeneralLog> GeneralLogDbSet;

        public Repository(DbContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
            GeneralLogDbSet = Context.Set<GeneralLog>();
        }

        public IIdentity Identity
        {
            get
            {
                return AuthHelper.GetIdentity();
            }
        }

        public IEnumerable<TEntity> Get()
        {
            return DbSet.Take(2000).ToList();
        }
        public async Task<IEnumerable<TEntity>> GetAsync()
        {
            return await DbSet.Take(2000).ToListAsync();
        }

        public TEntity Get(Func<TEntity, Boolean> where)
        {
            return DbSet.Where(where).FirstOrDefault<TEntity>();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return DbSet.ToList();
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public TEntity GetByID(params object[] id)
        {
            return DbSet.Find(id);
        }
        public async Task<TEntity> GetByIDAsync(params object[] id)
        {
            return await DbSet.FindAsync(id);
        }

        public TEntity GetByID(object id)
        {
            return DbSet.Find(id);
        }
        public async Task<TEntity> GetByIDAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public bool Exists(object id)
        {
            return DbSet.Any();
        }

        public bool Exists(Func<TEntity, Boolean> where)
        {
            return DbSet.Any(where);
        }

        public TEntity Insert(TEntity entityToInsert)
        {
            Type typeEntityToInsert = entityToInsert.GetType();
            PropertyInfo[] properties = typeEntityToInsert.GetProperties();

            TEntity entityForInsert = (TEntity)Activator.CreateInstance(typeof(TEntity), new object[] { });

            Type typeEntityForInsert = entityForInsert.GetType();
            PropertyInfo[] entityproperties = typeEntityForInsert.GetProperties();
            foreach (PropertyInfo prop in entityproperties)
            {
                var value = prop.GetValue(entityToInsert, null);
                if (!prop.PropertyType.IsGenericType || prop.PropertyType.GetGenericTypeDefinition() == typeof(DateTime))
                {
                    typeEntityForInsert.GetProperty(prop.Name).SetValue(entityForInsert, value, null);                    
                }
                else if (prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    if (typeEntityForInsert.GetProperty(prop.Name).GetValue(entityToInsert) != null)
                    {
                        typeEntityForInsert.GetProperty(prop.Name).SetValue(entityForInsert, value, null);
                    }
                    else
                    {
                        typeEntityForInsert.GetProperty(prop.Name).SetValue(entityForInsert, value, null);
                    }
                }

                if (prop.GetCustomAttribute<GeneralLogAttribute>() != null)
                {
                    GeneralLogDbSet.Add(new GeneralLog(prop.Name, entityForInsert, Identity.GetUserName()));
                }
            }

            entityForInsert.CreateBy = Identity.GetUserName();
            entityForInsert.CreateAt = DateTime.Now;
            entityForInsert.UpdateBy = Identity.GetUserName();
            entityForInsert.UpdateAt = DateTime.Now;
            entityForInsert.IsActive = true;

            DbSet.Add(entityForInsert);
            return entityForInsert;
        }

        public TEntity Update(object entityToUpdate)
        {
            Type typeEntityToUpdate = entityToUpdate.GetType();
            PropertyInfo[] properties = typeEntityToUpdate.GetProperties();
            List<string> namePropKeys = typeEntityToUpdate.GetPropertiesName<KeyAttribute>(entityToUpdate);
            object[] id = new object[namePropKeys.Count];
            for (int i = 0; i < namePropKeys.Count; i++)
            {
                id[i] = typeEntityToUpdate.GetProperty(namePropKeys[i]).GetValue(entityToUpdate, null);
            }
            TEntity entityForUpdate = GetByID(id);
            if (entityForUpdate == null)
            {
                throw new Core.Common.Utility.Validation.ValidationException(ErrorEnum.DataNotFound);
            }

            Type typeEntityForUpdate = entityForUpdate.GetType();

            List<Task> tasks = new List<Task>();
            var identName = Identity.GetUserName();
            
            foreach (var prop in properties)
            { 
                var value = prop.GetValue(entityToUpdate);
                if (typeEntityForUpdate.GetProperty(prop.Name) != null
                    && (!prop.PropertyType.IsGenericType || Nullable.GetUnderlyingType(prop.PropertyType) != null))
                {
                    typeEntityForUpdate.GetProperty(prop.Name).SetValue(entityForUpdate, value, null);
                    if (prop.GetCustomAttribute<GeneralLogAttribute>() != null)
                    {
                        GeneralLogDbSet.Add(new GeneralLog(prop.Name, entityForUpdate, identName));
                    }
                }
            } 

            entityForUpdate.UpdateBy = Identity.GetUserName();
            entityForUpdate.UpdateAt = DateTime.Now.Date;
            return entityForUpdate;
        }

        /*private List<string> GetPropertyNameOfKeyAttribute(PropertyInfo[] properties)
        {
            List<string> name = new List<string>();
            foreach (PropertyInfo prop in properties)
            {
                KeyAttribute attr = prop.GetCustomAttribute<KeyAttribute>();
                if (attr != null)
                {
                    name.Add(prop.Name);
                }
            }
            return name;
            throw new Exception("The Object Found KeyAttribute.");
        }*/

        public void Delete(object id)
        {
            TEntity entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(Func<TEntity, Boolean> where)
        {
            IQueryable<TEntity> objects = DbSet.Where<TEntity>(where).AsQueryable();
            foreach (TEntity obj in objects)
                DbSet.Remove(obj);
        }

        public void Delete(TEntity entityToDelete)
        {
            Type typeEntityToUpdate = entityToDelete.GetType();
            List<string> namePropKeys = typeEntityToUpdate.GetPropertiesName<KeyAttribute>(entityToDelete);
            object[] id = new object[namePropKeys.Count];
            for (int i = 0; i < namePropKeys.Count; i++)
            {
                id[i] = typeEntityToUpdate.GetProperty(namePropKeys[i]).GetValue(entityToDelete, null);
            }
            TEntity entityForDelete = GetByID(id);

            if (Context.Entry(entityForDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityForDelete);
            }
            DbSet.Remove(entityForDelete);
        }


        // Other
        public IEnumerable<TEntity> GetMany(Func<TEntity, bool> where)
        {
            return DbSet.Where(where).ToList();
        }

        public IQueryable<TEntity> GetManyQueryable(Func<TEntity, bool> where)
        {
            return DbSet.Where(where).AsQueryable();
        }

        public IQueryable<TEntity> GetWithInclude(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, params string[] include)
        {
            IQueryable<TEntity> query = this.DbSet;
            query = include.Aggregate(query, (current, inc) => current.Include(inc));
            return query.Where(predicate);
        }

        public IQueryable<TEntity> GetWithInclude(Func<TEntity, bool> predicate, params string[] include)
        {
            IQueryable<TEntity> query = this.DbSet;
            query = include.Aggregate(query, (current, inc) => current.Include(inc));
            return query.Where(predicate).AsQueryable();
        }

        public TEntity GetSingle(Func<TEntity, bool> predicate)
        {
            return DbSet.Single<TEntity>(predicate);
        }

        public TEntity GetFirst(Func<TEntity, bool> predicate)
        {
            return DbSet.First<TEntity>(predicate);
        }

        public string GetValidation(string tableName)
        {
            List<DbSchema> schemaList = new List<DbSchema>();
            string mainResult = Context.Database.SqlQuery<string>("ProcGetTableValidation @tableName"
                , new SqlParameter("@tableName", tableName)).FirstOrDefault();

            if (!string.IsNullOrEmpty(mainResult))
            {
                JObject objs = JsonConvert.DeserializeObject<JObject>(mainResult);

                foreach (var obj in objs)
                {
                    DbSchema schema = new DbSchema();
                    schema.FieldName = obj.Key;
                    var validates = obj.Value.ToArray();
                    foreach (var validate in validates)
                    {
                        string[] arValidate = validate.ToString().Split(':');
                        if (arValidate.Length == 2)
                        {
                            string validateType = arValidate[0].Replace("\"", "").Trim();
                            string validateValue = arValidate[1].Replace("\"", "").Trim();
                            if (!string.IsNullOrEmpty(validateValue))
                                schema.Fields.Add(new DbSchema.ValidationField(validateType, validateValue));
                        }
                    }
                    if (schema.Fields.Count > 0)
                        schemaList.Add(schema);
                }
                ObjectContext objContext = ((IObjectContextAdapter)Context).ObjectContext;
                var container = objContext.MetadataWorkspace.GetEntityContainer(objContext.DefaultContainerName, DataSpace.CSpace);
                var propEntityset = container.EntitySets.Where(w => w.Name == tableName).FirstOrDefault();

                if (propEntityset != null)
                {
                    var properties = propEntityset.ElementType.Properties;
                    if (properties.Count > 0)
                    {
                        foreach (var prop in properties)
                        {
                            if (!prop.Nullable | prop.MaxLength.HasValue)
                            {
                                var i = schemaList.Where(w => w.FieldName.ToUpper() == prop.Name.ToUpper()).ToList();
                                i.ForEach(f =>
                                {
                                    if (f.Fields.Any(a => a.Key == "Nullable") && !prop.Nullable)
                                        f.Fields.Find(w => w.Key == "Nullable").Value = "NO";

                                    if (!f.Fields.Any(a => a.Key == "MaxLength") && prop.MaxLength.HasValue)
                                        f.Fields.Add(new DbSchema.ValidationField("MaxLength", "" + prop.MaxLength.Value));
                                });
                            }
                        }
                    }

                }
            }
            return JsonConvert.SerializeObject(schemaList);
        }

    }
}
