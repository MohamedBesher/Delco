using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Saned.Delco.Api.Models;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Persistence.Repositories;

namespace Saned.Delco.Api.Controllers
{
    [RoutePrefix("api/Abuse")]
    public class AbuseController : BaseController
    {
        [HttpPost]
        [Route("GetAbuse")]
        public async Task<IHttpActionResult> GetAbuse(PagingViewModel model)
        {
            try
            {
                var abuse = await _UnitOfWork.AbuseRepository.All().OrderBy(x => x.Id).Skip(model.PageSize * model.PageNumber)
                    .Take(model.PageSize).ToListAsync();

                return Ok(abuse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }



        [Route("SaveAbuse")]
        [Authorize(Roles = "User , Agent")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveAbuse(AbuseViewModel model)
        {
            string userName = User.Identity.GetUserName();
            ApplicationUser u = await GetApplicationUser(userName);
            var setting = _UnitOfWork.SettingRepository.GetSetting();

            EmailSetting emailSettings = _UnitOfWork.EmailSetting.GetEmailSetting(EmailType.AbuseEmail.GetHashCode().ToString());
           //Title //emailSettings.SubjectAr 

            var messageBody = emailSettings.MessageBodyAr.Replace("@title", emailSettings.SubjectAr);
            messageBody = messageBody.Replace("@userName", userName);
            messageBody = messageBody.Replace("@subject", model.Title);
            messageBody = messageBody.Replace("@body", model.Message);


            EmailManager.SendAEmail(emailSettings.Host,emailSettings.Port,emailSettings.FromEmail,emailSettings.Password,
                                     setting.ContactUsEmail, messageBody, $"بلاغ إساءة من المستخدم {userName} ");


            model.UserId = u.Id;
            var abuse = Mapper.Map<AbuseViewModel, Abuse>(model);
            var result = _UnitOfWork.AbuseRepository.Create(abuse);
            _UnitOfWork.Commit();
           
           


            return Ok(result);
        }

    }
}
