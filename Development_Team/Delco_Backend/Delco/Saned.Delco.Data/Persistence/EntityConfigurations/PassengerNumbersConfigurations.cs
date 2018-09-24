using System.Data.Entity.ModelConfiguration;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Persistence.EntityConfigurations
{
    public class PassengerNumbersConfigurations : EntityTypeConfiguration<PassengerNumber>
    {
        
        public PassengerNumbersConfigurations()
        {

            Property(u => u.Value).
                IsRequired();








        }

    }
}