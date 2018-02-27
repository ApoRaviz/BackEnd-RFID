using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WMS.Service;
using WMS.Entity.ItemManagement;
using WMS.Common.ValueObject;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.Extensions;

namespace WMS.WebApi.Controllers
{
   // [Authorize]
    [RoutePrefix("api/v1/items")]
    public class ItemsController : ApiController
    {
        private IItemService ItemService;
        public ItemsController(IItemService itemService)
        {
            this.ItemService = itemService;
        }

        // GET: api/Items
        //[Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetItems()
        {
            ResponseData<IEnumerable<ItemDto>> response = new ResponseData<IEnumerable<ItemDto>>();
            try
            {
                IEnumerable<ItemDto> items = ItemService.GetItems();
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(items);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }       

        // GET: api/items/1
        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage GetItem([FromUri]int id, string include = "")
        {
            IResponseData<ItemDto> response = new ResponseData<ItemDto>();
            try
            {
                string[] tableNames = null;
                if (!string.IsNullOrEmpty(include))
                {
                    tableNames = include.Split(',');
                }
                ItemDto item = ItemService.GetItem(id, tableNames);                
                response.SetData(item);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // GET: api/items/1
        [HttpGet]
        [Route("autocomplete/{term}")]
        public HttpResponseMessage AutocompleteItem(string term = "")
        {
            IResponseData<IEnumerable<AutocompleteItemDto>> response = new ResponseData<IEnumerable<AutocompleteItemDto>>();
            try
            {
                if (string.IsNullOrEmpty(term))
                {
                    throw new Exception("Missing term");
                }
                IEnumerable<AutocompleteItemDto> item = ItemService.AutocompleteItem(term);
                response.SetData(item);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // POST: api/Items
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]Item_MT item)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                item.ProjectIDSys = User.Identity.GetProjectIDSys();
                int id = ItemService.CreateItem(item);
                response.SetData(id);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }



        // PUT: api/Items/5
        [HttpPut]
        [Route("{id}")]
        public HttpResponseMessage Put(int id, [FromBody]Item_MT item)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = ItemService.UpdateItem(item);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // DELETE: api/Items/5
        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = ItemService.DeleteItem(id);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpDelete]
        [Route("unit")]
        public HttpResponseMessage DeleteUnit(ItemUnitMapping item)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = ItemService.DeleteItemUnit(item);
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
