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
    [RoutePrefix("api/PassengerNumber")]
    public class PassengerNumberController : BaseController
    {


        [HttpGet]
        [Route("PassengerNumbers")]

        public IHttpActionResult GetPassengerNumbers()
        {
            try
            {
                var request = _UnitOfWork.PassengerNumberRepository.All();
                return Ok(request);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }



       
        [HttpPost]
        [Route("SavePassengerNumbers")]
        public async Task<IHttpActionResult> SaveRequest(PassengerNumbersViewModel model)
        {           
            var passengerNumbersModel = Mapper.Map<PassengerNumbersViewModel, PassengerNumber>(model);
            var result = _UnitOfWork.PassengerNumberRepository.Create(passengerNumbersModel);
            await _UnitOfWork.CommitAsync();
            return Ok(result);
        }
    }
}
