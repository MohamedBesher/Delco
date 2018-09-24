using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.UI.WebControls;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Saned.Delco.Api.Models;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Api.Attributes;

namespace Saned.Delco.Api.Controllers
{


    [RoutePrefix("api/Request")]
    public class RequestController : BaseController
    {

        [HttpPost]
        [Route("AllRequest")]
        public async Task<IHttpActionResult> GetAllRequests(RequestStatusViewModel model)
        {
            try
            {
                var request = await _UnitOfWork.RequestRepository.Filter(x => (x.Status == (RequestStatusEnum)model.Status || model.Status == null) && (x.Type == (RequestTypeEnum)model.Type || model.Type == null))
                    .OrderBy(x => x.Id).Skip(model.PageSize * model.PageNumber)
                    .Take(model.PageSize).ToListAsync();


                return Ok(request);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }




        [HttpPost]
        [Route("UserAllRequests")]
        [Attributes.Authorize(Roles = "User")]
        public async Task<IHttpActionResult> GetUserAllRequests(RequestStatusViewModel model)
        {
            try
            {
                var request = await _UnitOfWork.RequestRepository.Filter(x => (x.Status == RequestStatusEnum.New || x.Status == RequestStatusEnum.Inprogress))
                    .OrderBy(x => x.Id).Skip(model.PageSize * model.PageNumber)
                    .Take(model.PageSize).ToListAsync();


                return Ok(request);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPost]
        [Route("AgentAllRequests")]
        [Attributes.Authorize(Roles = "Agent")]
        public async Task<IHttpActionResult> GetAgentAllRequest(RequestStatusViewModel model)
        {
            try
            {
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName);
                var status = model.Status != null ? (RequestStatusEnum)model.Status : 0;
                // var type = model.Type != null ? (RequestTypeEnum)model.Type : 0;
                var type = RequestTypeEnum.Request;
                var request = _UnitOfWork.RequestRepository.GetAllAgentRequest(status, type, model.PageSize, model.PageNumber, u.CityId.Value, u.Id);

                return Ok(request);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }


        [HttpPost]
        [Route("UserRequest")]
        [Attributes.Authorize(Roles = "User")]
        public async Task<IHttpActionResult> GetUserRequest(RequestStatusViewModel model)
        {
            try
            {
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName);
                var status = model.Status != null ? (RequestStatusEnum)model.Status : 0;

                var type = model.Type != null ? (RequestTypeEnum)model.Type : 0;
                var request = _UnitOfWork.RequestRepository.GetUserRequest(u.Id, status,
                        model.PageSize, model.PageNumber);

                return Ok(request);



            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPost]
        [Route("AgentRequest")]
        [Attributes.Authorize(Roles = "Agent")]
        public async Task<IHttpActionResult> GetAgentRequest(RequestStatusViewModel model)
        {
            try
            {
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName);
                var status = model.Status != null ? (RequestStatusEnum)model.Status : 0;
                // var type = model.Type != null ? (RequestTypeEnum)model.Type : 0;
                var type = RequestTypeEnum.Request;
                var request = _UnitOfWork.RequestRepository.GetAgentRequest(u.Id, status, type, model.PageSize, model.PageNumber);

                return Ok(request);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPost]
        [Route("RequestDetails")]
        [Attributes.Authorize(Roles = "Agent,User")]

        public async Task<IHttpActionResult> GetRequestDetails(EditRequestViewModel model)
        {
            try
            {
                var request = await _UnitOfWork.RequestRepository.GetRequestDetails(model.Id);
                if (request == null)
                    return BadRequest($"Request's User Is Removed");

                return Ok(request);
            }
            catch (Exception ex)
            {
                ErrorSaver.SaveError(ex.Message);
                return BadRequest(ex.Message);

            }
        }



        [HttpGet]
        [Route("AgentHasInProgressRequest")]
        [Attributes.Authorize(Roles = "Agent")]

        public async Task<IHttpActionResult> AgentHasInProgressRequest()
        {
            try
            {
                string userId = GetUserId();
                ApplicationUser u = await GetApplicationUserById(userId:userId);
                var inUse =
                    _UnitOfWork.RequestRepository.All()
                        .Any(x => x.Status == RequestStatusEnum.Inprogress && x.AgentId == u.Id);
               
                  return Ok(inUse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }


        [Route("SaveRequest")]
        [Attributes.Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveRequest(RequestViewModel model)
        {
            try
            {
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName);
                model.UserId = u.Id;
                var requestModel = Mapper.Map<RequestViewModel, Request>(model);
                var result = _UnitOfWork.RequestRepository.Create(requestModel);

                await _UnitOfWork.CommitAsync();
                var notify = await SendNotification(result, NotificationType.AgentsuitableRequest.GetHashCode(), userName);

                result.Agent = null;
                result.City = null;
                result.User = null;
                result.NotificationMessages = null;

                return Ok(result);
            }
            catch (Exception ex)
            {

                string msg = ex.Message;
                return BadRequest(msg);
            }

        }

        [Route("UpdateRequest")]
        [Attributes.Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateRequest(RequestViewModel model)
        {
            var requestModel = Mapper.Map<RequestViewModel, Request>(model);
            var result = _UnitOfWork.RequestRepository.Updated(requestModel);
            await _UnitOfWork.CommitAsync();
            return Ok(result);
        }

        [Route("AproveRequest")]
        [Attributes.Authorize(Roles = "Agent")]
        [HttpPost]
        public async Task<IHttpActionResult> AproveRequest(EditRequestViewModel model)
        {
            try
            {
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName);


                var request = _UnitOfWork.RequestRepository.Find(model.Id);

                //  request.Status = RequestStatusEnum.Approved;
                request.AgentId = u.Id;
                // send from agent to user
                var notify = await SendNotification(request, NotificationType.AgentReceivedRequest.GetHashCode(), null);

                await _UnitOfWork.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [Route("ConfirmRequest")]
        [Attributes.Authorize(Roles = "Agent")]
        [HttpPost]
        public async Task<IHttpActionResult> ConfirmRequest(EditRequestViewModel model)
        {
            try
            {
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName);
                var request = _UnitOfWork.RequestRepository.Find(model.Id);
                var inUse =
                    _UnitOfWork.RequestRepository.All()
                        .Any(x => x.Status == RequestStatusEnum.Inprogress && x.AgentId == u.Id);
                if (inUse)
                    return BadRequest("This Agent Already in Joined in Another Request");
                request.AgentId = u.Id;

                request.Status = RequestStatusEnum.Inprogress;

                var notify = await SendNotification(request, NotificationType.AgentReceivedRequest.GetHashCode(), userName);
                await _UnitOfWork.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }


        [Route("InProgressRequest")]
        [Attributes.Authorize(Roles = "Agent")]
        [HttpPost]
        public async Task<IHttpActionResult> InProgressRequest(EditRequestViewModel model)
        {
            try
            {
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName);
                var request = _UnitOfWork.RequestRepository.Find(model.Id);
                request.Status = RequestStatusEnum.Inprogress;
                await _UnitOfWork.CommitAsync();
                return Ok(request);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }





        [Route("CancelAgentRequest")]
        [Attributes.Authorize(Roles = "Agent")]
        [HttpPost]
        public async Task<IHttpActionResult> CancelAgentRequest(EditRequestViewModel model)
        {
            try
            {

                var request = _UnitOfWork.RequestRepository.Find(model.Id);
                request.Status = RequestStatusEnum.New;
                request.AgentId = null;
                await _UnitOfWork.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }



    }
}