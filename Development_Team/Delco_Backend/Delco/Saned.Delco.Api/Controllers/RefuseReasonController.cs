using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Saned.Delco.Api.Models;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Api.Controllers
{

    [RoutePrefix("api/RefuseReason")]
    public class RefuseReasonController : BaseController
    {

        [HttpGet]
        [Route("RefuseReasons")]

        public async Task<IHttpActionResult> GetRefuseReasons()
        {
            try
            {
                var refuseReasons = await _UnitOfWork.RefuseReasonsRepository.All().ToListAsync();

                return Ok(refuseReasons);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [Route("SaveRefuseReason")]
        [Attributes.Authorize(Roles = "Agent,User")]

        [HttpPost]
        public async Task<IHttpActionResult> SaveRefuseReason(RefuseReasonModel model)
        {
            string userName = User.Identity.GetUserName();
            ApplicationUser u = await GetApplicationUser(userName);

            var reason = Mapper.Map<RefuseReasonModel, RefuseReason>(model);
            var result = _UnitOfWork.RefuseReasonsRepository.Create(reason);
            _UnitOfWork.Commit();

            return Ok(result);
        }
    }
}