using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Saned.Delco.Data.Core.Dto;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Core.Repositories
{
    public interface IRequestRepository : IBaseRepository<Request>
    {
        List<RequestListDto> GetUserRequest(string userId, RequestStatusEnum? status, int pageSize, int pageNumber);
        List<RequestListDto> GetAgentRequest(string userId, RequestStatusEnum? status, RequestTypeEnum? type, int pageSize, int pageNumber);

        List<RequestListDto> GetAllAgentRequest( RequestStatusEnum? status, RequestTypeEnum? type, int pageSize, int pageNumber, long cityId,string agentId);
        Task<RequestDetailsDto> GetRequestDetails(long id);


        IQueryable<RequestListDto> SearchRequests(string userId, string agentId, RequestTypeEnum? type,
            RequestStatusEnum? status, long cityId, string keyword);

        IQueryable<RequestListDto> SearchRequests(string userId, RequestTypeEnum? type,
            RequestStatusEnum? status, long cityId, string keyword);

        Request GetbyId(long id);
    }
}
