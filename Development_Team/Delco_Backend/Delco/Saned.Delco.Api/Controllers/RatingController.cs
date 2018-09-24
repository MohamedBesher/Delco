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

    [RoutePrefix("api/Rating")]
    public class RatingController : BaseController
    {
        [Route("SaveRating")]
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveRating(RatingViewModel model)
        {
            try
            {
                //string userName = User.Identity.GetUserName();
                //ApplicationUser user = await GetApplicationUser(userName);

               
                model.UserId = GetUserId();

                var rating=_UnitOfWork.RatingRepository.All().FirstOrDefault(u => u.AgentId == model.AgentId && u.UserId == model.UserId);
                if (rating != null)
                {
                 rating.Degree = model.Degree;
                _UnitOfWork.Commit();
                 return Ok(rating);


                }
                else
                {
                  var rate = Mapper.Map<RatingViewModel, Rating>(model);
                  var result = _UnitOfWork.RatingRepository.Create(rate);
                    _UnitOfWork.Commit();

                    return Ok(result);

                }


               
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
        }

        
    }
}