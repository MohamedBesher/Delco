using System.Linq;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Core.Repositories;

namespace Saned.Delco.Data.Persistence.Repositories
{
    public class EmailSettingRepository : IEmailSettingRepository
    {
        private readonly IApplicationDbContext _context;

        public EmailSettingRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public EmailSetting GetEmailSetting(string type)
        {
            return _context.EmailSettings.SingleOrDefault(u => u.EmailSettingType == type.Trim());
        }


       
    }


}