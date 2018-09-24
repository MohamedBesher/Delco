using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Saned.Delco.Api.Controllers;
using Saned.Delco.Api.Models;
using Saned.Delco.Data.Core;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Persistence;

namespace Saned.Delco.Api.Hubs
{
    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }


    [HubName("notificationhub")]
    public class NotificationHub : Hub
    {
        public override Task OnConnected()
        {
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }


        [HubMethodName("Hello")]
        public void Hello()
        {
            Clients.All.SendNotifications("HELLO");
        }

        [HubMethodName("joinToGroup")]
        public async void JoinToSpecificGroup(string group)
        {
            ErrorSaver.SaveError("Join To group -" + group);
            //logic creating group //userid1++"chat"idchat+userid2 
            // Clients.Group(groupid);
            // Groups.Add(group, Context.ConnectionId);
            await Groups.Add(Context.ConnectionId, group);
            //Clients.All.agantMove("daDDDDDDDDDDDDDDDS");
        }

        // this method will be called from the client, when the Agant move 
        [HubMethodName("trackingAgant")]
        public async void TrackingAgant(TrackViewModel model)
        {
            try
            {
               // ErrorSaver.SaveError("Track Agent -" + model.UserId + model.RequestId);
                // TrackViewModel model+
                // this method will send Longitude, Latitude to the other user
                await UpdateRequest(model);

               // ErrorSaver.SaveError("After Update Request -" + model.UserId + model.RequestId);

                Clients.Group(model.UserId + model.RequestId).agantMove(model);


               // ErrorSaver.SaveError(UserHandler.ConnectedIds.Count.ToString());


               // ErrorSaver.SaveError("After Group Agent Move -" + model.UserId + model.RequestId);

                Clients.All.agantMove(model);

               // ErrorSaver.SaveError("After Clients All Agent Move -" + model.UserId + model.RequestId);
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError("exeption thrown - " + e.Message);
            }
        }

        private async Task<TrackViewModel> UpdateRequest(TrackViewModel model)
        {
           // ErrorSaver.SaveError("Start Update Request");

            IUnitOfWorkAsync _UnitOfWork = new UnitOfWorkAsync();

            var request = _UnitOfWork.RequestRepository.GetbyId(model.RequestId);

            var key = ConfigurationManager.AppSettings["googleapis-map-key"];


            var result = _UnitOfWork.CityRepository.GetDifference(key, model.Latitude, model.Longitude,
                request.ToLatitude,
                request.ToLongtitude);
            var text = Json.Decode(result);

            decimal disanceinKilometers = text.rows[0] != null && text.rows[0].elements[0] != null &&
                                          text.rows[0].elements[0].distance != null &&
                                          text.rows[0].elements[0].distance.value != null
                ? text.rows[0].elements[0].distance.value
                : 0;

            //ErrorSaver.SaveError("After disanceinKilometers" + disanceinKilometers);

            model.Distance = disanceinKilometers;
            if (model.Distance <= 20 && request.Status == RequestStatusEnum.Inprogress)
            {
                request.Status = RequestStatusEnum.Delivered;
                _UnitOfWork.RequestRepository.Updated(request);
                await _UnitOfWork.CommitAsync();

                var baset = new BaseController();
                await baset.SendNotification(request, NotificationType.AgentDelivere.GetHashCode(),
                    request.Agent.UserName, "");
            }

            //ErrorSaver.SaveError("End Update Request");

            return model;
        }
    }


    public class NotificationHubHellper
    {
        public void SendToAllUsersInThisHub()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.clientFunctionHock();
            ;
        }

        public void SendToSpecificUser(TrackViewModel model)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.Group(model.UserId).agantMove(model.Longitude, model.Latitude);
        }

        public void SendToSpecificGroup(string groupid)
        {
            //logic creating group //userid1++"chat"idchat+userid2
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.Group(groupid).clientfounctionHock();
        }

        public void SendNotification(string msg, string userId)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.Group(userId).notificationMsg(msg);
        }

        public void MoveAgant(TrackViewModel model)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

            context.Clients.Group(model.UserId + model.RequestId).agantMove(model);
        }

        public void SendMessage()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.Hello("Hello");
            context.Clients.All.agantMove("agantMove");
        }
    }
}