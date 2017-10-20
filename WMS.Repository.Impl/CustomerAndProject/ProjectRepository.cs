using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Repository;
using WMS.Common;

namespace WMS.Repository.Impl.CustomerAndProject
{
    public class ProjectRepository : IGenericRepository<Project_MT>
    {
        private CoreDbContext Db;

        public ProjectRepository()
        {
            Db = new CoreDbContext();
        }

        public IEnumerable<Project_MT> Get()
        {
            var projects = from c in Db.Project_MT
                           select c;
            return projects;
        }

        public Project_MT GetByID(object id)
        {
            var project = (from c in Db.Project_MT
                           where c.ProjectIDSys== (int)id
                           select c).SingleOrDefault();
            return project;
        }

        public Project_MT GetProjectByProjIDincluCus(int projectid)
        {
            var project = (from c in Db.Project_MT
                           where c.ProjectIDSys == projectid
                           select c).Include(b => b.Customer_MT).SingleOrDefault();
            return project;
        }

        public IEnumerable<Project_MT> GetProjectHaveMenu(int CusID)
        {
            var project = from row in Db.Project_MT
                          where (from o in Db.MenuProjectMapping
                                 select o.ProjectIDSys).Contains(row.ProjectIDSys)
                                 && row.CusIDSys == CusID
                          select row;
            return project;
        }

        public IEnumerable<Project_MT> GetProjectCustomer(int CusID)
        {
            var project = from row in Db.Project_MT
                          where row.CusIDSys == CusID
                          select row;
            return project;
        }

        public void Insert(Project_MT entity)
        {
            entity.ProjectID = Db.ProcGetNewID("PJ").FirstOrDefault();
            entity.ProjectStatus = "Active";
            entity.CreatedDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            entity.UserUpdate = "1";
            Db.Project_MT.Add(entity);
            Db.SaveChanges();
        }

        public void Delete(object id)
        {
            var existedProject = Db.Project_MT.SingleOrDefault(p => p.ProjectIDSys== (int)id);
            existedProject.ProjectStatus = "Inactive";
            existedProject.UpdateDate = DateTime.Now;
            existedProject.UserUpdate = "1";
            Db.SaveChanges();
        }

        public void Delete(Project_MT entityToDelete)
        {
            throw new NotImplementedException();
        }

        public void Update(Project_MT entityToUpdate)
        {
            var existedProject = GetByID(entityToUpdate.ProjectIDSys);
            existedProject.ProjectName = entityToUpdate.ProjectName;
            existedProject.UpdateDate = DateTime.Now;
            existedProject.UserUpdate = "1";
            Db.SaveChanges();
        }

        public IEnumerable<Project_MT> GetMany(Func<Project_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Project_MT> GetManyQueryable(Func<Project_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public Project_MT Get(Func<Project_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<Project_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Project_MT> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Project_MT> GetWithInclude(Expression<Func<Project_MT, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public Project_MT GetSingle(Func<Project_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Project_MT GetFirst(Func<Project_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
