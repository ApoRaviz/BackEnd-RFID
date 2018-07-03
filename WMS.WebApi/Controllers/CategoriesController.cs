using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WMS.Common.ValueObject;
using WMS.Entity.ControlMaster;
using WMS.Entity.ItemManagement;
using WMS.Service;
using WMS.Service.ControlMaster;

namespace WMS.WebApi.Controller
{
    //[Authorize]
    [RoutePrefix("api/v1/categories")]
    public class CategoriesController : ApiController
    {
        private ICategoryService CategoryService;
        private IControlService ControlService;
        public CategoriesController(ICategoryService categoryService,IControlService controlservice)
        {
            this.CategoryService = categoryService;
            this.ControlService = controlservice;
        }

        // GET: api/Categories
        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetCategories()
        {
            ResponseData<IEnumerable<Category_MT>> response = new ResponseData<IEnumerable<Category_MT>>();
            try
            {
                IEnumerable<Category_MT> categories = CategoryService.GetCategoriesByProjectID(User.Identity.GetProjectIDSys());
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(categories);
            }
            catch (AppValidationException ex)
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
            IResponseData<Category_MT> response = new ResponseData<Category_MT>();
            try
            {
                Category_MT category = CategoryService.GetCategory(id);
                response.SetData(category);
            }
            catch (AppValidationException ex)
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
            catch (AppValidationException ex)
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
                if (category.Control_MT != null)
                {

                    if (category.ControlIDSys != null)
                    {
                        Control_MT control = category.Control_MT;
                        ControlService.UpdateControl(control);
                    }
                    else
                    {
                        if (category.Control_MT.ControlDetails.Count > 0)
                        {
                            int control;
                            control = ControlService.CreateControl(category.Control_MT);
                            category.ControlIDSys = control;
                        }
                    }
                }
                category.Control_MT = null;
                bool isUpated = CategoryService.UpdateCategory(category);
                response.SetData(isUpated);
            }
            catch (AppValidationException ex)
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
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("autocomplete/{term}")]
        public HttpResponseMessage AutocompletCategories(string term)
        {
            IResponseData<IEnumerable<AutocompleteCategoryDto>> response = new ResponseData<IEnumerable<AutocompleteCategoryDto>>();
            try
            {
                if (string.IsNullOrEmpty(term))
                {
                    throw new Exception("Missing term");
                }
                IEnumerable<AutocompleteCategoryDto> category = CategoryService.AutocompleteCategory(term);
                response.SetData(category);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }
    }
}
