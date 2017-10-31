using Fuji.Entity.LabelManagement;
using Fuji.Repository.LabelManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;
using System.Data.Entity;
using Fuji.Context;

namespace Fuji.Repository.Impl.LabelManagement
{
   
    public class LabelRunningRepository : Repository<LabelRunning>, ILabelRunningRepository
    {
        FujiDbContext Db { get; set; }
        DbSet<LabelRunning> DbSet { get; set; }
        public LabelRunningRepository(FujiDbContext context) : base(context)
        {
            Db = context;
            this.DbSet = Db.Set<LabelRunning>();
        }

        public void UpdateItem(LabelRunning item, string username)
        {
            item.UpdateBy = username;
            item.UpdateAt = DateTime.Now;

            DbSet.Attach(item);
            Context.Entry(item).State = EntityState.Modified;
            Db.SaveChanges();
        }
    }
}
