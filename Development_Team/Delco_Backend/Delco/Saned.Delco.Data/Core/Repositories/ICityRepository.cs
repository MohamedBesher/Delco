using Saned.Delco.Data.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Saned.Delco.Data.Core.Repositories
{
    public interface ICityRepository : IBaseRepository<City>
    {
        Task<List<City>> SelectNearByCities(string lat, string lang);

        string GetDifference(string key, string fromLatitude, string fromLongitude, string toLatitude, string toLongitude);

    }
}
