using Saned.Delco.Data.Persistence.Infrastructure;

namespace Saned.Delco.Data.Core.Models
{
    public class ContactUs
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}
