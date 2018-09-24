using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Provider;
using Saned.Delco.Data.Core.Dto;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Core.Repositories;
using Saned.Delco.Data.Persistence;
using Saned.Delco.Data.Persistence.Repositories;

namespace Saned.Delco.Data.Persistence.Repositories
{
    public class RequestRepository : BaseRepository<Request>, IRequestRepository
    {

        public RequestRepository(ApplicationDbContext context) : base(context)
        {

           

        }


        public IQueryable<RequestListDto> SearchRequests(string userId, string agentId, RequestTypeEnum? type,
            RequestStatusEnum? status, long cityId, string keyword)
        {
            if (status.HasValue && status.Value == RequestStatusEnum.New)
            {
                var requests = _context.Requests
                    .Include(u => u.User)
                    .Include(u => u.City)

                .Where(res => (string.IsNullOrEmpty(userId) || res.UserId == userId) &&
                                  (type == 0 || res.Type == type) &&
                                  (status == 0 || res.Status == status)  &&
                        (res.CityId == cityId || cityId == 0) &&
                        (res.Description.Contains(keyword) || res.Address.Contains(keyword) ||
                         res.ToLocation.Contains(keyword) || res.FromLocation.Contains(keyword) ||
                         string.IsNullOrEmpty(keyword))
                    )
                    .OrderBy(res => res.Id);

                return requests.Select(u => new RequestListDto()
               {
                   Id = u.Id,
                   Address = u.Address,
                   AgentId = u.AgentId,
                   UserId = u.UserId,
                   AgentUserName = u.Agent.UserName,
                   AgentFullName = u.Agent.FullName,
                   UserName = u.User.UserName,
                   UserFullName = u.User.FullName,
                   CityId = u.CityId,
                   CityName = u.City.Name,
                   Status = u.Status,
                   Type = u.Type,
                   ToLocation = u.ToLocation,
                   FromLocation = u.FromLocation,
                   CreatedDate = u.CreatedDate,
                   Price = u.Price,
                   PassengerNumber = u.PassengerNumber,
                   IsHidden = u.IsHidden,
                   //Description = u.Description
               });

            }



            return _context.Requests
                .Include(u => u.User)
                .Include(u => u.Agent)
                .Include(u => u.City)
                .Where(res => (res.UserId == userId || string.IsNullOrEmpty(userId)) &&
                              (res.AgentId == agentId || string.IsNullOrEmpty(agentId)) &&
                              (res.Type == type || type == 0) &&
                              (res.Status == status || status == 0) &&
                              (res.CityId == cityId || cityId == 0)
                              &&
                              (res.Description.Contains(keyword) || res.Address.Contains(keyword) ||
                               res.ToLocation.Contains(keyword) || res.FromLocation.Contains(keyword) ||
                               string.IsNullOrEmpty(keyword))
                )
                .OrderBy(res => res.Id)
                .Select(u => new RequestListDto()
                {
                    Id = u.Id,
                    Address = u.Address,
                    AgentId = u.AgentId,
                    UserId = u.UserId,
                    AgentUserName = u.Agent.UserName,
                    AgentFullName = u.Agent.FullName,
                    UserName = u.User.UserName,
                    UserFullName = u.User.FullName,
                    CityId = u.CityId,
                    CityName = u.City.Name,
                    Status = u.Status,
                    Type = u.Type,
                    ToLocation = u.ToLocation,
                    FromLocation = u.FromLocation,
                    CreatedDate = u.CreatedDate,
                    Price = u.Price,
                    PassengerNumber = u.PassengerNumber,
                    IsHidden = u.IsHidden,
                });


        }



