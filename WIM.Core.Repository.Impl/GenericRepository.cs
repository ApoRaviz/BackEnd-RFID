using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Repository.Impl
{
    public class GenericRepository<TEntity>
    {
        internal DbContext Context;
        //internal DbSet<TEntity> DbSet;

        public GenericRepository(DbContext context)
        {
            this.Context = context;
            //this.DbSet = context.Set<TEntity>();
        }
        
    }
}
