using AutoMapper;
using Fuji.Common.ValueObject;
using Fuji.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fuji.WebApi
{
    public static class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                //FUJI
                cfg.CreateMap<ImportSerialHead, ItemImportDto>();

            });
        }
    }
}