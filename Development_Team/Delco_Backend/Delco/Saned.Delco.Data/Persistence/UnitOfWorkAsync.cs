using System;
using Saned.Delco.Data.Core;
using Saned.Delco.Data.Core.Repositories;
using Saned.Delco.Data.Persistence.Repositories;
using System.Threading.Tasks;
using Saned.Delco.Data.Core.Dto;

namespace Saned.Delco.Data.Persistence
{
    public class UnitOfWorkAsync : IUnitOfWorkAsync
    {
        ApplicationDbContext _context;
        public IEmailSettingRepository EmailSetting { get; }
        public IAbuseRepository AbuseRepository { get; }
        public ICarRepository CarRepository { get; }
        public ICityRepository CityRepository { get; }
        public IContactUsRepository ContactUsRepository { get; }
        public IDeviceSettingRepository DeviceSettingRepository { get; }
        public INotificationDeviceRepository NotificationDeviceRepository { get; }
        public INotificationMessageRepository NotificationMessageRepository { get; }
        public IPathRepository PathRepository { get; }
        public IRatingRepository RatingRepository { get; }
        public IRequestRepository RequestRepository { get; }
        public ISettingRepository SettingRepository { get; }
        public IPassengerNumberRepository PassengerNumberRepository { get;}
        public IRefuseRequestsRepository RefuseRequestsRepository { get; }
        public IRefuseReasonsRepository RefuseReasonsRepository { get; }

        public UnitOfWorkAsync()
        {
            _context = new ApplicationDbContext();
            EmailSetting = new EmailSettingRepository(_context);
            AbuseRepository = new AbuseRepository(_context);
            CarRepository = new CarRepository(_context);
            CityRepository = new CityRepository(_context);
            ContactUsRepository = new ContactUsRepository(_context);
            DeviceSettingRepository = new DeviceSettingRepository(_context);
            NotificationDeviceRepository = new NotificationDeviceRepository(_context);
            NotificationMessageRepository = new NotificationMessageRepository(_context);
            PathRepository = new PathRepository(_context);
            RatingRepository = new RatingRepository(_context);
            RequestRepository = new RequestRepository(_context);
            SettingRepository = new SettingRepository(_context);
             PassengerNumberRepository = new PassengerNumberRepository(_context);
            RefuseRequestsRepository = new RefuseRequestsRepository(_context);

            RefuseReasonsRepository = new RefuseReasonsRepository(_context);
        }
        public UnitOfWorkAsync(ApplicationDbContext context)
        {
            _context = context;
            EmailSetting = new EmailSettingRepository(context);
            AbuseRepository = new AbuseRepository(context);
            CarRepository = new CarRepository(context);
            CityRepository = new CityRepository(context);
            ContactUsRepository = new ContactUsRepository(context);
            DeviceSettingRepository = new DeviceSettingRepository(context);
            NotificationDeviceRepository = new NotificationDeviceRepository(context);
            NotificationMessageRepository = new NotificationMessageRepository(context);
            PathRepository = new PathRepository(context);
            RatingRepository = new RatingRepository(context);
            RequestRepository = new RequestRepository(context);
            SettingRepository = new SettingRepository(context);
            PassengerNumberRepository = new PassengerNumberRepository(_context);
            RefuseRequestsRepository = new RefuseRequestsRepository(_context);
            RefuseReasonsRepository = new RefuseReasonsRepository(_context);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (this._context == null)
            {
                return;
            }

            this._context.Dispose();
            this._context = null;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public Task<int> CommitAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}