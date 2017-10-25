using HRMS.Repository.Context;
using HRMS.Repository.Entity.LeaveRequest;
using HRMS.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Extensions;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;
using WIM.Core.Context;
using WIM.Core.Entity.Status;

namespace HRMS.WebApi.Controllers
{
    [RoutePrefix("api/v1/demo")]
    public class DemoController : ApiController
    {
        private IDemoService DemoService;

        public DemoController(IDemoService demoService)
        {
            DemoService = demoService;
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Demo([FromBody]Leave leave)
        {
            ResponseData<int> response = new ResponseData<int>();
            try
            {
                List<string> l = new List<string>();
                
                IList<string> l1 = new List<string>();


                ILeaveRepository repo = new LeaveRepository();

             


                HRMSDbContext hrmsDb = new HRMSDbContext();


                hrmsDb.Entry(leave).State = EntityState.Modified;



                hrmsDb.SaveChanges();
                response.SetData(1);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }
    }

    public class BaseEntity
    {
        public string CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateAt { get; set; }
    }

    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get();
        TEntity GetByID(object id);
        void Insert(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);

    }

    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected DbContext Context;

        public GenericRepository()
        {

        }

            public GenericRepository(DbContext context)
        {
            Context = context;
        }



        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entityToDelete)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Get()
        {
            throw new NotImplementedException();
        }

        public TEntity GetByID(object id)
        {
            throw new NotImplementedException();
        }

        public void Insert(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entityToUpdate)
        {        
          
            Context.SaveChanges();
            throw new NotImplementedException();
        }
    }

    public interface ITest
    {

    }

    public interface ILeaveRepository : IGenericRepository<Leave>
    {
        IEnumerable<Leave> GetTopSellingCourses(int count);
        IEnumerable<Leave> GetCoursesWithAuthors(int pageIndex, int pageSize);

    }

    public class LeaveRepository : GenericRepository<Leave>,  ILeaveRepository
    {

        public LeaveRepository()
        {
            this.Context = new HRMSDbContext();
        }

        public LeaveRepository(DbContext context)  : base(context)
        {

        }

        public IEnumerable<Leave> GetCoursesWithAuthors(int pageIndex, int pageSize)
        {
            ///
            throw new NotImplementedException();
        }
         
      

        public IEnumerable<Leave> GetTopSellingCourses(int count)
        {
            throw new NotImplementedException();
        }
    }


    public class LeaveDetailRepository
    {
     
    }

}
