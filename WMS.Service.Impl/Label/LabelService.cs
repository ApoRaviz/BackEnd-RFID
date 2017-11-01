using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Validation;
using WMS.Context;
using WMS.Entity.LayoutManagement;
using WMS.Service.Label;

namespace WMS.Service.Impl.Label
{
    public class LabelService : ILabelService
    {
        string pXmlDetail = "<row><Label_Code>{0}</Label_Code><Label_Type>{1}</Label_Type><Label_From>{2}</Label_From>" +
                            "<Label_Item>{3}</Label_Item><Font_Name>{4}</Font_Name><Font_Size>{5}</Font_Size>" +
                            "<Label_Top>{6}</Label_Top><Label_Left>{7}</Label_Left><Label_Width>{8}</Label_Width>" +
                            "<Label_Height>{9}</Label_Height><Label_BarcodeType>{10}</Label_BarcodeType><Label_Text>{11}</Label_Text><PxPerInch_Ratio>{12}</PxPerInch_Ratio></row>";

        private WMSDbContext Db;

        public LabelService()
        {
            Db = new WMSDbContext();
        }

        public List<LabelLayoutHeader_MT> GetAllLabelHeader(string forTable)
        {
            List<LabelLayoutHeader_MT> label = Db.LabelLayoutHeader_MT.Where(x => x.ForTable == forTable).ToList();
            return label;
        }

        public LabelLayoutHeader_MT GetLabelLayoutByReportIDSys(int id, string include)
        {
            LabelLayoutHeader_MT label = Db.LabelLayoutHeader_MT.Find(id);
            if (string.IsNullOrEmpty(include))
            {
                return label;
            }

            string[] includes = include.Replace(" ", "").Split(',');
            foreach (string inc in includes)
            {
                //if (inc.ToLower()[includes.Length - 1].Equals('s'))
                //{
                //    Db.Entry(report).Collection(inc).Load();
                //}
                //else
                //{
                //    Db.Entry(report).Reference(inc).Load();
                //}

                Db.Entry(label).Collection(inc).Load();
            }

            return label;
        }

        public int? CreateLabelForItemMaster(LabelLayoutHeader_MT data)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            int? ReportSysID = 0;

            foreach (LabelLayoutDetail_MT d in data.detail)
                sb.AppendFormat(pXmlDetail, d.Label_Code, d.Label_Type, d.Label_From, d.Label_Item, d.Font_Name, d.Font_Size.ToString(), d.Label_Top.ToString(), d.Label_Left.ToString(), d.Label_Width.ToString(), d.Label_Height.ToString(), d.Label_BarcodeType, d.Label_Text, d.PxPerInch_Ratio);

            using (var scope = new TransactionScope())
            {
                //data.CreatedDate = DateTime.Now;
                //data.UpdatedDate = DateTime.Now;
                //data.UserUpdate = "1";

                //Repo.Insert(customer);
                try
                {
                    ReportSysID = Db.ProcCreateLabelLayout(data.ForTable, data.FormatName, data.Width, data.WidthUnit, data.Height, data.HeightUnit
                                              , data.CreateAt, data.UpdateAt, data.UpdateBy, sb.ToString()).FirstOrDefault();
                    Db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                scope.Complete();
                return ReportSysID;
            }
        }

        public bool UpdateLabelForItemMaster(int LabelIDSys, LabelLayoutHeader_MT data)
        {
            System.Text.StringBuilder sb = new StringBuilder();

            foreach (LabelLayoutDetail_MT d in data.detail)
                sb.AppendFormat(pXmlDetail, d.Label_Code, d.Label_Type, d.Label_From, d.Label_Item, d.Font_Name, d.Font_Size.ToString(), d.Label_Top.ToString(), d.Label_Left.ToString(), d.Label_Width.ToString(), d.Label_Height.ToString(), d.Label_BarcodeType, d.Label_Text, d.PxPerInch_Ratio);

            using (var scope = new TransactionScope())
            {
                //data.UpdatedDate = DateTime.Now;
                //data.UserUpdate = "1";

                try
                {
                    Db.ProcUpdateLabelLayout(data.LabelIDSys, data.FormatName, data.Width, data.WidthUnit, data.Height, data.HeightUnit
                                              , data.UpdateAt, data.UpdateBy, sb.ToString());
                    Db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                scope.Complete();
                return true;
            }
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
