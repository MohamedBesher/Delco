using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Saned.Delco.Api.Models;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Api.Controllers
{

    [RoutePrefix("api/RefuseRequest")]
    public class RefuseRequestController : BaseController
    {


        [HttpPost]
        [Route("RefuseRequests")]
        public async Task<IHttpActionResult> GetRefuseRequests(PagingViewModel model)
        {
            try
            {
                var refuseReasons = await _UnitOfWork.RefuseRequestsRepository.All().OrderBy(x => x.Id).Skip(model.PageSize * model.PageNumber)
                    .Take(model.PageSize).ToListAsync();

                return Ok(refuseReasons);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpGet]
        [Route("RefuseAgentCount")]
        [Authorize(Roles = "Agent")]
        public async Task<IHttpActionResult> GetRefuseAgentRequests()
        {
            try
            {
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName);
                var refuseCount =  _UnitOfWork.RefuseRequestsRepository.All().Count(x => x.AgentId == u.Id && x.UserId==null);
                return Ok(refuseCount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [Route("SaveRefuseAgentRequest")]
        [Authorize(Roles = "Agent")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveRefuseAgentRequest(RefuseRequestModel model)
        {
            string userName = User.Identity.GetUserName();
            ApplicationUser u = await GetApplicationUser(userName);
            model.AgentId = u.Id;
            var refuseRequest = Mapper.Map<RefuseRequestModel, RefuseRequest>(model);
            var count = _UnitOfWork.RefuseRequestsRepository.All().Count(x => x.AgentId == u.Id);

            var alreadyExists =_UnitOfWork.RefuseRequestsRepository.All()
                .Any(x => x.RequestId == model.RequestId && x.AgentId == model.AgentId);

            if (alreadyExists)
                return BadRequest($"Request Refused Before {count}");


                var result = _UnitOfWork.RefuseRequestsRepository.Create(refuseRequest);
                _UnitOfWork.Commit();
            var request = _UnitOfWork.RequestRepository.Find(model.RequestId);
            request.Status = RequestStatusEnum.New;
            request.AgentId = null;
            _UnitOfWork.RequestRepository.Update(request);
            await _UnitOfWork.CommitAsync();
            if (count >= 2)
            {
                  count += 1;
                    await _repo.SuspendAgent(u);
                    if (request != null)
                     await SendNotification(request, NotificationType.SuspendAgent.GetHashCode(),"");
            }
            return Ok(count);           
        }

        [Route("SaveRefuseUserRequest")]
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveRefuseUserRequest(RefuseRequestModel model)
        {
            string userName = User.Identity.GetUserName();
            ApplicationUser u = await GetApplicationUser(userName);
            model.UserId = u.Id;
            var refuseRequest = Mapper.Map<RefuseRequestModel, RefuseRequest>(model);
            var result = _UnitOfWork.RefuseRequestsRepository.Create(refuseRequest);
            _UnitOfWork.Commit();
           var request = _UnitOfWork.RequestRepository.Find(model.RequestId);

            if (request.Status == RequestStatusEnum.Inprogress)
            {
                var res = _UnitOfWork.RefuseReasonsRepository.Find(x => x.Id == model.RefuseReasonId);
                var notify = await SendNotification(request, NotificationType.CancelInProgressRequest.GetHashCode(), u.UserName, model.Cause + res.Value);

            }        
            request.Status = RequestStatusEnum.Canceled;
            _UnitOfWork.RequestRepository.Update(request);
            await _UnitOfWork.CommitAsync();
           

            return Ok();
        }
    }
}