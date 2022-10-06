using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Services
{
    public class DeviceService
    {
        private readonly CapstoneDBContext _dbContext;
        public DeviceService(CapstoneDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        //retrieves list of devices under the current user account
        public async Task<List<Device>>
                GetDevicesAsync(string strCurrentUser)
        {
            return await _dbContext.Device
                     // Only get entries for the current logged in user
                     .Where(x => x.UserName == strCurrentUser)
                     // Use AsNoTracking to disable EF change tracking
                     // Use ToListAsync to avoid blocking a thread
                     .AsNoTracking().ToListAsync();

        }
        //Creates a new device in the database
        public Task<Device>CreateDeviceAsync(Device obj)
        {
            _dbContext.Device.Add(obj);
            _dbContext.SaveChanges();
            return Task.FromResult(obj); 
        }
        //updates a device in the database
        public object UpdateDeviceAsync(Device obj)
        {
            var Exsisting = _dbContext.Device.Where(x => x.Id == obj.Id).FirstOrDefault();
            if(Exsisting != null)
            {
                Exsisting.Temp=obj.Temp;
                Exsisting.WindSpd=obj.WindSpd;
                Exsisting.Humidity = obj.Humidity;
                Exsisting.DateTime = obj.DateTime;
                Exsisting.Name = obj.Name;
                _dbContext.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }
        //delete function, not implemented in this App
        //public Task<bool> DeleteDeviceAsync()
        //{
        //    var Exsisting = _dbContext.Device.Where(x)
        //}
    }
}
