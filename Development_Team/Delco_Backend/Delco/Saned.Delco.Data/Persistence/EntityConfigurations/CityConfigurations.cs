using System.Data.Entity.ModelConfiguration;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Persistence.EntityConfigurations
{
    public class CityConfigurations : EntityTypeConfiguration<City>
    {
        public CityConfigurations()
        {
            HasKey(u => u.Id);

            Property(u => u.Name).
                IsRequired()
                .HasMaxLength(250);


            Property(u => u.Latitude).
                IsRequired()
                .HasMaxLength(50);

            Property(u => u.Longitude).
                IsRequired()
                .HasMaxLength(50);

           

        }
    }
}