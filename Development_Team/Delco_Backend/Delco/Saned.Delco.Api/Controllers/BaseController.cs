using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using OneSignalLibrary;
using OneSignalLibrary.Posting;
using Saned.Delco.Api.Hubs;
using Saned.Delco.Data.Core;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Persistence;
using Saned.Delco.Data.Persistence.Repositories;
using Saned.Delco.Data.Persistence.Tools;
using Saned.Delco.Data.Extentions;
using Attachment = OneSignalLibrary.Posting.Attachment;

namespace Saned.Delco.Api.Controllers
{
    public class BaseController : ApiController
    {
        protected readonly AuthRepository _repo = null;
        protected readonly IUnitOfWorkAsync _UnitOfWork;
        OneSignalClient client;
        string appId = WebConfigurationManager.AppSettings["appId"];
        string restKey = WebConfigurationManager.AppSettings["restKey"];
        string userAuth = WebConfigurationManager.AppSettings["userAuth"];
        readonly NotificationHubHellper _notificationHub = new NotificationHubHellper();
        protected EmailManager EmailManager = new EmailManager();
        public BaseController()
        {

            _repo = new AuthRepository();
            _UnitOfWork = new UnitOfWorkAsync();
            client = new OneSignalClient(appId, restKey, userAuth);
        }

        public async Task<ApplicationUser> GetApplicationUser(string userName)
        {
            return await _repo.FindUserByUserName(userName);
        }


        public async Task<ApplicationUser> GetApplicationUserById(string userId)
        {
            return await _repo.FindUserById(userId);
        }
        public IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

     


        public async Task<long> SendNotification(Request model, int type,string senderName,string refusemsg="")
        {
            NotificationType notificationType = (NotificationType)type;
            var arMessage=notificationType.GetStringValue();
            var devicesList = _UnitOfWork.DeviceSettingRepository.All().Include(u=>u.ApplicationUser);
            arMessage = arMessage.Replace("####", senderName);
            if (type == 4 || type == 7)
            {
                arMessage += "بسبب :";
                arMessage += refusemsg;
            }



            Attachment attachment;
            var content = BuildNotificationContent(model, arMessage, out attachment);

            HashSet<string> hashSet = new HashSet<string>();


            
                //save message vs message device vs Return Device Ids
                var messageId = SaveMessageVsReturnsDeviceIds(model, type, arMessage, notificationType, devicesList, ref hashSet);


            // send notification using one signal 
                if (hashSet.Any())
                {
                     Device device = new Device(hashSet);
                    var send = client.SendNotification(device, content, attachment, null);
                }
           
               
           


            return messageId;
           


        }

        private static ContentAndLanguage BuildNotificationContent(Request model, string arMessage, out Attachment attachment)
        {
            Dictionary<string, string> contents = new Dictionary<string, string>();
            contents.Add("ar", arMessage);
            contents.Add("en", arMessage);

            Dictionary<string, string> headings = new Dictionary<string, string>();
            headings.Add("ar", "");
            headings.Add("en", "");

            ContentAndLanguage content = new ContentAndLanguage(contents, headings, null, false);


            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("Id", model.Id.ToString());

            attachment = new Attachment();
            attachment.data = data;
            return content;
        }

        private  long SaveMessageVsReturnsDeviceIds(Request model, int type, string arMessage, NotificationType notificationType,
            IQueryable<DeviceSetting> devicesList, ref HashSet<string> hashSet)
        {

           

            long messageId = 0;
            // notification will be sent to users 
            if (type < 4)
            {
                messageId = _UnitOfWork.NotificationDeviceRepository.Add(model.UserId, null,notificationType, arMessage, arMessage, model.Id, false);
                hashSet =
                    new HashSet<string>(devicesList.Where(u => u.DeviceId != null && u.ApplicationUserId == model.UserId && u.ApplicationUser.IsOnline).Select(x => x.DeviceId).Distinct().ToList());
            }
            //notification canceled by admin and notification will be sent to agent and user 
            else  if (type==4)
            {
                messageId = _UnitOfWork.NotificationDeviceRepository.Add(model.UserId, model.AgentId, notificationType, arMessage, arMessage, model.Id, false);
                hashSet = new HashSet<string>( devicesList.Where( u => u.DeviceId != null && u.ApplicationUserId == model.UserId || u.ApplicationUserId== model.AgentId && u.ApplicationUser.IsOnline ).Select(x => x.DeviceId).Distinct().ToList());
            }
            // notification will be sent to user when admin delete agent
            else if (type == 5)
            {
                messageId = _UnitOfWork.NotificationDeviceRepository.Add(model.UserId, null, notificationType, arMessage, arMessage, model.Id, false);
                hashSet = new HashSet<string>(devicesList.Where(u => u.DeviceId != null && u.ApplicationUserId == model.UserId && u.ApplicationUser.IsOnline ).Select(x => x.DeviceId).Distinct().ToList());
            }
            //new request and notifications will be sent to all agents suitable to this request
            if (type == 6)
            {
                List<string> agentids;
                if (model.Type == RequestTypeEnum.Request)
                {
                    agentids = (_repo.GetAllAgent())
                   .Where(x => (x.Type == (RequestTypesEnum)model.Type || x.Type == RequestTypesEnum.Both)
                   && x.CityId == model.CityId                
                   && x.IsNotified && x.IsOnline)
                   .Select(u => u.Id).ToList();
                }
                else
                {
                       agentids = (_repo.GetAllAgent())
                    .Where(x=>(x.Type== (RequestTypesEnum) model.Type || x.Type ==RequestTypesEnum.Both) 
                    && x.CityId== model.CityId &&
                    x.Car.PassengerNumberId>=model.PassengerNumberId 
                    && x.IsNotified && x.IsOnline)
                    .Select(u=>u.Id).ToList();
                }
              
                messageId = _UnitOfWork.NotificationDeviceRepository.Add(notificationType, arMessage, arMessage, model.Id, agentids, false);
                hashSet = new HashSet<string>( devicesList.Where(u => u.DeviceId != null && agentids.Contains(u.ApplicationUserId) && u.ApplicationUser.IsOnline && u.ApplicationUser.IsNotified).Select(x => x.DeviceId).Distinct().ToList());
            }
            // notification will be sent to agent when user cancel request          
            else if (type == 7)
            {
                messageId = _UnitOfWork.NotificationDeviceRepository.Add(model.AgentId, null,notificationType, arMessage, arMessage, model.Id, false);

                hashSet =
                    new HashSet<string>(  devicesList.Where(u => u.DeviceId != null && u.ApplicationUserId == model.AgentId && u.ApplicationUser.IsOnline ).Select(x => x.DeviceId).Distinct().ToList());
            }
         
             
            return messageId;
        }


      

        public string GetUserId()
        {
            ClaimsPrincipal principal = ClaimsPrincipal.Current;
            return principal.Claims.First(c => c.Type == "user_id").Value;
        }



        //public bool SendSms(string userName ,string phoneMobile,int emailType,string token)
        //{
        //    EmailSetting emailSettings = _UnitOfWork.EmailSetting.GetEmailSetting(emailType.ToString());
        //    string url=WebConfigurationManager.AppSettings["smsAPI"];
        //    EmailManager email =new EmailManager();
        //    string message;
        //    return email.SendSMS(url, phoneMobile, message);

        //}
       

    }
}
