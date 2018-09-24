using Saned.Delco.Data.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saned.Delco.Data.Core.Repositories
{
    public interface IDeviceSettingRepository : IBaseRepository<DeviceSetting>
    {
        Task<List<DeviceSetting>> GetDevicesBasedOnUserId(string applicationUserId);

        Task AdddeviceSetting(DeviceSetting deviceSetting);
    }
}
