using System;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Migrations;
using Saned.Delco.Data.Persistence.EntityConfigurations;
using Saned.Delco.Data.Persistence.Infrastructure;

namespace Saned.Delco.Data.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public DbSet<EmailSetting> EmailSettings { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Abuse> Abuses { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<ContactUs> ContactUses { get; set; }
        public DbSet<DeviceSetting> DeviceSettings { get; set; }
        public DbSet<NotificationMessage> NotificationMessages { get; set; }
        public DbSet<NotificationDevice> NotificationDevices { get; set; }

        
        public DbSet<Path> Paths { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<PassengerNumber> PassengerNumbers { get; set; }
        public DbSet<RefuseRequest> RefuseRequests { get; set; }
        public DbSet<RefuseReason> RefuseReasons { get; set; }


        static string _connection;
        public ApplicationDbContext() : base(_connection ?? "connectionString")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public ApplicationDbContext(string connectionString) : base(connectionString)
        {
            _connection = connectionString;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            var configuration = new Configuration();
            var migrator = new System.Data.Entity.Migrations.DbMigrator(configuration);
            if (migrator.GetPendingMigrations().Any())
            {
                migrator.Update();
            }
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Configurations.Add(new EmailSettingConfigurations());
            modelBuilder.Configurations.Add(new ClientConfigurations());
            modelBuilder.Configurations.Add(new RefreshTokenConfigurations());
            modelBuilder.Configurations.Add(new AbuseConfigurations());
            modelBuilder.Configurations.Add(new CarConfigurations());
            modelBuilder.Configurations.Add(new CityConfigurations());
            modelBuilder.Configurations.Add(new ContactUsConfigurations());
            modelBuilder.Configurations.Add(new DeviceSettingConfigurations());
            modelBuilder.Configurations.Add(new NotificationDeviceConfigurations());
            modelBuilder.Configurations.Add(new NotificationMessageConfigurations());
            modelBuilder.Configurations.Add(new PassengerNumbersConfigurations());         
            modelBuilder.Configurations.Add(new PathConfigurations());
            modelBuilder.Configurations.Add(new RatingConfigurations());
            modelBuilder.Configurations.Add(new RequestConfigurations());
            modelBuilder.Configurations.Add(new SettingConfigurations());
            modelBuilder.Configurations.Add(new RefuseRequestsConfigurations());
            modelBuilder.Configurations.Add(new RefuseReasonsConfigurations());
            
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
            modelBuilder.Types().Configure(t => t.MapToStoredProcedures());

            base.OnModelCreating(modelBuilder);
        }



    }



}
