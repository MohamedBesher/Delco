using System.Linq;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Core.Repositories;

namespace Saned.Delco.Data.Persistence.Repositories
{
    public class RefuseRequestsRepository : BaseRepository<RefuseRequest>, IRefuseRequestsRepository
    {
        public RefuseRequestsRepository(ApplicationDbContext context) : base(context)
        {

        }

        public void RemoveRange(IQueryable<RefuseRequest> userRefuseRequest)
        {
            _context.RefuseRequests.RemoveRange(userRefuseRequest);
        }
    }
}