using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Core.Repositories;

namespace Saned.Delco.Data.Persistence.Repositories
{
    public class RefuseReasonsRepository : BaseRepository<RefuseReason>, IRefuseReasonsRepository
    {
        public RefuseReasonsRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}