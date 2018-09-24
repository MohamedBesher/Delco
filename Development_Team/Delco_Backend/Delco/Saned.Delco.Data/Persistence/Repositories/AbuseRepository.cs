using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Core.Repositories;

namespace Saned.Delco.Data.Persistence.Repositories
{
    public class AbuseRepository : BaseRepository<Abuse>, IAbuseRepository
    {

        public AbuseRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
