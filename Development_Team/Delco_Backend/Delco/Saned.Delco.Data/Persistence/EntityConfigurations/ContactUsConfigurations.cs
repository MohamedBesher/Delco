using System.Data.Entity.ModelConfiguration;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Persistence.EntityConfigurations
{
    public class ContactUsConfigurations : EntityTypeConfiguration<ContactUs>
    {
        public ContactUsConfigurations()
        {
            Property(u => u.Message).
                IsRequired()
                .HasMaxLength(1000);

            Property(u => u.Title).
                IsRequired()
                .HasMaxLength(250);

            HasOptional(n => n.User)
            .WithMany(u => u.ContactUses)
            .WillCascadeOnDelete(true);
            //Property(u => u.UserId).IsRequired();
        }
    }
}