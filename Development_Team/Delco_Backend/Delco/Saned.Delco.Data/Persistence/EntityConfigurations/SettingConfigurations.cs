using System.Data.Entity.ModelConfiguration;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Persistence.EntityConfigurations
{
    public class SettingConfigurations : EntityTypeConfiguration<Setting>
    {
        public SettingConfigurations()
        {
            Property(u => u.UnSupportedPathMessage).
                IsRequired().HasMaxLength(1000);

            Property(u => u.SuspendAgentMessage).
                IsRequired().HasMaxLength(1000);

            Property(u => u.MinimumPrice).
                IsRequired();

            Property(u => u.StartPrice).
                IsRequired();

            Property(u => u.MinutePrice).
                IsRequired();

            Property(u => u.ManagementPercentage).
                IsRequired();


            Property(u => u.AbuseEmail).
                IsRequired();



            Property(u => u.ContactUsEmail).
                IsRequired();



            Property(u => u.TermsOfConditions).
                IsRequired(); //.HasMaxLength(1000);

        }
    }
}