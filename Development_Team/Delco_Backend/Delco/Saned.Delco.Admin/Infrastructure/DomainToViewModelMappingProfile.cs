using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Saned.Delco.Admin.Models;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Admin.Infrastructure
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            
            CreateMap<Setting, SettingViewModel>();
            CreateMap<SettingViewModel, Setting>();

            CreateMap<ApplicationUser, UserViewModel>();
            CreateMap<UserViewModel, ApplicationUser>();
            CreateMap<ApplicationUser, UserEditViewModel>();
            CreateMap<UserEditViewModel, ApplicationUser>();
            CreateMap<ApplicationUser, AgentViewModel>();
            CreateMap<AgentViewModel , ApplicationUser>();
            CreateMap<City, CityViewModel>();
            CreateMap<CityViewModel, City>();

            
        }

    }
}