using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Entity.ProjectManagement;
using Master.Common.ValueObject;
using WIM.Core.Entity.LabelManagement;
using WIM.Core.Common.Utility.UtilityHelpers;
using Newtonsoft.Json;
using Master.WebApi.ServiceBus.Events;

namespace Master.WebApi
{
    public static class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<HeadReportControl, HeadReportControlDto>();
                cfg.CreateMap<LabelControl, LabelControlDto>();
                //EventBus
                cfg.CreateMap<TestEvent, Customer_MT>();
            });
            //Mapper.Initialize(cfg =>
            //{
            //    cfg.CreateMap<LabelControl, LabelControlDto>();
            //});
        }
    }
}