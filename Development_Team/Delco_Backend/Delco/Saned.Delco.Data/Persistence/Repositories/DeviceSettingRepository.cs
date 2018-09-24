using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Persistence;
using Saned.Delco.Data.Persistence.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;
using Saned.Delco.Data.Core.Repositories;

namespace Saned.Delco.Data.Persistence.Repositories
{
    public class DeviceSettingRepository : BaseRepository<DeviceSetting>, IDeviceSettingRepository
    {
        public DeviceSettingRepository(ApplicationDbContext context) : base(context)
        {

        }

        public Task<List<DeviceSetting>> GetDevicesBasedOnUserId(string applicationUserId)
        {
          
                return (from device in _context.DeviceSettings
                        where device.ApplicationUserId == applicationUserId
                        select device).ToListAsync();

           
        }

        public async Task AdddeviceSetting(DeviceSetting deviceSetting)
        {
            var userDevices = await GetDevicesBasedOnUserId(deviceSetting.ApplicationUserId);
            if (userDevices.All(x => x.DeviceId != deviceSetting.DeviceId))
            {
                Create(deviceSetting);
            }
        }
    }
}
