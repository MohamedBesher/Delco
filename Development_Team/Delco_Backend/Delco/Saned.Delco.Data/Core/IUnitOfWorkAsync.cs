using System;
using Saned.Delco.Data.Core.Repositories;
using System.Threading.Tasks;

namespace Saned.Delco.Data.Core
{
    public interface IUnitOfWorkAsync : IDisposable
    {
        IEmailSettingRepository EmailSetting { get; }
        IAbuseRepository AbuseRepository { get; }
        ICarRepository CarRepository { get; }
        ICityRepository CityRepository { get; }
        IContactUsRepository ContactUsRepository { get; }
        IDeviceSettingRepository DeviceSettingRepository { get; }
        INotificationDeviceRepository NotificationDeviceRepository { get; }
        INotificationMessageRepository NotificationMessageRepository { get; }
        IPathRepository PathRepository { get; }
        IRatingRepository RatingRepository { get; }
        IRequestRepository RequestRepository { get; }
        ISettingRepository SettingRepository { get; }
        IPassengerNumberRepository PassengerNumberRepository { get; }
        IRefuseRequestsRepository RefuseRequestsRepository { get; }
        IRefuseReasonsRepository RefuseReasonsRepository { get; }
        

        void Commit();

        Task<int> CommitAsync();
    }
}