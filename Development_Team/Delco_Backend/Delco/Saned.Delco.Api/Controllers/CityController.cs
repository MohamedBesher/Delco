using Saned.Delco.Api.Models;
using Saned.Delco.Data.Core;
using Saned.Delco.Data.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;

namespace Saned.Delco.Api.Controllers
{
    [RoutePrefix("api/City")]
    public class CityController : BaseController
    {
       
          [HttpPost]
        public async Task<IHttpActionResult> GetAll(PagingViewModel model)
        {
            try
            {
                var cities = await _UnitOfWork.CityRepository
                    .Filter(x => x.Name.ToLower()
                    .Contains(model.Keyword.ToLower()))
                    .OrderBy(x => x.Id).Skip(model.PageSize * model.PageNumber)
                    .Take(model.PageSize).ToListAsync();

                return Ok(cities);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                var cities = await _UnitOfWork.CityRepository.All().ToListAsync();
              
                return Ok(cities);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
    }
}
