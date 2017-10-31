using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Repository.Entity.LeaveRequest;

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