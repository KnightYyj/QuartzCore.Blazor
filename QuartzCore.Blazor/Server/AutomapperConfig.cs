using AutoMapper;
using QuartzCore.Blazor.Shared;
using QuartzCore.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzCore.Blazor.Server
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<AppEntity, AppInputDto>();
            CreateMap<TasksQzDto, TasksQzEntity>();
            CreateMap<TasksQzEntity, TasksQzDto>();
            CreateMap<QzRunLogEntity, QzRunLogDto>();
            
            //CreateMap<List<AppEntity>, List<AppInputDto>>();
        }
    }
}
