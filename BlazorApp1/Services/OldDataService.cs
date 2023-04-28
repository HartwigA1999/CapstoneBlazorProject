using BlazorApp1.Data;
using BlazorApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Services
{
    public class OldDataService
    {

        private readonly CapstoneDBContext _dbContext;
        public OldDataService(CapstoneDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<OldData> CreateDeviceAsync(OldData obj)
        {
            _dbContext.OldData.Add(obj);
            _dbContext.SaveChanges();
            return await Task.FromResult(obj);
        }
        public Task<List<OldData>> GetDataAsync(int devID)
        {
            var OldDevices = _dbContext.OldData.Where(x => x.DeviceId == devID).AsNoTracking().ToListAsync();
            return OldDevices;
        }
        public Task<OldData[]> GetDataArray(int devID)
         {
            object toArray = _dbContext.OldData.Where(x => x.DeviceId == devID).ToArray();
            object OldDevices = toArray;
            return (Task<OldData[]>)OldDevices;
        }
        public async Task<List<Device>> GetDevicesAsync(string user)
        {
            var devices = await _dbContext.Device.Where(x => x.UserName == user).AsNoTracking().ToListAsync();
            return devices;
        }
        public async void SetCount(int deviceID)
        {
            List<OldData> sortList = await GetDataAsync(deviceID);

            //find the oldest date in data
            OldData d = sortList.ElementAt(0);
            int oldestLoc = 0;
            for(int i = 0; i < sortList.Count-1; i++)
            {
                if (DateTime.Compare((DateTime)d.DateTime, (DateTime)sortList.ElementAt(i).DateTime) > 0)
                {
                    d = sortList.ElementAt(i);
                    oldestLoc = i;
                }
                
            }
            //identify oldest data
            d.Count = 0;
            //since the list is cyclical, every value after the oldest should be the next eldest, wrapping around in a loop.
            int j = 1;
            for(int i = oldestLoc+1; i < sortList.Count-1; i++) {
                sortList.ElementAt(i).Count = j;
                j++;
            }
            //fill count before the oldest element
            if (oldestLoc != 0)
            {
                for (int i = 0; i < oldestLoc; i++)
                {
                    sortList.ElementAt(i).Count = j;
                    j++;
                }
            }
            //now the list count should be established and the list can be printed using count
            _dbContext.SaveChanges();
            


        }
        //public async Task<OldData> GetData(int deviceID)
        //{
        //    var data = 
        //}

    }
}
