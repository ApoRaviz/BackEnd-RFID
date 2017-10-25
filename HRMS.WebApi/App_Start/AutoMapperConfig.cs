using AutoMapper;
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
                //cfg.CreateMap<ImportSerialHead, ItemImportDto>();

            });
        }
    }
}