using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Saned.Delco.Api.Models;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Api.Infrastructure
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<DeviceSetting, DeviceSettingModel> ();
            CreateMap< DeviceSettingModel, DeviceSetting>();
            CreateMap<Request, RequestViewModel>();
            CreateMap<RequestViewModel, Request>();
            CreateMap<PassengerNumber, PassengerNumbersViewModel>();
            CreateMap <PassengerNumbersViewModel, PassengerNumber > ();
            CreateMap<SettingViewModel, Setting>();
            CreateMap<Setting, SettingViewModel>();
            CreateMap<Abuse, AbuseViewModel>();
            CreateMap<AbuseViewModel, Abuse>();
            CreateMap<ContactUsViewModel, ContactUs > ();
            CreateMap<ContactUs, ContactUsViewModel>();
            CreateMap<RefuseReason, RefuseReasonModel>();
            CreateMap<RefuseReasonModel,RefuseReason>();
            CreateMap<RefuseRequestModel, RefuseRequest>();
            CreateMap<RefuseRequest, RefuseRequestModel>();
            CreateMap<RatingViewModel, Rating> ();
            CreateMap<Rating, RatingViewModel>();
        }

    }
}