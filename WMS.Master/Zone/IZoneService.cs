﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Master.Zone
{
    public interface IZoneService
    {
        List<ZoneLayoutHeader_MT> GetAllZoneHeader();
        List<ZoneLayoutDetail_MT> GetAllZoneDetail();
        ZoneLayoutHeader_MT GetZoneLayoutByZoneIDSys(int id, string include);
        int? CreateZoneLayout(ZoneLayoutHeader_MT data);
        bool UpdateZoneLayout(int ZoneIDSys, ZoneLayoutHeader_MT data);
        int? CreateRackLayout(List<RackLayout_MT> data);
        List<RackLayout> GetAllRackDetail(int ZoneIDSys, int ZoneID);
        List<RackLayout> GetRackDetailByZoneIDSys(int ZoneIDSys);
    }
}
