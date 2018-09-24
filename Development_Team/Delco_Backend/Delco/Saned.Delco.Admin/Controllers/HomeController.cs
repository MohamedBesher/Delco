using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Saned.Delco.Admin.Models;
using Saned.Delco.Data.Core.Enum;

namespace Saned.Delco.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class HomeController : BasicController
    {
        public async Task<ViewResult> Index()
        {
            try
            {
                var userRole = await _repo.IsRoleByName(RolesEnum.User.ToString());
                var users =(await _repo.FindUsers(userRole.Id)).Where(u => u.PhoneNumberConfirmed).ToList();


                var agentsRole = await _repo.IsRoleByName(RolesEnum.Agent.ToString());
                var agents = (await _repo.FindUsers(agentsRole.Id)).Where(u=>u.PhoneNumberConfirmed).ToList();

                var requests = _UnitOfWork.RequestRepository.Filter(x => x.Type == RequestTypeEnum.Request);
                var trips = _UnitOfWork.RequestRepository.Filter(x => x.Type == RequestTypeEnum.Trip);


                var cities = _UnitOfWork.CityRepository.Count;

                var paths = _UnitOfWork.PathRepository.All();


                var role = await _repo.IsRoleByName(RolesEnum.Agent.ToString());
                var agentRequests = await _repo.FindNewAgentsByRoleId(role.Id);
                int agentRequestsCount =  agentRequests.Count();

                int agentRequestsUnSeenCount =  agentRequests.Count(u=>!u.RequestIsSeen);

                var model = new StatisticsModel()
                {
                    UserCount = users.Count,
                    AgentCount = agents.Count,

                    RequestNew = requests.Count(u => u.Status==RequestStatusEnum.New),
                    RequestInProgress = requests.Count(u => u.Status == RequestStatusEnum.Inprogress),
                    RequestCanceled = requests.Count(u => u.Status == RequestStatusEnum.Canceled),
                    RequestDelivered = requests.Count(u => u.Status == RequestStatusEnum.Delivered),

                    TripNew = trips.Count(u => u.Status == RequestStatusEnum.New),
                    TripInProgress = trips.Count(u => u.Status == RequestStatusEnum.Inprogress),
                    TripCanceled = trips.Count(u => u.Status == RequestStatusEnum.Canceled),
                    TripDelivered = trips.Count(u => u.Status == RequestStatusEnum.Delivered),

                    InternalPath=paths.Count(u=>u.Type==PathTypesEnum.Internal),
                    ExternalPath = paths.Count(u=>u.Type==PathTypesEnum.External),
                    AgentRequestsCount= agentRequestsCount,
                    AgentRequestsUnSeenCount = agentRequestsUnSeenCount,
                    CityCount= cities

                    //TripCount = trips.Count(),
                };
                return View(model);
            }
            catch (Exception ex)
            {

                return View();
            }
         
          
        }



        public ActionResult _GetUnSeenAgentRequest()
        {
            var role = Task.Run(() => _repo.IsRoleByName(RolesEnum.Agent.ToString())).Result;
            var agentRequests = Task.Run(() => _repo.FindNewAgentsByRoleId(role.Id)).Result;

            int agentRequestsCount = agentRequests.Count();
            int agentRequestsUnSeenCount = agentRequests.Count(u => !u.RequestIsSeen);
            ViewBag.UnSeenCount = agentRequestsUnSeenCount;
            return PartialView();

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}