using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Saned.Delco.Api.Hubs;
using Saned.Delco.Api.Models;
using Saned.Delco.Data.Core.Enum;

namespace Saned.Delco.Api.Controllers
{
    [RoutePrefix("Tracking")]
    public class TrackingController : BaseController
    {
        readonly NotificationHubHellper _notificationHub = new NotificationHubHellper();
        [HttpPost]
        public async Task<IHttpActionResult> TrackAgent(TrackViewModel model)
        {

            
            var request= _UnitOfWork.RequestRepository.GetbyId(model.RequestId);
   
            var key = ConfigurationManager.AppSettings["googleapis-map-key"];


            var res =  _UnitOfWork.CityRepository.GetDifference(key, model.Latitude, model.Longitude, request.ToLatitude,
                request.ToLongtitude);
            var result = System.Web.Helpers.Json.Decode(res);

            ErrorSaver.SaveError($"Log Model before {key}, {model.Latitude}, {model.Longitude}, {request.ToLatitude},{request.ToLongtitude}");


            decimal disanceinKilometers = result.rows[0] != null && result.rows[0].elements[0] != null && result.rows[0].elements[0].distance != null && result.rows[0].elements[0].distance.value != null ?
                result.rows[0].elements[0].distance.value : 0;
            model.Distance =disanceinKilometers ;

            ErrorSaver.SaveError($"Distance before {disanceinKilometers}");

            if (model.Distance <= 100)
            {
                request.Status = RequestStatusEnum.Delivered;
                _UnitOfWork.RequestRepository.Updated(request);
                await _UnitOfWork.CommitAsync();

                ErrorSaver.SaveError($"Distance Less than 100 {disanceinKilometers}");

            }
            _notificationHub.MoveAgant(model);
            return Ok(model);
        }

        [HttpGet]
        public string  Get()
        {

            _notificationHub.SendMessage();
            return "done";
        }

    }
}
