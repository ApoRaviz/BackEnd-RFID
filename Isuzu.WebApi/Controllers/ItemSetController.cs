﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Common.Extensions;
using WIM.Master;
using WIM.Common.Http;
using WIM.Common.Validation;

namespace WIM.WebApi.Controllers
{
    // [Authorize]
    [RoutePrefix("api/v1/ItemSets")]
    public class ItemSetsController : ApiController
    {
        private IItemSetService ItemSetService;
        public ItemSetsController(IItemSetService ItemSetService)
        {
            this.ItemSetService = ItemSetService;
        }

        // GET: api/ItemSets
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetItemSets()
        {
            ResponseData<IEnumerable<ItemSetDto>> response = new ResponseData<IEnumerable<ItemSetDto>>();
            try
            {
                IEnumerable<ItemSetDto> ItemSets = ItemSetService.GetItemSets();
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(ItemSets);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // GET: api/ItemSets/1
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
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }



        // POST: api/ItemSets
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]ItemSetDto ItemSet)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                int id = ItemSetService.CreateItemSet(ItemSet);
                response.SetData(id);
                int idSet = 0;
                for(int i = 0; i< ItemSet.ItemSetDetail.Count; i++)
                {
                    idSet = ItemSetService.CreateItemsetDetail(id, ItemSet.ItemSetDetail[i]);
                }
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("{ItemSetIDSys}")]
        public HttpResponseMessage Post2(int ItemSetIDSys,[FromBody]List<ItemSetDetailDto> ItemSet)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                int idSet = 0;
                for (int i = 0; i < ItemSet.Count; i++)
                {
                    idSet = ItemSetService.CreateItemsetDetail(ItemSetIDSys, ItemSet[i]);
                }
                response.SetData(idSet);
            }
            catch (ValidationException ex)
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
                bool isUpated = ItemSetService.UpdateItemSet(id, ItemSet);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
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
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }
    }
}