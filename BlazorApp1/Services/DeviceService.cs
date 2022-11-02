using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using BlazorApp1.Models;
using BlazorApp1.Data;

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
            var devices = await _dbContext.Device.Where(x => x.UserName == strCurrentUser).AsNoTracking().ToListAsync();
            return devices;
        }
        //Creates a new device in the database
        public async Task<Device>CreateDeviceAsync(Device obj)
        {
            _dbContext.Device.Add(obj);
            _dbContext.SaveChanges();
            return await Task.FromResult(obj); 
        }
        //updates a device in the database
        public async Task<bool> UpdateDeviceAsync(Device obj)
        {
            var Exsisting = _dbContext.Device.Where(x => x.Id == obj.Id).FirstOrDefault();
            if(Exsisting != null)
            {
                Exsisting.Temp=obj.Temp;
                Exsisting.WindSpeed=obj.WindSpeed;
                Exsisting.Humidity = obj.Humidity;
                Exsisting.DateTime = obj.DateTime;
                Exsisting.Name = obj.Name;
                _dbContext.SaveChanges();
            }
            else
            {
                return await Task.FromResult(false);
            }
            return await Task.FromResult(true);
        }
        //delete function, not implemented in this App
        //public Task<bool> DeleteDeviceAsync()
        //{
        //    var Exsisting = _dbContext.Device.Where(x)
        //}
    }
}
