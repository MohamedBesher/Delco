using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Core.Repositories;
using Saned.Delco.Data.Persistence;
using Saned.Delco.Data.Persistence.Repositories;

namespace Saned.Delco.Data.Persistence.Repositories
{
    public class PassengerNumberRepository : BaseRepository<PassengerNumber>, IPassengerNumberRepository
    {
        public PassengerNumberRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
