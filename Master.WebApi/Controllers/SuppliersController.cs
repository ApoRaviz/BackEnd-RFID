using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Entity.SupplierManagement;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.ValueObject;
using WIM.Core.Service;

namespace Master.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/Suppliers")]
    public class SuppliersController : ApiController
    {
        private ISupplierService SupplierService;

        public SuppliersController(ISupplierService SupplierService)
        {
            this.SupplierService = SupplierService;
        }

        // GET: api/Suppliers
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<Supplier_MT>> response = new ResponseData<IEnumerable<Supplier_MT>>();
            try
            {
                IEnumerable<Supplier_MT> Suppliers = SupplierService.GetSuppliersByProjectID(User.Identity.GetProjectIDSys());
                response.SetData(Suppliers);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // GET: api/Suppliers/1
        [HttpGet] 
        [Route("{supIDSys}")]
        public HttpResponseMessage Get(int supIDSys)
        {
            IResponseData<Supplier_MT> response = new ResponseData<Supplier_MT>();
            try
            {
                Supplier_MT Supplier = SupplierService.GetSupplierBySupIDSys(supIDSys);
                response.SetData(Supplier);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }



        // POST: api/Suppliers
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]Supplier_MT Supplier)
        {
            IResponseData<Supplier_MT> response = new ResponseData<Supplier_MT>();
            try
            {
                Supplier.UpdateBy = User.Identity.Name;
                Supplier_MT supplier = SupplierService.CreateSupplier(Supplier);
                response.SetData(supplier);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // PUT: api/Suppliers/5

        [HttpPut]
        [Route("{supIDSys}")]
        public HttpResponseMessage Put(int supIDSys, [FromBody]Supplier_MT Supplier)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = SupplierService.UpdateSupplier(Supplier);
                response.SetData(isUpated);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        // DELETE: api/Suppliers/5
        [HttpDelete]
        [Route("{supIDSys}")]
        public HttpResponseMessage Delete(int supIDSys)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = SupplierService.DeleteSupplier(supIDSys);
                response.SetData(isUpated);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // GET: api/items/1
        [HttpGet]
        [Route("autocomplete/{term}")]
        public HttpResponseMessage AutocompleteSupplier(string term = "")
        {
            IResponseData<IEnumerable<AutocompleteSupplierDto>> response = new ResponseData<IEnumerable<AutocompleteSupplierDto>>();
            try
            {
                if (string.IsNullOrEmpty(term))
                {
                    throw new Exception("Missing term");
                }
                IEnumerable<AutocompleteSupplierDto> sub = SupplierService.AutocompleteSupplier(term);
                response.SetData(sub);
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
