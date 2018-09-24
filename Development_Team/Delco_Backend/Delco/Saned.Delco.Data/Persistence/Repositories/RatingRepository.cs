using System.Linq;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Core.Repositories;
using Saned.Delco.Data.Persistence;
using Saned.Delco.Data.Persistence.Repositories;

namespace Saned.Delco.Data.Persistence.Repositories
{
    public class RatingRepository : BaseRepository<Rating>, IRatingRepository
    {
        public RatingRepository(ApplicationDbContext context) : base(context)
        {

        }

        public void RemoveRange(IQueryable<Rating> agentRatingUsers)
        {
            _context.Ratings.RemoveRange(agentRatingUsers);
        }
    }
}
