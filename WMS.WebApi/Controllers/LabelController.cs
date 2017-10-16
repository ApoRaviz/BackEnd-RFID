

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;
using WIM.Core.Common.Extensions;
using WMS.WebApi.Report;
using System.Drawing;
using System.Data;
using WIM.Core.Common.Constants;
//using WMS.WebApi.DataSet;
using OnBarcode.Barcode.ASPNET;
using BarcodeLib;
using System.Threading.Tasks;
using System.IO;
using WMS.Common;
using WMS.Service;
using WMS.Service.Label;
using WMS.Entity.LayoutManagement;
using WMS.Entity.ItemManagement;
using WIM.Core.Entity.SupplierManagement;

namespace WMS.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/label")]
    public class LabelController : ApiController
    {
        private ILabelService LabelService;
        private IItemService ItemService;

        public LabelController(ILabelService labelService, IItemService itemService)
        {
            this.LabelService = labelService;
            this.ItemService = itemService;
        }
        
        [HttpGet]
        [Route("GetHeader/{ForTable}")]
        public HttpResponseMessage Get(string forTable)
        {
            ResponseData<IEnumerable<LabelLayoutHeader_MT>> response = new ResponseData<IEnumerable<LabelLayoutHeader_MT>>();
            try
            {
                IEnumerable<LabelLayoutHeader_MT> header = LabelService.GetAllLabelHeader(forTable);
                response.SetData(header);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // GET: api/label/1
        [HttpGet]
        [Route("{LabelIDSys}")]
        public HttpResponseMessage Get(int LabelIDSys)
        {
            IResponseData<LabelLayoutHeader_MT> response = new ResponseData<LabelLayoutHeader_MT>();
            try
            {
                LabelLayoutHeader_MT label = LabelService.GetLabelLayoutByReportIDSys(LabelIDSys, "LabelLayoutDetail_MT");
                response.SetData(label);
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
        public HttpResponseMessage Post([FromBody]LabelLayoutHeader_MT data)
        {        
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                data.UserUpdate = User.Identity.Name;
                int id = LabelService.CreateLabelForItemMaster(data).Value;          
                response.SetData(id);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPut]
        [Route("{LabelIDSys}")]
        public HttpResponseMessage Put(int LabelIDSys, [FromBody]LabelLayoutHeader_MT data)
        {
            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = LabelService.UpdateLabelForItemMaster(LabelIDSys, data);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("GetItemLabel/{LabelIDSys}")]
        public HttpResponseMessage GetItemLabel(int LabelIDSys, [FromBody]Item_MT item)
        {
            LabelLayoutHeader_MT label = LabelService.GetLabelLayoutByReportIDSys(LabelIDSys, "LabelLayoutDetail_MT");
            label.detail = label.LabelLayoutDetail_MT.ToList();

            MemoryStream m = ReportUtils.CreateReportToStream(label);

            DataSet.MasterDataSet ds = new DataSet.MasterDataSet();
            DataSet.MasterDataSetTableAdapters.tb_ItemTableAdapter adp = new DataSet.MasterDataSetTableAdapters.tb_ItemTableAdapter();

            adp.Fill(ds.tb_Item, item.ItemCode);
            //adp.Fill(ds.tb_Item, "31");

            foreach (LabelLayoutDetail_MT d in label.LabelLayoutDetail_MT)
            {
                if(d.Label_Type == Constant.BarCode_Text)
                {
                    var colName = "";

                    if (d.Label_From == Constant.Newword_Text)
                    {
                        colName = d.Label_BarcodeType.ToString().Replace(" ", "") + d.Label_Text.Replace(" ", "");
                    }
                    else if (d.Label_From == Constant.FromMaster_Text)
                    {
                        colName = d.Label_BarcodeType.ToString().Replace(" ", "") + d.Label_Item;
                    }

                    if (!ds.tb_Item.Columns.Contains(colName))
                    {
                        DataColumn col = new DataColumn(colName, typeof(byte[]));
                        ds.tb_Item.Columns.Add(col);
                    }
                }
            }

            List<LabelLayoutDetail_MT> detail = label.detail;

            BarcodeLib.Barcode barcode = new BarcodeLib.Barcode();

            foreach (DataSet.MasterDataSet.tb_ItemRow row in ds.tb_Item.Rows)
            {
                List<LabelLayoutDetail_MT> field = detail.Where(x => x.Label_Type == Constant.BarCode_Text).ToList();

                foreach (LabelLayoutDetail_MT f in field)
                {
                    var colName = "";
                    var dataDraw = "";

                    if (f.Label_From == Constant.Newword_Text)
                    {
                        colName = f.Label_BarcodeType.ToString().Replace(" ", "") + f.Label_Text.Replace(" ", "");
                        dataDraw = f.Label_Text;
                    }
                    else if (f.Label_From == Constant.FromMaster_Text)
                    {
                        colName = f.Label_BarcodeType.ToString().Replace(" ", "") + f.Label_Item;
                        dataDraw = row[f.Label_Item].ToString();
                    }

                    if(f.Label_BarcodeType == BarCode_Type.QRCode)
                    {
                        byte[] data = ReportUtils.GenerateQrCodeToByte(dataDraw);
                        row[colName] = data;
                    }
                    else if(f.Label_BarcodeType == BarCode_Type.Code_128)
                    {
                        byte[] data = barcode.EncodeToByte(TYPE.CODE128, dataDraw, Color.Black, Color.White, 400, 200);
                        row[colName] = data;
                    }
                    else if(f.Label_BarcodeType == BarCode_Type.Code_93)
                    {
                        byte[] data = barcode.EncodeToByte(TYPE.CODE93, dataDraw, Color.Black, Color.White, 400, 200);
                        row[colName] = data;
                    }
                }                
            } 
            
            return ReportUtils.ViewReportFromStream(m, ds.tb_Item);
        }

        [HttpGet]
        [Route("GetSupplierLabel/{LabelIDSys}")]
        public HttpResponseMessage GetSupplierLabel(int LabelIDSys, [FromBody]Supplier_MT supplier)
        {
            LabelLayoutHeader_MT label = LabelService.GetLabelLayoutByReportIDSys(LabelIDSys, "LabelLayoutDetail_MT");
            label.detail = label.LabelLayoutDetail_MT.ToList();

            MemoryStream m = ReportUtils.CreateReportToStream(label);

            DataSet.MasterDataSet ds = new DataSet.MasterDataSet();
            DataSet.MasterDataSetTableAdapters.tb_SupplierTableAdapter adp = new DataSet.MasterDataSetTableAdapters.tb_SupplierTableAdapter();

            adp.Fill(ds.tb_Supplier, supplier.SupID);
            //adp.Fill(ds.tb_Supplier, "S001");

            foreach (LabelLayoutDetail_MT d in label.LabelLayoutDetail_MT)
            {
                if (d.Label_Type == Constant.BarCode_Text)
                {
                    var colName = "";

                    if (d.Label_From == Constant.Newword_Text)
                    {
                        colName = d.Label_BarcodeType.ToString().Replace(" ", "") + d.Label_Text.Replace(" ", "");
                    }
                    else if (d.Label_From == Constant.FromMaster_Text)
                    {
                        colName = d.Label_BarcodeType.ToString().Replace(" ", "") + d.Label_Item;
                    }

                    if (!ds.tb_Supplier.Columns.Contains(colName))
                    {
                        DataColumn col = new DataColumn(colName, typeof(byte[]));
                        ds.tb_Supplier.Columns.Add(col);
                    }
                }
            }

            List<LabelLayoutDetail_MT> detail = label.detail;

            BarcodeLib.Barcode barcode = new BarcodeLib.Barcode();

            foreach (DataSet.MasterDataSet.tb_SupplierRow row in ds.tb_Supplier.Rows)
            {
                List<LabelLayoutDetail_MT> field = detail.Where(x => x.Label_Type == Constant.BarCode_Text).ToList();

                foreach (LabelLayoutDetail_MT f in field)
                {
                    var colName = "";
                    var dataDraw = "";

                    if (f.Label_From == Constant.Newword_Text)
                    {
                        colName = f.Label_BarcodeType.ToString().Replace(" ", "") + f.Label_Text.Replace(" ", "");
                        dataDraw = f.Label_Text;
                    }
                    else if (f.Label_From == Constant.FromMaster_Text)
                    {
                        colName = f.Label_BarcodeType.ToString().Replace(" ", "") + f.Label_Item;
                        dataDraw = row[f.Label_Item].ToString();
                    }

                    if (f.Label_BarcodeType == BarCode_Type.QRCode)
                    {
                        byte[] data = ReportUtils.GenerateQrCodeToByte(dataDraw);
                        row[colName] = data;
                    }
                    else if (f.Label_BarcodeType == BarCode_Type.Code_128)
                    {
                        byte[] data = barcode.EncodeToByte(TYPE.CODE128, dataDraw, Color.Black, Color.White, 400, 200);
                        row[colName] = data;
                    }
                    else if (f.Label_BarcodeType == BarCode_Type.Code_93)
                    {
                        byte[] data = barcode.EncodeToByte(TYPE.CODE93, dataDraw, Color.Black, Color.White, 400, 200);
                        row[colName] = data;
                    }
                }
            }

            return ReportUtils.ViewReportFromStream(m, ds.tb_Supplier);
        }
    }
}
