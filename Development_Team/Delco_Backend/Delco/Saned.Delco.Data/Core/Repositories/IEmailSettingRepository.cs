using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Core.Repositories
{
    public interface IEmailSettingRepository
    {
        EmailSetting GetEmailSetting(string type);
    }
}