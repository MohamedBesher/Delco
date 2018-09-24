using System.Data.Entity.ModelConfiguration;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Persistence.EntityConfigurations
{
    public class PathConfigurations : EntityTypeConfiguration<Path>
    {
        public PathConfigurations()
        {
            Property(u => u.Type).
                IsRequired();

            Property(u => u.FromCityId).
                IsRequired();


            //HasRequired(n => n.FromCity)
            //    .WithMany(u => u.Paths)
            //    .WillCascadeOnDelete(false);



        }
    }
}