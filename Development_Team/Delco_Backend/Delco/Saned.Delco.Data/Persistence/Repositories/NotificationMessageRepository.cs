using System;
using System.Collections.Generic;
using System.Data.Entity;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Persistence;
using Saned.Delco.Data.Persistence.Repositories;
using System.Linq;
using System.Threading.Tasks;
using OneSignalLibrary.Posting;
using Saned.Delco.Data.Core.Dto;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Core.Repositories;
using Saned.Delco.Data.Extentions;


namespace Saned.Delco.Data.Persistence.Repositories
{
    public class NotificationMessageRepository : BaseRepository<NotificationMessage>, INotificationMessageRepository
    {

        public NotificationMessageRepository(ApplicationDbContext context) : base(context)
        {

        }

        public long Add(string message, string enMessage, Request requestId)
        {          
            var notificationMessage = _context.NotificationMessages.Add(new NotificationMessage()
            {
                Message = message,
                EnglishMessage = message,
                RequestId = requestId.Id
            });
            _context.SaveChanges();
            return notificationMessage.Id;
        }

     



    }
}
