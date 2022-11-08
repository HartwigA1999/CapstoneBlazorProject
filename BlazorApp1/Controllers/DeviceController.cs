using BlazorApp1.Data;
using BlazorApp1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BlazorApp1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly CapstoneDBContext _dbContext;

        public DeviceController(CapstoneDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpPatch("assign/{id:int}/{RNG:int}")]
        //assigns RNG from Database
        public async Task<ActionResult<Device>> AssignRNG(int id, int RNG)
        {
            // Device dev = new Device();
            //find device with proper id
            //string random = "";
            //random = Convert.ToString(RNG);

          
            var dev = await _dbContext.Device.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (dev == null) { return BadRequest("hi"); }

            else
            {
                //if (random.Length != 6)
                //{
                //    return BadRequest("here");
                //}
                //for (int i = 0; i < 6; i++)
                //{
                //    if (!char.IsDigit(random.ElementAt(i)))
                //    {
                //        return BadRequest("bad");
                //    }
                //}

                dev.Temp = RNG;
                
                return Ok(dev);
            }
        }

        [HttpGet("test/{id:int}")]
        public async Task<ActionResult<Device>> Test(int id)
        {
            var dev = await _dbContext.Device.Where(x => x.Id == id).FirstOrDefaultAsync();
            if(dev == null)
            {
                return BadRequest();
            }
            return Ok(dev);

        }

    }
}
