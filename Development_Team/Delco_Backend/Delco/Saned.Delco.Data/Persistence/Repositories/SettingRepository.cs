using System.Linq;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Core.Repositories;
using Saned.Delco.Data.Persistence;
using Saned.Delco.Data.Persistence.Repositories;

namespace Saned.Delco.Data.Persistence.Repositories
{
    public class SettingRepository : BaseRepository<Setting>, ISettingRepository
    {
        public SettingRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public string GetTermsOfConditions()
        {
            return All().First().TermsOfConditions;
        }
        public Setting GetSetting()
        {
            return All().First();
        }
    }
}
