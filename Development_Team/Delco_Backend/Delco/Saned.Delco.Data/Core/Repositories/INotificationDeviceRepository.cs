using System.Collections.Generic;
using System.Threading.Tasks;
using Saned.Delco.Data.Core.Dto;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Persistence.Repositories;

namespace Saned.Delco.Data.Core.Repositories
{
    public interface INotificationDeviceRepository : IBaseRepository<NotificationDevice>
    {
        long Add(long messageId, string applicationUserId, NotificationType type,bool isRead=false);


        long Add(string applicationUserId, string agentId, NotificationType type, string arMessage, string enMessage, long requestId, bool isRead = false);


        long Add(NotificationType type,
            string arMessage, string enMessage, long requestId, List<string> agentsIds,
            bool isRead = false);

        Task<int> SaveNotificationDevice(long id, List<string> users);

        Task<List<NotificationDto>> GetUserNotifications(string userId,int pageSize,int pageNumber);




    }
}