        public IQueryable<RequestListDto> SearchRequests(string userId, RequestTypeEnum? type,
           RequestStatusEnum? status, long cityId, string keyword)
        {
            if (status.HasValue && status.Value == RequestStatusEnum.New)
            {
                var requests = _context.Requests
                    .Include(u => u.User)
                    .Include(u => u.City)

                .Where(res => (string.IsNullOrEmpty(userId) || res.UserId == userId) &&
                                  (type == 0 || res.Type == type) &&
                                  (status == 0 || res.Status == status) &&
                        (res.CityId == cityId || cityId == 0) &&
                        (res.Description.Contains(keyword) || res.Address.Contains(keyword) ||
                         res.ToLocation.Contains(keyword) || res.FromLocation.Contains(keyword) ||
                         string.IsNullOrEmpty(keyword))
                    )
                    .OrderBy(res => res.Id);

                return requests.Select(u => new RequestListDto()
                {
                    Id = u.Id,
                    Address = u.Address,
                    AgentId = u.AgentId,
                    UserId = u.UserId,
                    AgentUserName = u.Agent.UserName,
                    AgentFullName = u.Agent.FullName,
                    UserName = u.User.UserName,
                    UserFullName = u.User.FullName,
                    CityId = u.CityId,
                    CityName = u.City.Name,
                    Status = u.Status,
                    Type = u.Type,
                    ToLocation = u.ToLocation,
                    FromLocation = u.FromLocation,
                    CreatedDate = u.CreatedDate,
                    Price = u.Price,
                    PassengerNumber = u.PassengerNumber,
                    IsHidden = u.IsHidden,
                    //Description = u.Description
                });

            }



            return _context.Requests
                .Include(u => u.User)
                .Include(u => u.Agent)
                .Include(u => u.City)
                .Where(res => (res.UserId == userId || res.AgentId == userId || string.IsNullOrEmpty(userId)) &&
                             
                              (res.Type == type || type == 0) &&
                              (res.Status == status || status == 0) &&
                              (res.CityId == cityId || cityId == 0)
                              &&
                              (res.Description.Contains(keyword) || res.Address.Contains(keyword) ||
                               res.ToLocation.Contains(keyword) || res.FromLocation.Contains(keyword) ||
                               string.IsNullOrEmpty(keyword))
                )
                .OrderBy(res => res.Id)
                .Select(u => new RequestListDto()
                {
                    Id = u.Id,
                    Address = u.Address,
                    AgentId = u.AgentId,
                    UserId = u.UserId,
                    AgentUserName = u.Agent.UserName,
                    AgentFullName = u.Agent.FullName,
                    UserName = u.User.UserName,
                    UserFullName = u.User.FullName,
                    CityId = u.CityId,
                    CityName = u.City.Name,
                    Status = u.Status,
                    Type = u.Type,
                    ToLocation = u.ToLocation,
                    FromLocation = u.FromLocation,
                    CreatedDate = u.CreatedDate,
                    Price = u.Price,
                    PassengerNumber = u.PassengerNumber,
                    IsHidden = u.IsHidden,
                });


        }

        public List<RequestListDto> GetUserRequest(string userId, RequestStatusEnum? status, int pageSize, int pageNumber)
        {
             IQueryable<Request> queryable;

            if (status == null || status == 0)
            {
                var newRequests = _context.Requests
                    .Include(u => u.User)
                    .Include(u => u.Agent)
                    .Include(u => u.City)
                    .Where(res => (res.UserId == userId) && res.Type == RequestTypeEnum.Request
                                  && (res.Status == RequestStatusEnum.New))
                    .OrderByDescending(res => res.Id)
                    .Skip((pageSize / 2) * pageNumber)
                    .Take(pageSize / 2);

                var inProgressRequests = _context.Requests
                    .Include(u => u.User)
                    .Include(u => u.Agent)
                    .Include(u => u.City)
                    .Where(res => (res.UserId == userId) && res.Type == RequestTypeEnum.Request
                                  && (res.Status == RequestStatusEnum.Inprogress))
                    .OrderByDescending(res => res.Id)
                    .Skip((pageSize / 2) * pageNumber)
                    .Take(pageSize / 2);

                queryable = newRequests.Union(inProgressRequests).OrderBy(u=>u.Status);
            }
            else
            {
                queryable = _context.Requests
                    .Include(u => u.User)
                    .Include(u => u.Agent)
                    .Include(u => u.City)
                    .Where(res => (res.UserId == userId) && res.Type == RequestTypeEnum.Request
                                  && (res.Status == status || status == 0))
                    .OrderByDescending(res => res.Id)
                    .Skip(pageSize * pageNumber)
                    .Take(pageSize);

                 
            }
            return queryable.Select(u => new RequestListDto()
            {
                Id = u.Id,
                Address = u.Address,
                AgentId = u.AgentId,
                UserId = u.UserId,
                AgentUserName = u.Agent.UserName,
                AgentFullName = u.Agent.FullName,
                UserName = u.User.UserName,
                UserFullName = u.User.FullName,
                CityId = u.CityId,
                CityName = u.City.Name,
                Status = u.Status,
                Type = u.Type,
                ToLocation = u.ToLocation,
                FromLocation = u.FromLocation,
                CreatedDate = u.CreatedDate,
                Price = u.Price,
                IsHidden = u.IsHidden,
            }).ToList();


        }




       

