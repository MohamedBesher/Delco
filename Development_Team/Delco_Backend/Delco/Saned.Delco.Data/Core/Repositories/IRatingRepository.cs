using System.Linq;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Core.Repositories
{
    public interface IRatingRepository : IBaseRepository<Rating>
    {
        void RemoveRange(IQueryable<Rating> agentRatingUsers);
    }
}
