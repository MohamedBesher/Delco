using System.Data.Entity;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<EmailSetting> EmailSettings { get; set; }
        DbSet<Client> Clients { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        DbSet<Abuse> Abuses { get; set; }
        DbSet<Car> Cars { get; set; }
        DbSet<City> Cities { get; set; }
        DbSet<ContactUs> ContactUses { get; set; }
        DbSet<DeviceSetting> DeviceSettings { get; set; }
        DbSet<NotificationDevice> NotificationDevices { get; set; }

        DbSet<NotificationMessage> NotificationMessages { get; set; }
        DbSet<Path> Paths { get; set; }
        DbSet<Rating> Ratings { get; set; }
        DbSet<Request> Requests { get; set; }
        DbSet<Setting> Settings { get; set; }
        DbSet<PassengerNumber> PassengerNumbers { get; set; }
        DbSet<RefuseRequest> RefuseRequests { get; set; }
        DbSet<RefuseReason> RefuseReasons { get; set; }
        void Dispose();
    }
}
