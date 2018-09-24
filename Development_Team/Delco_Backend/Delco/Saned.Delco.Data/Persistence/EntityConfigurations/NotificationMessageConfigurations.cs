using System.Data.Entity.ModelConfiguration;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Persistence.EntityConfigurations
{
    public class NotificationMessageConfigurations : EntityTypeConfiguration<NotificationMessage>
    {
        public NotificationMessageConfigurations()
        {
            Property(u => u.Message).
                IsRequired().HasMaxLength(500);


            HasRequired(n => n.Request)
               .WithMany(u => u.NotificationMessages)
               .WillCascadeOnDelete(true);



        }
    }
}