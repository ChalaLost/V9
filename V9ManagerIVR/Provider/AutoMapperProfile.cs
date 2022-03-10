using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using V9ManagerIVR.Models.Entities;
using V9ManagerIVR.Models.Extension;

namespace V9ManagerIVR.Provider
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Extension, ExtensionElasticModel>();
            CreateMap<IVR, IVRElasticModel>();
            CreateMap<Models.Entities.Action, ActionElasticModel>();
            CreateMap<ICollection<CalendarIVR>, ICollection<CalendarIVRElasticModel>>();
            CreateMap<Calendar, CalendarElasticModel>();
            CreateMap<CalendarDayInWeek, CalendarDayInWeekElasticModel>();
            CreateMap<CalendarDayInWeekTime, CalendarDayInWeekTimeElasticModel>();
            CreateMap<CalendarDate, CalendarDateElasticModel>();
            CreateMap<TM_Mobile, TM_MobileElasticModel>();

        }
    }
}
