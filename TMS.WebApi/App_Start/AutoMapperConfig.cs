using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.WebApi
{
    public static class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {

            });
        }
    }
}