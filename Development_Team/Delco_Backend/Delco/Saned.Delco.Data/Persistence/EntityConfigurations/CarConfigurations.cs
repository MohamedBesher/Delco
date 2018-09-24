using System.Data.Entity.ModelConfiguration;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Persistence.EntityConfigurations
{
    public class CarConfigurations : EntityTypeConfiguration<Car>
    {
        public CarConfigurations()
        {

            HasKey(p => p.Id);

            Property(u => u.CompanyName).
               IsRequired()
               .HasMaxLength(250);


            Property(u => u.Type).
              IsRequired()
              .HasMaxLength(250);

            Property(u => u.Model).
              IsRequired()
              .HasMaxLength(250);

            Property(u => u.Color).
              IsRequired()
              .HasMaxLength(250);

            Property(u => u.PlateNumber).
              IsRequired()
              .HasMaxLength(250);

   

            HasRequired(c1 => c1.User).WithOptional(c2 => c2.Car).WillCascadeOnDelete(true);

          




        }
    }
}