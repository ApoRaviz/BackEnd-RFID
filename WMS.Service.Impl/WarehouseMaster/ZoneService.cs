using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Utility.Validation;
using WMS.Context;
using WMS.Entity.WarehouseManagement;
using WMS.Repository.Impl.Warehouse;
using WMS.Repository.Warehouse;
using WMS.Service.WarehouseMaster;

namespace WMS.Service.Impl.WarehouseMaster
{
    public class ZoneService : WIM.Core.Service.Impl.Service, IZoneService
    {
        string pXmlDetail = "<row><ZoneID>{0}</ZoneID><Name>{1}</Name><Left>{2}</Left><Top>{3}</Top>" +
                            "<Width>{4}</Width><Length>{5}</Length><Use>{6}</Use><Floor>{7}</Floor><ZoneParentID>{8}</ZoneParentID>"+
                            "<UpdateAt>{9}</UpdateAt><UpdateBy>{10}</UpdateBy><IsActive>{11}</IsActive><Type>{12}</Type></row>";

        string pXmlRack = "<row><ZoneIDSys>{0}</ZoneIDSys><ZoneID>{1}</ZoneID><RackID>{2}</RackID><BlockID>{3}</BlockID>" +
                            "<Floor>{4}</Floor><Left>{5}</Left><Top>{6}</Top></row>";


        public ZoneService()
        {
        }

        public List<ZoneLayoutHeader_MT> GetAllZoneHeader()
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                IZoneLayoutHeaderRepository repo = new ZoneLayoutHeaderRepository(Db);
                List<ZoneLayoutHeader_MT> zone = repo.Get().ToList();
                return zone;
            }
        }

        public List<ZoneLayoutDetail_MT> GetAllZoneDetail()
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                IZoneLayoutDetailRepository repo = new ZoneLayoutDetailRepository(Db);
                List<ZoneLayoutDetail_MT> zone = repo.Get().ToList();
                return zone;
            }
        }

        public ZoneLayoutHeader_MT GetZoneLayoutByZoneIDSys(int id, string include)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                IZoneLayoutHeaderRepository repo = new ZoneLayoutHeaderRepository(Db);
                ZoneLayoutHeader_MT zone = repo.GetByID(id);
                if (string.IsNullOrEmpty(include))
                {
                    return zone;
                }

                string[] includes = include.Replace(" ", "").Split(',');
                foreach (string inc in includes)
                {
                    Db.Entry(zone).Collection(inc).Load();
                }

                return zone;
            }
        }

        public int? CreateZoneLayout(ZoneLayoutHeader_MT data)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            int? ZoneSysID = 0;

            foreach (ZoneLayoutDetail_MT d in data.detail)
            {
                d.IsActive = true;
                d.CreateAt = DateTime.Now;
                d.UpdateAt = DateTime.Now;
                d.UpdateBy = Identity.Name;
                sb.AppendFormat(pXmlDetail
                    , d.ZoneID.ToString()
                    , d.Name
                    , d.Left.ToString()
                    , d.Top.ToString()
                    , d.Width.ToString()
                    , d.Length.ToString()
                    , d.Use
                    , d.Floor
                    , d.ZoneParentID
                    , DateTime.Now.ToString("yyyy-MM-dd HH:mm")
                    , d.UpdateBy
                    , "1"
                    , d.Type);
            }
            using (var scope = new TransactionScope())
            {
                using (WMSDbContext Db = new WMSDbContext())
                {
                    try
                    {
                        ZoneSysID = Db.ProcCreateZoneLayout(data.ZoneName, data.Warehouse, data.Area, data.TotalFloor, Identity.Name, sb.ToString());
                        Db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        HandleValidationException(e);
                    }
                    scope.Complete();
                    return ZoneSysID;
                }
            }
        }

        public bool UpdateZoneLayout(int ZoneIDSys, ZoneLayoutHeader_MT data)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            foreach (ZoneLayoutDetail_MT d in data.detail)
            {
                d.IsActive = true;
                d.UpdateAt = DateTime.Now.Date;
                d.UpdateBy = Identity.Name;
                sb.AppendFormat(pXmlDetail
                    , d.ZoneID.ToString()
                    , d.Name
                    , d.Left.ToString()
                    , d.Top.ToString()
                    , d.Width.ToString()
                    , d.Length.ToString()
                    , d.Use
                    , d.Floor
                    , d.ZoneParentID
                    , DateTime.Now.ToString("yyyy-MM-dd HH:mm")
                    , d.UpdateBy
                    , "1"
                    , d.Type);
            }
            using (var scope = new TransactionScope())
            {
                using (WMSDbContext Db = new WMSDbContext())
                {
                    data.UpdateBy = Identity.Name;

                    try
                    {
                        Db.ProcUpdateZoneLayout(data.ZoneIDSys, data.ZoneName, data.Warehouse, data.Area, data.TotalFloor
                                                  , data.UpdateBy, sb.ToString());
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
        }

        public int? CreateRackLayout(List<RackLayout_MT> data)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            int? RackSysID = 0;

            if (data != null && data[0].RackID != 0)
            {
                foreach (RackLayout_MT d in data)
                {
                    d.IsActive = true;
                    d.CreateAt = DateTime.Now;
                    d.UpdateAt = DateTime.Now;
                    d.UpdateBy = Identity.Name;
                    sb.AppendFormat(pXmlRack, d.ZoneIDSys.ToString(), d.ZoneID.ToString(), d.RackID.ToString(), d.BlockID.ToString(), d.Floor, d.Left.ToString(), d.Top.ToString());
                }
                }

            using (var scope = new TransactionScope())
            {
                using (WMSDbContext Db = new WMSDbContext())
                {
                    try
                    {
                        Db.ProcCreateRackLayout(data[0].ZoneIDSys, data[0].ZoneID, DateTime.Now, DateTime.Now, Identity.Name, sb.ToString());
                        Db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        HandleValidationException(e);
                    }
                    scope.Complete();
                    return RackSysID;
                }
            }
        }

        public List<RackLayout> GetAllRackDetail(int ZoneIDSys, int ZoneID)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                List<RackLayout> rack = Db.ProcGetRackLayout(ZoneIDSys, ZoneID);
                return rack;
            }
        }

        public List<RackLayout> GetRackDetailByZoneIDSys(int ZoneIDSys)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                List<RackLayout> rack = Db.ProcGetRackLayoutByZoneIDSys(ZoneIDSys);
                return rack;
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
