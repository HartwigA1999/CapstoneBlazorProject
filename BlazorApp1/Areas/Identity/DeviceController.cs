using BlazorApp1.Data;
using BlazorApp1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Areas.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly CapstoneDBContext? _dbContext;

        [HttpPost("/Assign/{id}/{RNG}")]
        //assigns RNG from Database
        public async Task<ActionResult<Device>> AssignRNG(int id, string RNG)
        {
            Device dev = new Device();
            //find device with proper id
            dev = await _dbContext.Device.Where(x => x.Id == id).FirstOrDefaultAsync();
            if(dev == null) { return BadRequest(); }

            else
            {
                if(RNG.Length != 6)
                {
                    return BadRequest();
                }
                for(int i = 0; i <6; i++)
                {
                    if (!Char.IsDigit(RNG.ElementAt(i)))
                    {
                        return BadRequest();
                    }
                }
                dev.RandomNum = RNG;
                _dbContext.SaveChanges();
                return Ok(dev);
            }
        }

    }
}
