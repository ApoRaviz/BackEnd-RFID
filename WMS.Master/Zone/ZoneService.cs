using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Validation;
using WMS.Master;

namespace WMS.Master.Zone
{
    public class ZoneService : IZoneService
    {
        string pXmlDetail = "<row><ZoneID>{0}</ZoneID><Name>{1}</Name><Left>{2}</Left><Top>{3}</Top>" +
                            "<Width>{4}</Width><Length>{5}</Length><Use>{6}</Use><Floor>{7}</Floor><ZoneParentID>{8}</ZoneParentID></row>";

        string pXmlRack = "<row><ZoneIDSys>{0}</ZoneIDSys><ZoneID>{1}</ZoneID><RackID>{2}</RackID><BlockID>{3}</BlockID>" +
                            "<Floor>{4}</Floor><Left>{5}</Left><Top>{6}</Top></row>";

        private MasterContext Db = MasterContext.Create();

        public ZoneService()
        {
        }

        public List<ZoneLayoutHeader_MT> GetAllZoneHeader()
        {
            List<ZoneLayoutHeader_MT> zone = Db.ZoneLayoutHeader_MT.ToList();
            return zone;
        }

        public List<ZoneLayoutDetail_MT> GetAllZoneDetail()
        {
            List<ZoneLayoutDetail_MT> zone = Db.ZoneLayoutDetail_MT.ToList();
            return zone;
        }

        public ZoneLayoutHeader_MT GetZoneLayoutByZoneIDSys(int id, string include)
        {
            ZoneLayoutHeader_MT zone = Db.ZoneLayoutHeader_MT.Find(id);
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

        public int? CreateZoneLayout(ZoneLayoutHeader_MT data)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            int? ZoneSysID = 0;

            foreach (ZoneLayoutDetail_MT d in data.detail)
                sb.AppendFormat(pXmlDetail, d.ZoneID.ToString(), d.Name, d.Left.ToString(), d.Top.ToString(), d.Width.ToString(), d.Length.ToString(), d.Use, d.Floor, d.ZoneParentID);

            using (var scope = new TransactionScope())
            {
                data.CreatedDate = DateTime.Now;
                data.UpdatedDate = DateTime.Now;
                data.UserUpdate = "1";
  
                try
                {
                    ZoneSysID = Db.ProcCreateZoneLayout(data.ZoneName, data.Warehouse, data.Area, data.TotalFloor
                                              , data.CreatedDate, data.UpdatedDate, data.UserUpdate, sb.ToString()).FirstOrDefault();
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

        public bool UpdateZoneLayout(int ZoneIDSys, ZoneLayoutHeader_MT data)
        {
            System.Text.StringBuilder sb = new StringBuilder();

            foreach (ZoneLayoutDetail_MT d in data.detail)
                sb.AppendFormat(pXmlDetail, d.ZoneID.ToString(), d.Name, d.Left.ToString(), d.Top.ToString(), d.Width.ToString(), d.Length.ToString(), d.Use, d.Floor, d.ZoneParentID);

            using (var scope = new TransactionScope())
            {
                data.UpdatedDate = DateTime.Now;
                data.UserUpdate = "1";

                try
                {
                    Db.ProcUpdateZoneLayout(data.ZoneIDSys, data.ZoneName, data.Warehouse, data.Area, data.TotalFloor
                                              , data.UpdatedDate, data.UserUpdate, sb.ToString());
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

        public int? CreateRackLayout(List<RackLayout_MT> data)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            int? RackSysID = 0;

            if (data != null && data[0].RackID != 0)
            {
                foreach (RackLayout_MT d in data)
                    sb.AppendFormat(pXmlRack, d.ZoneIDSys.ToString(), d.ZoneID.ToString(), d.RackID.ToString(), d.BlockID.ToString(), d.Floor, d.Left.ToString(), d.Top.ToString());
            }

            using (var scope = new TransactionScope())
            {
                try
                {
                    Db.ProcCreateRackLayout(data[0].ZoneIDSys, data[0].ZoneID, DateTime.Now, DateTime.Now, "1", sb.ToString());
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

        public List<RackLayout> GetAllRackDetail(int ZoneIDSys, int ZoneID)
        {
            List<RackLayout> rack = Db.ProcGetRackLayout(ZoneIDSys, ZoneID).ToList();
            return rack;
        }

        public List<RackLayout> GetRackDetailByZoneIDSys(int ZoneIDSys)
        {
            List<RackLayout> rack = Db.ProcGetRackLayoutByZoneIDSys(ZoneIDSys).ToList();
            return rack;
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
