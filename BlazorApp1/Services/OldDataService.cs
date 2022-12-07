using BlazorApp1.Data;
using BlazorApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Services
{
    public class OldDataService
    {

        private readonly CapstoneDBContext _OldDatadbContext;
        public OldDataService(CapstoneDBContext dbContext)
        {
            _OldDatadbContext = dbContext;
        }
        public async Task<OldData> CreateDeviceAsync(OldData obj)
        {
            _OldDatadbContext.OldData.Add(obj);
            _OldDatadbContext.SaveChanges();
            return await Task.FromResult(obj);
        }
        //public async Task<OldData> GetData(int deviceID)
        //{
        //    var data = 
        //}

    }
}
