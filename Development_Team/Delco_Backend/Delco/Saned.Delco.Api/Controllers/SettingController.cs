using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Saned.Delco.Api.Models;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Api.Controllers
{

    [RoutePrefix("api/Setting")]
    public class SettingController : BaseController
    {
        [HttpPost]
        [Route("GetSetting")]
        public async Task<IHttpActionResult> GetSetting(PagingViewModel model)
        {
            try
            {
                var setting = await _UnitOfWork.SettingRepository.All().FirstAsync();

                return Ok(setting);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }


        [HttpGet]
        [Route("TermsOfConditions")]
        public  IHttpActionResult GetTermsOfConditions()
        {
            try
            {
                var termsOfConditions =  _UnitOfWork.SettingRepository.GetTermsOfConditions();

                return Ok(termsOfConditions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }


        [Route("SaveSetting")]
        [HttpPost]
        public IHttpActionResult SaveSetting(SettingViewModel model)
        {
            var settingModel = Mapper.Map<SettingViewModel, Setting>(model);
            var result = _UnitOfWork.SettingRepository.Create(settingModel);
            _UnitOfWork.Commit();
            return Ok(result);
        }


    }
}