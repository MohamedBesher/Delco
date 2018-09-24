using System.Data.Entity.ModelConfiguration;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Persistence.EntityConfigurations
{
    public class AbuseConfigurations : EntityTypeConfiguration<Abuse>
    {
        public AbuseConfigurations()
        {

            Property(u => u.Message).
               IsRequired()
               .HasMaxLength(1000);


            Property(u => u.Title).
               IsRequired()
               .HasMaxLength(150);


            HasRequired(n => n.User)
              .WithMany(u => u.Abuses)
              .WillCascadeOnDelete(true);




        }
      
        
    }
}