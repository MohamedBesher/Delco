using System.Data.Entity.ModelConfiguration;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Persistence.EntityConfigurations
{
    public class EmailSettingConfigurations : EntityTypeConfiguration<EmailSetting>
    {
        public EmailSettingConfigurations()
        {
            Property(u => u.Host)
                .HasMaxLength(150);

            Property(u => u.FromEmail)
                .HasMaxLength(150);

            Property(u => u.Password)
                .HasMaxLength(150);

            Property(u => u.SubjectAr)
                .HasMaxLength(150);

            Property(u => u.SubjectEn)
                .HasMaxLength(150);

            Property(u => u.EmailSettingType)
                .HasMaxLength(10);

        }

    }
}