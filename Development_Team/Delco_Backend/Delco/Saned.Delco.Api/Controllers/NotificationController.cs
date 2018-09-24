using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Saned.Delco.Api.Models;
using Saned.Delco.Api.Results;
using Saned.Delco.Data.Core.Models;
using WebGrease.Activities;

namespace Saned.Delco.Api.Controllers
{
    [RoutePrefix("api/Notification")]
    public class NotificationController : BaseController
    {


        [Route("SaveDevicSetting")]
        [Attributes.Authorize(Roles = "User,Agent")]

        [HttpPost]
        public  IHttpActionResult SaveDevicSetting(DeviceSettingModel model)
        {

          var deviceModel= Mapper.Map<DeviceSettingModel,DeviceSetting>(model);
          var result =  _UnitOfWork.DeviceSettingRepository.Create(deviceModel);
          _UnitOfWork.Commit();
          return Ok(result);
        }

        [Route("GetUserNotifications")]
        [Attributes.Authorize(Roles = "User,Agent")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUserNotifications(PagingViewModel model)
        {
            try
            {
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName);

                var notification = await _UnitOfWork.NotificationDeviceRepository
                                              .GetUserNotifications(u.Id,model.PageSize, model.PageNumber);
           
                return Ok(notification);
            }
            catch (Exception ex)
            {
                
                return InternalServerError(ex);
            }


        }

        
    }
}