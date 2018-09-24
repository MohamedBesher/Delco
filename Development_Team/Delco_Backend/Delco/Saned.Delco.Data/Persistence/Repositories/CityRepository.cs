using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Core.Repositories;
using Saned.Delco.Data.Persistence;
using Saned.Delco.Data.Persistence.Repositories;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Saned.Delco.Data.Persistence.Repositories
{
    public class CityRepository : BaseRepository<City>, ICityRepository
    {
        public CityRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<List<City>> SelectNearByCities(string lat, string lang)
        {

            var query = @"DECLARE @point GEOGRAPHY
                        SET @point = geography::Point(@Lat, @Lang, 4326);

                        SELECT * FROM Cities where 
                        @point.STDistance(geography::Point(Latitude, Longitude, 4326))<= Cities.NumberOfKilometers * 1000
                        ORDER BY @point.STDistance(geography::Point(Latitude, Longitude, 4326)) ASC";

            var res =
             (await _context.Database.SqlQuery<City>(
                    query,
                     new SqlParameter("Lat", SqlDbType.NVarChar) { Value = lat },
                     new SqlParameter("Lang", SqlDbType.NVarChar) { Value = lang }
                   ).ToListAsync());

            return res;

        }

        public string GetDifference(string key, string fromLatitude, string fromLongitude, string toLatitude, string toLongitude)
        {
            string apiRoute = "https://maps.googleapis.com/maps/api/distancematrix/json?origins={0},{1}&destinations={2},{3}&mode={4}&language={5}&key={6}";

            apiRoute = string.Format(apiRoute, fromLatitude, fromLongitude, toLatitude, toLongitude, "driving", "ar-sa", key);


            var request = WebRequest.Create(apiRoute);
            request.ContentType = "application/json; charset=utf-8";
            string text;
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }

            return text;
        
        }
    }
}
