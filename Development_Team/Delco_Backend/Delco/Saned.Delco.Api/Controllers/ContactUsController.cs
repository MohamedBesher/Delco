using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Saned.Delco.Api.Models;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Persistence.Repositories;
using Saned.Delco.Data.Persistence.Tools;

namespace Saned.Delco.Api.Controllers
{
    [RoutePrefix("api/ContactUs")]
    public class ContactUsController : BaseController
    {

        [HttpPost]
        [Route("GetContactUs")]
        public async Task<IHttpActionResult> GetSetting(PagingViewModel model)
        {
            try
            {
                var contactUs = await _UnitOfWork.ContactUsRepository.All().OrderBy(x => x.Id).Skip(model.PageSize * model.PageNumber)
                    .Take(model.PageSize).ToListAsync();

                return Ok(contactUs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }



        [Route("SaveContactUs")]
        //[Authorize(Roles = "User , Agent")]

        [HttpPost]
        public async Task<IHttpActionResult> SaveContactUs(ContactUsViewModel model)
        {
            
            string userName = User.Identity.GetUserName();
            if (!string.IsNullOrEmpty(userName))
            {
               ApplicationUser u = await GetApplicationUser(userName);
               model.UserId = u.Id;
            }
            else
            {
                userName = "غير مسجل";
            }
            

            var setting = _UnitOfWork.SettingRepository.GetSetting();

            EmailSetting emailSettings = _UnitOfWork.EmailSetting.GetEmailSetting(EmailType.ContactUs.GetHashCode().ToString());
            var messageBody = emailSettings.MessageBodyAr.Replace("@title", emailSettings.SubjectAr);
            messageBody = messageBody.Replace("@userName", userName);
            messageBody = messageBody.Replace("@subject", model.Title);
            messageBody = messageBody.Replace("@body", model.Message);
            

            EmailManager.SendAEmail(emailSettings.Host, emailSettings.Port, emailSettings.FromEmail, emailSettings.Password,
                                   setting.ContactUsEmail, messageBody, $"اتصل بنا من المستخدم {userName} ");

            var settingModel = Mapper.Map<ContactUsViewModel, ContactUs>(model);
            var result = _UnitOfWork.ContactUsRepository.Create(settingModel);
            _UnitOfWork.Commit();
           

            return Ok(result);
        }


    }
}