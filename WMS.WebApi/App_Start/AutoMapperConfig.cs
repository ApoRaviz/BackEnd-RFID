using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.Master;
using WMS.Common;
namespace WMS.WebApi
{
    public static class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Customer_MT, CustomerDto>();
                cfg.CreateMap<Project_MT, ProjectDto>();
                cfg.CreateMap<Category_MT, CategoryDto>();
                cfg.CreateMap<Item_MT, ItemDto>();
                cfg.CreateMap<ItemSet_MT, ItemSetDto>();
                cfg.CreateMap<ItemUnitMapping, ItemUnitDto>()
                    .ForMember(d => d.ItemIDSys, opt => opt.MapFrom(s => s.ItemIDSys))
                    .ForMember(d => d.UnitIDSys, opt => opt.MapFrom(s => s.UnitIDSys))
                    .ForMember(d => d.ProjectIDSys, opt => opt.MapFrom(s => s.Unit_MT.ProjectIDSys))
                    .ForMember(d => d.UnitID, opt => opt.MapFrom(s => s.Unit_MT.UnitID))
                    .ForMember(d => d.UnitName, opt => opt.MapFrom(s => s.Unit_MT.UnitName))
                    .ForMember(d => d.Weight, opt => opt.MapFrom(s => s.Weight))
                    .ForMember(d => d.Width, opt => opt.MapFrom(s => s.Width))
                    .ForMember(d => d.Length, opt => opt.MapFrom(s => s.Length))
                    .ForMember(d => d.Height, opt => opt.MapFrom(s => s.Height))
                    .ForMember(d => d.MainUnit, opt => opt.MapFrom(s => s.MainUnit))
                    .ForMember(d => d.Sequence, opt => opt.MapFrom(s => s.Sequence))
                    .ForMember(d => d.Cost, opt => opt.MapFrom(s => s.Cost))
                    .ForMember(d => d.Price, opt => opt.MapFrom(s => s.Price));

            });
        }
    }
}