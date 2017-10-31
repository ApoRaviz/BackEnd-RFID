using HRMS.Context;
using HRMS.Entity.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository;

namespace HRMS.Repository.Impl.LeaveManagement
{
    public class LeaveRepository : IGenericRepository<Leave>
    {
        private HrmsDbContext Db;

        public LeaveRepository()
        {
            Db = new HrmsDbContext();
        }

        public void Delete(object id)
        {
            var existedCountry = GetByID(id);
            //existedCountry.Active = 0;
            //existedCountry.UpdateDate = DateTime.Now;
            //existedCountry.UserUpdate = "1";
            Db.SaveChanges();
        }

        public void Delete(Leave entityToDelete)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<Leave, bool> where)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Leave> Get()
        {
            var leave = (from i in Db.Leave
                           select i);
            return leave;
        }

        public Leave Get(Func<Leave, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Leave> GetAll()
        {
            throw new NotImplementedException();
        }

        public Leave GetByID(object id)
        {
            Leave leave = Db.Leave.Find(id);
            return leave;
        }

        public Leave GetFirst(Func<Leave, bool> predicate)
        {
            throw new NotImplementedException();
        }
        
        public IEnumerable<Leave> GetMany(Func<Leave, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Leave> GetManyQueryable(Func<Leave, bool> where)
        {
            throw new NotImplementedException();
        }

        public Leave GetSingle(Func<Leave, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Leave> GetWithInclude(Expression<Func<Leave, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public void Insert(Leave entity)
        {
            Db.Leave.Add(entity);
            Db.SaveChanges();
        }

        public void Update(Leave Leave)
        {
            var existedCountry = GetByID(Leave.LeaveIDSys);
            existedCountry.LeaveType = Leave.LeaveType;
            existedCountry.StatusIDSys = Leave.StatusIDSys;
            existedCountry.Duration = Leave.Duration;
            existedCountry.Comment = Leave.Comment;
            existedCountry.LeaveTypeIDSys = Leave.LeaveTypeIDSys;
            existedCountry.Duration = Leave.Duration;
            Db.SaveChanges();
        }
    }
}