        public List<RequestListDto> GetAgentRequest(string userId, RequestStatusEnum? status, RequestTypeEnum? type, int pageSize, int pageNumber)
        {


           

            var queryable = _context.Requests
                .Include(u => u.User)
                .Include(u => u.Agent)
                .Include(u => u.City)
                .Where(res =>
                    (!res.IsHidden) &&
                    (status != RequestStatusEnum.New || res.UserId != null) &&               
                    (res.Type == type || type == 0));
            if (status != null && status == RequestStatusEnum.Canceled)
            {
                var canceledRequests=GetAllAgentCanceledRequest(userId);
                queryable = queryable.Where(u => canceledRequests.Contains(u.Id));
            }

            else
            {
                queryable = queryable.Where(res => res.AgentId == userId && res.Status == status || status == 0); 
            }

            return queryable
                   .OrderByDescending(res => res.Id).Skip(pageSize * pageNumber)
               .Take(pageSize)
               .Select(u => new RequestListDto()
               {
                   Id = u.Id,
                   Address = u.Address,
                   AgentId = u.AgentId,
                   UserId = u.UserId,
                   AgentUserName = u.Agent.UserName,
                   AgentFullName = u.Agent.FullName,
                   UserName = u.User.UserName,
                   UserFullName = u.User.FullName,
                   CityId = u.CityId,
                   CityName = u.City.Name,
                   Status = u.Status,
                   Type = u.Type,
                   ToLocation = u.ToLocation,
                   FromLocation = u.FromLocation,
                   CreatedDate = u.CreatedDate,
                   Price=u.Price,
                   IsHidden = u.IsHidden,
                   UserPhoneNumber = u.User.PhoneNumber
               })

                   .ToList();

          
        }

        public List<RequestListDto> GetAllAgentRequest(RequestStatusEnum? status, RequestTypeEnum? type, int pageSize, int pageNumber, long cityId,string agentId)
        {
            
            var refusedIds = _context.RefuseRequests.Where(u => u.AgentId == agentId && u.UserId==null)
                .Select(u=>u.RequestId).ToList();

            return _context.Requests
                .Include(u=>u.User)
                .Include(u=>u.Agent)
                .Include(u=>u.City)
                .Where(res =>
                     (!res.IsHidden )&& 
                     (status != RequestStatusEnum.New || res.UserId !=null ) &&
                    (res.Status == status || status == 0) &&
                    (res.CityId == cityId || cityId == 0) &&
                    (res.Type == type || type == 0)
                    && !refusedIds.Contains(res.Id))
                    .OrderByDescending(res => res.Id)
                     .ThenBy(res => res.PassengerNumber)
                .Select(u=>new RequestListDto()
                {
                    Id = u.Id,
                    Address = u.Address,
                    AgentId = u.AgentId,
                    UserId = u.UserId,
                    AgentUserName = u.Agent.UserName,
                    AgentFullName = u.Agent.FullName,
                    UserName = u.User.UserName,
                    UserFullName = u.User.FullName,
                    CityId=u.CityId,
                    CityName=u.City.Name,
                    Status = u.Status,
                    Type = u.Type,
                    ToLocation = u.ToLocation,
                    FromLocation = u.FromLocation,
                    CreatedDate = u.CreatedDate,
                    Price = u.Price,
                    IsHidden = u.IsHidden,
                    UserPhoneNumber=u.User.PhoneNumber
                })                                                   
                    .Skip(pageSize * pageNumber)
                    .Take(pageSize).ToList();
            
        }


        private List<long> GetAllAgentCanceledRequest(string userId)
        {
           return _context.RefuseRequests.Where(u => u.AgentId == userId).Select(u => u.RequestId).ToList();

        }
        public async Task<RequestDetailsDto> GetRequestDetails(long id)
        {

            // _context.Requests.Include(x => x.City).Include(x=>x.User).Join(_context.Ratings, q=> q.AgentId, r => r.AgentId, (q, r) => new { q, r });
            var res= await(_context.Database.SqlQuery<RequestDetailsDto>(
                        "EXEC [dbo].[GetRequestbyId] @Id",
                        new SqlParameter("Id", SqlDbType.BigInt) { Value = id }).FirstOrDefaultAsync());
            return res;






        }

        public Request GetbyId(long id)
        {
            return _context.Requests
                .Include(u => u.User)
                .Include(u => u.Agent)
                .Include(u => u.City).FirstOrDefault(u => u.Id == id);
        }
    }
}
