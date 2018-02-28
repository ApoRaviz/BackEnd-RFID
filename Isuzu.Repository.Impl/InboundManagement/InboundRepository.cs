using Isuzu.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;
using Isuzu.Context;
using Isuzu.Repository.ItemManagement;
using WIM.Core.Repository.Impl;
using System.Security.Principal;

namespace Isuzu.Repository.Impl
{
    public class InboundRepository : Repository<InboundItems>, IInboundRepository
    {

        private IsuzuDataContext Db { get; set; }
        private DbSet<InboundItems> DbSet;

        public InboundRepository(IsuzuDataContext context) :base(context)
        {
           Db = context;
           this.DbSet = context.Set<InboundItems>();
        }

        //****CUSTOM****//

        public InboundItems GetItemBy(Func<InboundItems, bool> where)
        {
            return DbSet.Find(where);
        }
        public InboundItems GetItemFirstBy(Func<InboundItems, bool> where)
        {
            return DbSet.Where(where).FirstOrDefault();
        }
        public InboundItems GetItemSingleBy(Func<InboundItems, bool> where)
        {
            return DbSet.Where(where).SingleOrDefault();
        }

        public IEnumerable<InboundItems> GetItemsBy(Func<InboundItems, bool> where)
        {
            return DbSet.Where(where);
        }

        public bool IsItemExistBy(Func<InboundItems, bool> where)
        {
            return DbSet.Find(where) != null;
        }

        public IEnumerable<T> SqlQuery<T>(string sql,params object[] parameters)
        {
            return Context.Database.SqlQuery<T>(sql, parameters);
           }

        public void InsertItem(InboundItems entity, string username)
        {
            entity.CreateBy = username;
            entity.CreateAt = DateTime.Now;
            entity.UpdateBy = username;
            entity.UpdateAt = DateTime.Now;

            DbSet.Add(entity);
            Db.SaveChanges();
        }

        public void UpdateItem(InboundItems entityToUpdate, string username)
        {
            entityToUpdate.UpdateBy = username;
            entityToUpdate.UpdateAt = DateTime.Now;

            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
            Db.SaveChanges();
        }

    }
}
