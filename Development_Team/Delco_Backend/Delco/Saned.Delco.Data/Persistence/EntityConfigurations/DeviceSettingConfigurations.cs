using System.Data.Entity.ModelConfiguration;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Persistence.EntityConfigurations
{
    public class DeviceSettingConfigurations : EntityTypeConfiguration<DeviceSetting>
    {
        public DeviceSettingConfigurations()
        {
            Property(u => u.DeviceId).
                IsRequired();

            Property(u => u.ApplicationUserId).
                IsRequired();


            HasRequired(n => n.ApplicationUser)
             .WithMany(u => u.DeviceSettings)
             .WillCascadeOnDelete(true);



        }
    }
}