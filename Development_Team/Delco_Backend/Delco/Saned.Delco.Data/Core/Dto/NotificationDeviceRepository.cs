using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Core.Repositories;
using Saned.Delco.Data.Extentions;
using Saned.Delco.Data.Persistence;
using Saned.Delco.Data.Persistence.Repositories;

namespace Saned.Delco.Data.Core.Dto
{
    public class NotificationDeviceRepository : BaseRepository<NotificationDevice>, INotificationDeviceRepository
    {
        public NotificationDeviceRepository(ApplicationDbContext context) : base(context)
        {

        }

        public long Add(long messageId, string applicationUserId, NotificationType type, bool isRead = false)
        {
            var message=_context.NotificationDevices.Add(new NotificationDevice()
            {
                NotificationMessageId = messageId,
                ApplicationUserId = applicationUserId,
                Type = type,
                IsRead = isRead
            });
            _context.SaveChanges();
            return message.Id;
        }

        public long Add(string applicationUserId,string agentId, NotificationType type,
            string arMessage, string enMessage, long requestId,
            bool isRead = false)

        {
            var message = _context.NotificationDevices.Add(new NotificationDevice()
            {
               
                ApplicationUserId = applicationUserId,
                Type = type,
                IsRead = isRead,
                
            });

            message.NotificationMessage = new NotificationMessage()
            {
                Message = arMessage,
                EnglishMessage = enMessage,
                RequestId = requestId
            };
            _context.SaveChanges();

            if (!string.IsNullOrEmpty(agentId))
                _context.NotificationDevices.Add(new NotificationDevice()
                {
                    NotificationMessageId = message.NotificationMessageId,
                    ApplicationUserId = agentId,
                    Type = type,
                    IsRead = isRead,

                });

            _context.SaveChanges();


            return message.Id;
        }


        public long Add(NotificationType type,
            string arMessage, string enMessage, long requestId,List<string> agentsIds ,
            bool isRead = false)

        {

           

            if (agentsIds.Any())
            {
                var notificationMessage = _context.NotificationMessages.Add(new NotificationMessage()
                {
                    Message = arMessage,
                    EnglishMessage = enMessage,
                    RequestId = requestId
                });

                _context.SaveChanges();

                var notificationDevices = agentsIds.Select(u => new NotificationDevice()
                {
                    ApplicationUserId = u.ToString(),
                    Type = type,
                    IsRead = isRead,
                    NotificationMessageId = notificationMessage.Id
                });

                _context.NotificationDevices.AddRange(notificationDevices);

                _context.SaveChanges();

                return notificationMessage.Id;


            }




            return 0;



        }


        public async Task<int> SaveNotificationDevice(long id, List<string> users)
        {
            foreach (string userId in users)
            {
                var bo = new NotificationDevice
                {
                    Id = 0,
                    NotificationMessageId = id,
                    ApplicationUserId = userId
                };
                Create(bo);

            }
            return await _context.SaveChangesAsync();
        }

        public async Task<List<NotificationDto>> GetUserNotifications(string userId, int pageSize, int pageNumber)
        {
            return await _context.NotificationDevices.Include(u => u.NotificationMessage)
                .OrderByDescending(u => u.NotificationMessage.CreationDate)
                .Where(u=>u.ApplicationUserId==userId)
                .Skip(pageSize * pageNumber)
                .Take(pageSize)
                .AsNoTracking()
                .Select(u => new NotificationDto()
                {
                    Message = u.NotificationMessage.Message,
                    Request = u.NotificationMessage.RequestId,
                    Type= u.Type,                 
                    IsRead = u.IsRead,
                    CreationDate = u.NotificationMessage.CreationDate

                }).ToListAsync();
                ;



        }
    }
}