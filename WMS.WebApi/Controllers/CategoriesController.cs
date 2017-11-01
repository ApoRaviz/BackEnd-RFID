using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Extensions;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;
using WMS.Common.ValueObject;
using WMS.Entity.ItemManagement;
using WMS.Service;

namespace WMS.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/categories")]
    public class CategoriesController : ApiController
    {
        private ICategoryService CategoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            this.CategoryService = categoryService;
        }

        // GET: api/Categories
        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetCategories()
        {
            ResponseData<IEnumerable<CategoryDto>> response = new ResponseData<IEnumerable<CategoryDto>>();
            try
            {
                IEnumerable<CategoryDto> categories = CategoryService.GetCategoriesByProjectID(User.Identity.GetProjectIDSys());
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(categories);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // GET: api/categories/1
        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage GetCategory([FromUri]int id)
        {
            IResponseData<CategoryDto> response = new ResponseData<CategoryDto>();
            try
            {
                CategoryDto category = CategoryService.GetCategory(id);
                response.SetData(category);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // POST: api/Categories
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]Category_MT category)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                int id = CategoryService.CreateCategory(category);
                response.SetData(id);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // PUT: api/Categories/5
        [HttpPut]
        [Route("{id}")]
        public HttpResponseMessage Put(int id, [FromBody]Category_MT category)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = CategoryService.UpdateCategory(category);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // DELETE: api/Categories/5
        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = CategoryService.DeleteCategory(id);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }
    }
}
