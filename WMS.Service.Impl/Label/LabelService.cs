using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Utility.Validation;
using WMS.Context;
using WMS.Entity.LayoutManagement;
using WMS.Repository.Impl.Label;
using WMS.Repository.Label;
using WMS.Service.Label;

namespace WMS.Service.Impl.Label
{
    public class LabelService : WIM.Core.Service.Impl.Service, ILabelService
    {
        string pXmlDetail = "<row><Label_Code>{0}</Label_Code><Label_Type>{1}</Label_Type><Label_From>{2}</Label_From>" +
                            "<Label_Item>{3}</Label_Item><Font_Name>{4}</Font_Name><Font_Size>{5}</Font_Size>" +
                            "<Label_Top>{6}</Label_Top><Label_Left>{7}</Label_Left><Label_Width>{8}</Label_Width>" +
                            "<Label_Height>{9}</Label_Height><Label_BarcodeType>{10}</Label_BarcodeType><Label_Text>{11}</Label_Text><PxPerInch_Ratio>{12}</PxPerInch_Ratio></row>";



        public LabelService()
        {
        }

        public List<LabelLayoutHeader_MT> GetAllLabelHeader(string forTable)
        {
            using (WMSDbContext Db = new WMSDbContext()) {
                ILabelLayoutHeaderRepository repo = new LabelLayoutHeaderRepository(Db);
                List<LabelLayoutHeader_MT> label = repo.GetMany(x => x.ForTable == forTable).ToList();
                return label;
            }
        }

        public LabelLayoutHeader_MT GetLabelLayoutByReportIDSys(int id, string include)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                ILabelLayoutHeaderRepository repo = new LabelLayoutHeaderRepository(Db);

                LabelLayoutHeader_MT label = repo.GetByID(id);
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
        }

        public int? CreateLabelForItemMaster(LabelLayoutHeader_MT data)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            int? ReportSysID = 0;

            foreach (LabelLayoutDetail_MT d in data.detail)
                sb.AppendFormat(pXmlDetail, d.Label_Code, d.Label_Type, d.Label_From, d.Label_Item, d.Font_Name, d.Font_Size.ToString(), d.Label_Top.ToString(), d.Label_Left.ToString(), d.Label_Width.ToString(), d.Label_Height.ToString(), d.Label_BarcodeType, d.Label_Text, d.PxPerInch_Ratio);

            using (var scope = new TransactionScope())
            {
                data.CreateAt = DateTime.Now;
                data.UpdateAt = DateTime.Now;
                data.UpdateBy = Identity.Name;

                //Repo.Insert(customer);
                using (WMSDbContext Db = new WMSDbContext())
                {
                    try
                    {
                        ReportSysID = Db.ProcCreateLabelLayout(data.ForTable, data.FormatName, data.Width, data.WidthUnit, data.Height, data.HeightUnit
                                                  , data.CreateAt, data.UpdateAt, data.UpdateBy, sb.ToString());
                        Db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new AppValidationException(e);
                    }
                    scope.Complete();
                    return ReportSysID;
                }
            }
        }

        public bool UpdateLabelForItemMaster(int LabelIDSys, LabelLayoutHeader_MT data)
        {
            System.Text.StringBuilder sb = new StringBuilder();

            foreach (LabelLayoutDetail_MT d in data.detail)
                sb.AppendFormat(pXmlDetail, d.Label_Code, d.Label_Type, d.Label_From, d.Label_Item, d.Font_Name, d.Font_Size.ToString(), d.Label_Top.ToString(), d.Label_Left.ToString(), d.Label_Width.ToString(), d.Label_Height.ToString(), d.Label_BarcodeType, d.Label_Text, d.PxPerInch_Ratio);

            using (var scope = new TransactionScope())
            {
                data.UpdateAt = DateTime.Now;
                data.UpdateBy = Identity.Name;
                using (WMSDbContext Db = new WMSDbContext())
                {
                    try
                    {
                        Db.ProcUpdateLabelLayout(data.LabelIDSys, data.FormatName, data.Width, data.WidthUnit, data.Height, data.HeightUnit
                                                  , data.UpdateAt, data.UpdateBy, sb.ToString());
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new AppValidationException(e);
                    }
                    
                    return true;
                }
            }
        }

    }
}
