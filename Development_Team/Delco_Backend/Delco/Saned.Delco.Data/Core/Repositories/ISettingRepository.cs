using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Core.Repositories
{
    public interface ISettingRepository : IBaseRepository<Setting>
    {
        string GetTermsOfConditions();
        Setting GetSetting();
    }
}
