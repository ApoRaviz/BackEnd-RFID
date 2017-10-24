using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.Dimension;
using WIM.Core.Repository;
using WMS.Common;


namespace WMS.Repository.Impl
{
    class DimensionRepository : IGenericRepository<DimensionLayout_MT>
    {
        private CoreDbContext Db { get; set; }

        public DimensionRepository()
        {
            Db = new CoreDbContext();
        }

        public IEnumerable<DimensionLayout_MT> Get()
        {
            var Dimension = from c in Db.DimensionLayout_MT
                            select c;
            return Dimension.ToList();
        }

        public DimensionLayout_MT GetByID(object id)
        {
            var Dimension = from c in Db.DimensionLayout_MT
                            where c.DimensionIDSys== (int)id
                            select c;
            return Dimension.SingleOrDefault();
        }

        public void Insert(DimensionLayout_MT data)
        {
            Db.ProcCreateDimensionLayout(data.FormatName, data.Unit, data.Width, data.Length, data.Height, data.Weight
                                              , data.Type, data.Color, data.CreatedDate, data.UpdatedDate, data.UserUpdate);
            Db.SaveChanges();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(DimensionLayout_MT entityToDelete)
        {
            throw new NotImplementedException();
        }

        public void Update(DimensionLayout_MT data)
        {
            Db.ProcUpdateDimensionLayout(data.DimensionIDSys, data.FormatName, data.Unit, data.Width, data.Length, data.Height, data.Weight
                                              , data.Type, data.Color, data.UpdatedDate, data.UserUpdate).FirstOrDefault();
            Db.SaveChanges();
        }

        public IEnumerable<DimensionLayout_MT> GetMany(Func<DimensionLayout_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DimensionLayout_MT> GetManyQueryable(Func<DimensionLayout_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public DimensionLayout_MT Get(Func<DimensionLayout_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<DimensionLayout_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DimensionLayout_MT> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<DimensionLayout_MT> GetWithInclude(Expression<Func<DimensionLayout_MT, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public DimensionLayout_MT GetSingle(Func<DimensionLayout_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public DimensionLayout_MT GetFirst(Func<DimensionLayout_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
