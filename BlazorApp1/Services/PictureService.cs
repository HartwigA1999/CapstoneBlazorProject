using BlazorApp1.Data;
using BlazorApp1.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace BlazorApp1.Services
{
    public class PictureService
    {
        private readonly CapstoneDBContext _dbContext;
        public PictureService(CapstoneDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        //get devices

        public async Task<List<Device>> GetDevicesAsync(string user)
        {
            var devices = await _dbContext.Device.Where(x => x.UserName == user).AsNoTracking().ToListAsync();
            return devices;
        }

        //in front end, get the device ID and send it over here
        //return the PictureDb object with the base64 in it

        public async Task<string>GetPictureDbAsync(int DeviceID)

        {

            PictureDb ThisPicCb = (PictureDb)_dbContext.PictureDb.Where(x=>x.DeviceId == DeviceID);
            return ThisPicCb.PictureData;
          
        }
 
    }
}
