using Saned.Delco.Api.Models;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Saned.Delco.Data.Core.Enum;

namespace Saned.Delco.Api.Controllers
{

    [RoutePrefix("api/Price")]
    public class PathController : BaseController
    {
        [Route("CalculateTripPrice")]
        [Attributes.Authorize(Roles = "User")]

        [HttpPost]
        public IHttpActionResult CalculateTripPrice(CalculateTripPriceViewModel model)
        {
            var key = ConfigurationManager.AppSettings["googleapis-map-key"];
            string apiRoute = "https://maps.googleapis.com/maps/api/distancematrix/json?origins={0},{1}&destinations={2},{3}&mode={4}&language={5}&key={6}";

            apiRoute = string.Format(apiRoute, model.FromLatitude, model.FromLongitude, model.ToLatitude, model.ToLongitude, "driving", "ar-sa", key);


            var request = WebRequest.Create(apiRoute);
            request.ContentType = "application/json; charset=utf-8";
            string text;
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }

            var result = System.Web.Helpers.Json.Decode(text);

            decimal disanceinKilometers = result.rows[0] != null && result.rows[0].elements[0] != null && result.rows[0].elements[0].distance != null && result.rows[0].elements[0].distance.value != null ?
                result.rows[0].elements[0].distance.value : 0;

            decimal timeInMints = result.rows[0] != null && result.rows[0].elements[0] != null && result.rows[0].elements[0].duration != null && result.rows[0].elements[0].duration.value != null ?
                result.rows[0].elements[0].duration.value : 0;
            disanceinKilometers = Math.Round(disanceinKilometers / 1000);
            timeInMints = Math.Round(timeInMints / 60);

            var setting = _UnitOfWork.SettingRepository.All().FirstOrDefault();
            decimal totalPrice = 0;
            if (setting != null)
            {
                if (disanceinKilometers > timeInMints)
                {
                  

                    totalPrice = ((setting.KiloMeterPrice * disanceinKilometers) + setting.StartPrice) ;
                    var managementPercentage = (totalPrice*setting.ManagementPercentage)/100;
                    totalPrice = totalPrice + managementPercentage;
                }
                else
                {
                    totalPrice = ((setting.MinutePrice * timeInMints) + setting.StartPrice );
                    var managementPercentage = (totalPrice * setting.ManagementPercentage) / 100;
                    totalPrice = totalPrice + managementPercentage;
                }


                if (totalPrice < setting.MinimumPrice)
                {
                    totalPrice = setting.MinimumPrice;
                }
            }
          

            return Ok(totalPrice);
        }


        [Route("CalculateRequestPrice")]
        [Attributes.Authorize(Roles = "User")]

        [HttpPost]
        public async Task<IHttpActionResult> CalculateOrderPrice(CalculateOrderViewModel model)
        {
            decimal totalPrice = 0;
            var cities = await _UnitOfWork.CityRepository.SelectNearByCities(model.ToLatitude, model.ToLongitude);
            if (cities.Any())
            {
                var isCityExist = cities.Any(x => x.Id == model.CityId);
               
                if (isCityExist)
                {

                    var internalPath = _UnitOfWork.PathRepository.Filter(x => x.Type == PathTypesEnum.Internal && x.FromCityId == model.CityId).FirstOrDefault();
                    if (internalPath != null)
                    {
                        totalPrice = internalPath.Price;
                    }
                    else
                    {
                        return BadRequest("هذا المسار غير مدعوم ");
                    }

                }
                else
                {
                    var tocity = cities.FirstOrDefault();
                    var externalPath = _UnitOfWork.PathRepository.Filter(x => x.Type == PathTypesEnum.External && x.FromCityId == model.CityId && x.ToCityId==tocity.Id).FirstOrDefault();
                    if (externalPath != null)
                    {
                        totalPrice = externalPath.Price;
                    }
                    else
                    {
                        return BadRequest("هذا المسار غير مدعوم ");
                    }

                }
            }
            else
            {
                return BadRequest("هذا المسار غير مدعوم ");
            }
         

            return Ok(totalPrice);
        }
    }
}