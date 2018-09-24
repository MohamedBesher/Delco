using System.Data.Entity.ModelConfiguration;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Persistence.EntityConfigurations
{
    public class RefuseRequestsConfigurations : EntityTypeConfiguration<RefuseRequest>
    {

        public RefuseRequestsConfigurations()
        {
            HasOptional(n => n.User)
             .WithMany(u => u.RefuseRequests)
             .WillCascadeOnDelete(true);

        }

    }
}