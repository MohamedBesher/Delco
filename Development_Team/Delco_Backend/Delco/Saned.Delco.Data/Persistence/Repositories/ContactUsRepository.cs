using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Core.Repositories;
using Saned.Delco.Data.Persistence;
using Saned.Delco.Data.Persistence.Repositories;

namespace Saned.Delco.Data.Persistence.Repositories
{
    public class ContactUsRepository : BaseRepository<ContactUs>, IContactUsRepository
    {
        public ContactUsRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
