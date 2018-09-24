using System.Data.Entity.ModelConfiguration;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Persistence.EntityConfigurations
{
    public class NotificationDeviceConfigurations : EntityTypeConfiguration<NotificationDevice>
    {
        public NotificationDeviceConfigurations()
        {
            Property(u => u.NotificationMessageId).
                IsRequired();


            HasRequired(n => n.NotificationMessage)
                 .WithMany(u => u.NotificationDevices)
               .WillCascadeOnDelete(true);


            Property(u => u.ApplicationUserId).
                IsRequired();

           
            


            Property(u => u.Type).
                IsRequired();






        }
    }
}