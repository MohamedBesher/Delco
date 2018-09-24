using System.Data.Entity.ModelConfiguration;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Persistence.EntityConfigurations
{
    public class RatingConfigurations : EntityTypeConfiguration<Rating>
    {
        public RatingConfigurations()
        {
            Property(u => u.TotalDegree).
                IsRequired();


            Property(u => u.Degree).
                IsRequired();





            HasRequired(n => n.User)
                .WithMany(u => u.UserRatings)
                .WillCascadeOnDelete(true);



            HasRequired(n => n.Agent)
                .WithMany(u => u.AgentRatings)
                .WillCascadeOnDelete(false);


        }
    }
}