using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WIM.Core.Repository.Impl;
using WMS.Common.ValueObject;
using WMS.Context;
using WMS.Entity.WarehouseManagement;
using WMS.Repository.Warehouse;

namespace WMS.Repository.Impl.Warehouse
{
    public class LocationGroupRepository : Repository<GroupLocation>, ILocationGroupRepository
    {
        private WMSDbContext Db;

        public LocationGroupRepository(WMSDbContext context) : base(context)
        {
            Db = context;
        }

        public IEnumerable<GroupLocationDto> GetListLocationGroupDto()
        {
            var qr = (from sp in Db.GroupLocation
                      join lt in Db.LocationType on sp.LocTypeIDSys equals lt.LocTypeIDSys
                      select new GroupLocationDto
                      {
                          Columns = sp.Columns,
                          Description = sp.Description,
                          GroupLocIDSys = sp.GroupLocIDSys,
                          LocTypeIDSys = sp.LocTypeIDSys,
                          LocTypeName = lt.Name,
                          Name = sp.Name,
                          Rows = sp.Rows
                      }).ToList();
            return qr;
        }

        public IEnumerable<AutocompleteLocationDto> AutocompleteLocation(string term)
        {
            var qr = (from sp in Db.Locations
                      where sp.LocNo.Contains(term)
                      select new AutocompleteLocationDto
                      {
                          LocIDSys = sp.LocIDSys,
                          LocNo = sp.LocNo
                      }
            ).ToList();
            return qr;
        }

        public IEnumerable<ZoneLocationDto> GetLocationRecommend(LocationControlDto control)
        {
            IEnumerable<ZoneLocationDto> locationlist;
            List<string> controltype = new List<string>();
            
            if (control.Control != null)
            {
                int lengthcontrol = control.Control.Count();
                for (int i = 0; i < lengthcontrol; i++)
                {
                    if (control.Control[i].Key == "Tempurature")
                    {
                        if (int.Parse(control.Control[i].Value) <= 25)
                        {
                            controltype.Add("2");
                        }
                        else if (int.Parse(control.Control[i].Value) <= 40)
                        {
                            controltype.Add("3");
                        }
                        else if (int.Parse(control.Control[i].Value) > 40)
                        {
                            controltype.Add("4");
                        }
                    }
                    else if (control.Control[i].Key == "Safety")
                    {
                        controltype.Add("5");
                    }
                    else if (control.Control[i].Key == "Secret")
                    {
                        controltype.Add("6");
                    }
                }
            }

            var warehouse = (from i in Db.ZoneLayoutHeader_MT
                             join o in Db.ZoneLayoutDetail_MT on i.ZoneIDSys equals o.ZoneIDSys
                             where i.Warehouse == 4
                             select new { i, o }).ToList();
            if(controltype.Count > 0)
            {
                var controldetail = warehouse.Where(a => a.o.Types.All(controltype.Contains)).Select(q => new {
                    q.o.ZoneIDSys , q.o.ZoneID , q.o.Floor
                }).ToList();
                var zoneidsys = controldetail.Select(a => a.ZoneIDSys).ToList();
                var zoneid = controldetail.Select(a => a.ZoneID).ToList();
                var floor = controldetail.Select(a => a.Floor).ToList();

                //controldetail.Any(a => a.ZoneID == i.ZoneID && a.ZoneIDSys == i.ZoneIDSys && a.Floor == i.Floor)
                //
                locationlist = (from i in Db.ZoneLayoutDetail_MT
                                join o in Db.ZoneLayoutHeader_MT on i.ZoneIDSys equals o.ZoneIDSys into zonee
                                from zon in zonee.DefaultIfEmpty()
                                join p in Db.Warehouse_MT on zon.Warehouse equals p.WHIDSys
                                join k in Db.GroupLocation on new { ZoneID = i.ZoneID, ZoneIDSys = i.ZoneIDSys, Floor = i.Floor } equals new { ZoneID = k.ZoneID ?? 0, ZoneIDSys = k.ZoneIDSys ?? 0, Floor = k.Floor ?? 0 } into grouplocate
                                from grouplocation in grouplocate.DefaultIfEmpty()
                                join l in Db.Locations on grouplocation.GroupLocIDSys equals l.GroupLocIDSys into locationss
                                from location in locationss.DefaultIfEmpty()
                                where zoneidsys.Contains(i.ZoneIDSys) && zoneid.Contains(i.ZoneID) && floor.Contains(i.Floor) && location.AvailableArea >= control.Dimension
                                select new ZoneLocationDto()
                                {
                                    ZoneName = i.Name,
                                    LocIDSys = location.LocIDSys,
                                    LocNo = location.LocNo,
                                    Type = i.Type,
                                    WHName = p.WHName,
                                    AvailableArea = location.AvailableArea
                                }
                                ).ToList();
            }
            else
            {
                locationlist = (from i in Db.ZoneLayoutDetail_MT
                                join o in Db.ZoneLayoutHeader_MT on i.ZoneIDSys equals o.ZoneIDSys into zonee
                                from zon in zonee.DefaultIfEmpty()
                                join p in Db.Warehouse_MT on zon.Warehouse equals p.WHIDSys
                                join k in Db.GroupLocation on new { ZoneID = i.ZoneID, ZoneIDSys = i.ZoneIDSys, Floor = i.Floor } equals new { ZoneID = k.ZoneID ?? 0, ZoneIDSys = k.ZoneIDSys ?? 0, Floor = k.Floor ?? 0 } into grouplocate
                                from grouplocation in grouplocate.DefaultIfEmpty()
                                join l in Db.Locations on grouplocation.GroupLocIDSys equals l.GroupLocIDSys into locationss
                                from location in locationss.DefaultIfEmpty()
                                where zon.Warehouse == 4 && location.AvailableArea > control.Dimension
                                select new ZoneLocationDto()
                                {
                                    ZoneName = i.Name,
                                    LocIDSys = location.LocIDSys,
                                    LocNo = location.LocNo,
                                    Type = i.Type,
                                    WHName = p.WHName,
                                    AvailableArea = location.AvailableArea
                                }
                                ).ToList();
            }
            
            return locationlist;

        }
    }
}
