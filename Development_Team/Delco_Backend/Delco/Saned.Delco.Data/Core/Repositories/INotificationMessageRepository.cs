using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Core.Repositories
{
    public interface INotificationMessageRepository : IBaseRepository<NotificationMessage>
    {


       long Add(string message,string enMessage,Request requestId);


    }
}
