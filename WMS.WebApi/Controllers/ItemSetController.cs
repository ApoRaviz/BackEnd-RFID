﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WMS.Common.ValueObject;
using WMS.Entity.ItemManagement;
using WMS.Service;

namespace WMS.WebApi.Controller
{
    // [Authorize]
    [RoutePrefix("api/v1/ItemSets")]
    public class ItemSetsController : ApiController
    {
        private IItemSetService ItemSetService { get; set; }

        public ItemSetsController(IItemSetService ItemSetService)
        {
            this.ItemSetService = ItemSetService;
        }

        // GET: api/ItemSets
        //[Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetItemSets()
        {
            ResponseData<IEnumerable<ItemSetDto>> response = new ResponseData<IEnumerable<ItemSetDto>>();
            try
            {
                IEnumerable<ItemSetDto> ItemSets = ItemSetService.GetDto(50);
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(ItemSets);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        //GET: api/ItemSets/1
        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage GetItemSet([FromUri]int id)
        {
            IResponseData<ItemSetDto> response = new ResponseData<ItemSetDto>();
            try
            {
                ItemSetDto ItemSet = ItemSetService.GetItemSet(id);
                response.SetData(ItemSet);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }



        // POST: api/ItemSets
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]ItemSet_MT ItemSet)
        {
            IResponseData<object> response = new ResponseData<object>();
            try
            {
                object id = ItemSetService.CreateItemSet(ItemSet);
                response.SetData(id);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("{ItemSetIDSys}")]
        public HttpResponseMessage UpdateItemSet(int ItemSetIDSys, [FromBody]ItemSet_MT ItemSet)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {

                
                int idSet = ItemSetService.UpdateItemSet(ItemSet);
                response.SetData(idSet);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // PUT: api/ItemSets/5
        [HttpPut]
        [Route("{id}")]
        public HttpResponseMessage Put(int id, [FromBody]ItemSet_MT ItemSet)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = ItemSetService.UpdateItemSet(id,ItemSet);
                response.SetData(isUpated);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // DELETE: api/ItemSets/5
        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = ItemSetService.DeleteItemSetDto(id);
                response.SetData(isUpated);
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