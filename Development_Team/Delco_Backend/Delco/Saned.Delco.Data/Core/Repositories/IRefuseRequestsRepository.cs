using System.Linq;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Core.Repositories
{
    public interface IRefuseRequestsRepository : IBaseRepository<RefuseRequest>
    {
        void RemoveRange(IQueryable<RefuseRequest> userRefuseRequest);
    }
}