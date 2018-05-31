using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.ValueObject;
using WMS.Common.ValueObject;
using WMS.Entity.InventoryManagement;
using WMS.Entity.ItemManagement;
using WMS.Entity.Receiving;
using WMS.Repository;
using WMS.Service;

namespace WMS.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/receive")]
    public class ReceiveController : ApiController
    {

        private IReceiveService ReceiveService;
        public ReceiveController(IReceiveService receiveService)
        {
            this.ReceiveService = receiveService;
        }

        // GET: api/Units
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<Receive>> response = new ResponseData<IEnumerable<Receive>>();
            try
            {
                IEnumerable<Receive> units = ReceiveService.GetReceives();
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(units);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // GET: api/Units/1
        [HttpGet]
        [Route("{receiveIDSys}")]
        public HttpResponseMessage Get(int receiveIDSys)
        {
            IResponseData<ReceiveDto> response = new ResponseData<ReceiveDto>();
            try
            {
                ReceiveDto unit = ReceiveService.GetReceiveByReceiveIDSys(receiveIDSys);
                response.SetData(unit);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }



        //[HttpGet]
        //[Route("autocomplete/{term}")]
        //public HttpResponseMessage AutocompleteUnit(string term)
        //{
        //    IResponseData<IEnumerable<AutocompleteUnitDto>> response = new ResponseData<IEnumerable<AutocompleteUnitDto>>();
        //    try
        //    {
        //        if (string.IsNullOrEmpty(term))
        //        {
        //            throw new Exception("Missing term");
        //        }
        //        IEnumerable<AutocompleteUnitDto> unit = UnitService.AutocompleteUnit(term);
        //        response.SetData(unit);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        response.SetErrors(ex.Errors);
        //        response.SetStatus(HttpStatusCode.PreconditionFailed);
        //    }
        //    return Request.ReturnHttpResponseMessage(response);
        //}

        // POST: api/Units
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]ReceiveDto receive)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                int id = ReceiveService.CreateReceive(receive);
                receive.ReceiveIDSys = id;
                response.SetData(id);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage PostInvoiceReceives([FromBody]InvoiceReceives invReceive)
        {
            IResponseData<InvoiceReceives> response = new ResponseData<InvoiceReceives>();
            try
            {
                //IInvoiceReceivesRepository invReceiveRopo = new InvoiceReceivesRepository();
                //int id = invReceiveRopo.(invReceive);
                //receive.ReceiveIDSys = id;
                //response.SetData(id);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // PUT: api/Units/5
        [HttpPut]
        [Route("{unitIDSys}")]
        public HttpResponseMessage Put(int unitIDSys, [FromBody]ReceiveDto unit)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = ReceiveService.UpdateReceive(unit);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }


        // PUT: api/Units/5
        [HttpPut]
        [Route("")]
        public HttpResponseMessage PutReceive([FromBody]ReceiveDto unit)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = ReceiveService.UpdateReceive(unit);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }


        // DELETE: api/Units/5
        [HttpDelete]
        [Route("{unitIDSys}")]
        public HttpResponseMessage Delete(int unitIDSys)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                //bool isUpated = UnitService.DeleteUnit(unitIDSys);
                response.SetData(false);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("tempInventoryTransactions/receive")]
        public HttpResponseMessage SaveReceive([FromBody]Receive receive)
        {
            IResponseData<Receive> response = new ResponseData<Receive>();
            Receive receiveResponse;
            try
            {
                receiveResponse = ReceiveService.SaveReceive(receive);
                response.SetData(receiveResponse);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("tempInventoryTransactions/item")]
        public HttpResponseMessage SaveItemFromReceiveToTempInventoryTransaction([FromBody]TempInventoryTransaction transaction)
        {
            IResponseData<TempInventoryTransaction> response = new ResponseData<TempInventoryTransaction>();
            TempInventoryTransaction tempInventoryTransactionResponse;
            try
            {
                tempInventoryTransactionResponse = ReceiveService.SaveTempInventoryTransaction(transaction);
                response.SetData(tempInventoryTransactionResponse);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("tempInventoryTransactions/items")]
        public HttpResponseMessage SaveItemFromReceiveToTempInventoryTransactions([FromBody]IEnumerable<TempInventoryTransaction> transactions)
        {
            IResponseData<IEnumerable<TempInventoryTransaction>> response = new ResponseData<IEnumerable<TempInventoryTransaction>>();
            IEnumerable<TempInventoryTransaction> tempInventoryTransactionsResponse;
            try
            {
                tempInventoryTransactionsResponse = ReceiveService.SaveTempInventoryTransactions(transactions);
                response.SetData(tempInventoryTransactionsResponse);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("tempInventoryTransactions/receiveAndItems")]
        public HttpResponseMessage SaveReceiveAndTempInventoryTransactions([FromBody]ReceiveTempInventoryTransaction receiveTempTransaction)
        {
            IResponseData<ReceiveTempInventoryTransaction> response = new ResponseData<ReceiveTempInventoryTransaction>();
            ReceiveTempInventoryTransaction receiveResponse;
            try
            {
                receiveResponse = ReceiveService.SaveReceiveAndTempInventoryTransactions(receiveTempTransaction);
                response.SetData(receiveResponse);
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
