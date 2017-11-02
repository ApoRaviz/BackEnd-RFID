using AutoMapper;
using HRMS.Common.ValueObject.LeaveManagement;
using HRMS.Entity.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.WebApi
{
    public static class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<LeaveDto, Leave>();
                cfg.CreateMap<Leave, LeaveDto>();

                cfg.CreateMap<LeaveDetail, LeaveDetailDto>();
                cfg.CreateMap<LeaveDetailDto, LeaveDetail>();

            });
        }
    }
}