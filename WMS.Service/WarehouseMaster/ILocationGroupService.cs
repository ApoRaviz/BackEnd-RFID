﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Service;
using WMS.Common.ValueObject;
using WMS.Entity.WarehouseManagement;
using WMS.Master.Common.ValueObject;

namespace WMS.Master
{
    public interface ILocationGroupService : IService
    {
        IEnumerable<GroupLocation> GetLocationGroup();
        IEnumerable<GroupLocationDto> GetListLocationGroupDto();
        IEnumerable<GroupLocation> GetUnassignLocationGroup();
        GroupLocation GetLocationGroupByGroupLocIDSys(int id);
        IEnumerable<GroupLocation> GetLocationGroupByZoneInfo(GroupLocation item);
        IEnumerable<GroupLocation> GetLocationGroupByZoneID(int zoneIDSys);
        IEnumerable<ZoneLocationDto> GetLocationForRecommend(LocationControlDto control);
        IEnumerable<AutocompleteLocationDto> AutocompleteLocation(string term);
        GroupLocation CreateLocationGroup(GroupLocation locationGroup);
        bool UpdateLocationGroup(int locationGroupIDSys, GroupLocation locationGroup);
        bool UpdateAllLocationGroup(List<GroupLocation> locationGroups);
        bool DeleteLocationGroup(int id);

        IEnumerable<Location> GetLocation();
        Location GetLocationByLocIDSys(int id);
        //int CreateLocation(Location Location);
        bool UpdateLocation(Location Location);
        bool DeleteLocation(int id);
    }
}
