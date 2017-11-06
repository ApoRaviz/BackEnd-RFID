using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Repository;
using WIM.Core.Common.Validation;
using Fuji.Service.PrintLabel;
using WIM.Core.Repository.Impl;
using Fuji.Context;
using Fuji.Entity.LabelManagement;
using System.Net.Http;
using Microsoft.Reporting.WebForms;
using BarcodeLib;
using Fuji.Common.ValueObject;
using System.IO;
using System.Drawing;
using Fuji.Repository.Impl.LabelManagement;
using System.Security.Principal;
using Fuji.Repository.LabelManagement;

namespace Fuji.Service.Impl.PrintLabel
{
    public class PrintLabelService : WIM.Core.Service.Impl.Service,IPrintLabelService
    {
        //private FujiDbContext Db { get; set; }
        //private LabelRunningRepository printRepo;

        public PrintLabelService()
        {
            //Db = FujiDbContext.Create();
            //printRepo = new LabelRunningRepository(new FujiDbContext());
        }        

        public int GetRunningByType(string type, int running,string userUpdate)
        {
            int baseRunning = 0;
            bool isNotUpdateDate;
            LabelRunning label;
            using (FujiDbContext Db = new FujiDbContext())
            { 
                label = (from l in Db.LabelRunning
                                      where l.Type.Equals(type)
                                      select l
                             ).SingleOrDefault();
            }

            if (isNotUpdateDate = (label.CreateAt.Date == DateTime.Now.Date))
            {
                baseRunning = label.Running;
            }
            else
            {
                baseRunning = 0;
            }            

            UpdateRunning(label, running, isNotUpdateDate,userUpdate);

            return baseRunning;            
        }

        private bool UpdateRunning(LabelRunning label, int running, bool isNotUpdateDate,string userUpdate)
        {
            using (var scope = new TransactionScope())
            {
                if (isNotUpdateDate)
                {
                    label.Running += running;
                }
                else
                {
                    label.CreateAt = DateTime.Now.Date;
                    label.CreateBy = userUpdate;
                    label.Running = running;
                }

                using (FujiDbContext Db = new FujiDbContext())
                {
                    ILabelRunningRepository printRepo = new LabelRunningRepository(Db);

                    try
                    {
                        printRepo.Update(label);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        HandleValidationException(e);
                    }
                   
                }
               
            }
            return true;
        }

        public StreamContent GetReportStream(int running,int baseRunning,string type= "BXFJ")
        {
            byte[] bytes;
            Warning[] warnings;
            string[] streamids;
            string mimeType, encoding, extension;
            List<FujiDataLabelBarcode> barcodeList = new List<FujiDataLabelBarcode>();
            Barcode bc = new Barcode();
            type = "BXFJ";

            for (int i = 0; i < running; i++)
            {
                string barcodeInfoImage = type
                    + DateTime.Now.ToString("yyMMdd", new System.Globalization.CultureInfo("en-US"))
                    + (baseRunning + i).ToString("0000");
                string barcodeInfo = type
                    + "\n"
                    + DateTime.Now.ToString("yyMMdd", new System.Globalization.CultureInfo("en-US"))
                    + (baseRunning + i).ToString("0000");

                byte[] barcodeImage = bc.EncodeToByte(TYPE.CODE128, barcodeInfoImage, Color.Black, Color.White, 287, 96);

                FujiDataLabelBarcode barcode = new FujiDataLabelBarcode(barcodeImage, barcodeInfo);
                barcodeList.Add(barcode);
            }

            using (var reportViewer = new ReportViewer())
            {
                //reportViewer.Width = 384;
                //reportViewer.Height = 384;
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = "Report/BoxLabelReport.rdlc";
                reportViewer.LocalReport.Refresh();
                reportViewer.LocalReport.EnableExternalImages = true;

                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = barcodeList;
                reportViewer.LocalReport.DataSources.Add(rds1);
                bytes = reportViewer.LocalReport.Render("Pdf", null, out mimeType, out encoding, out extension, out streamids, out warnings);

            }

            MemoryStream stream =  new MemoryStream(bytes);
            return new StreamContent(stream);
        }

        public void HandleValidationException(DbEntityValidationException ex)
        {
            foreach (var eve in ex.EntityValidationErrors)
            {
                foreach (var ve in eve.ValidationErrors)
                {
                    throw new ValidationException(ve.PropertyName, ve.ErrorMessage);
                }
            }
        }

    }
}
