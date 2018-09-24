using System.Data.Entity.ModelConfiguration;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Persistence.EntityConfigurations
{
   
        public class RequestConfigurations : EntityTypeConfiguration<Request>
        {
            public RequestConfigurations()
            {
                Property(u => u.ToLongtitude).
                    IsRequired();


                Property(u => u.ToLatitude).
                   IsRequired();


                Property(u => u.ToLocation).
                  IsRequired();


                Property(u => u.Status).
                  IsRequired();

                Property(u => u.Price).
                    IsRequired();

            //HasRequired(n => n.User)
            //   .WithMany(u => u.Requests)
            //   .WillCascadeOnDelete(false);



            HasOptional(x => x.PassengerNumber).WithMany(x => x.Requestes);
              




            }
        }
}