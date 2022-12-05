using BlazorApp1.Data;
using BlazorApp1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace BlazorApp1.Controllers
{
    [Route("api/dev")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly CapstoneDBContext _dbContext;

        public DeviceController(CapstoneDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpPatch("{id:int}/{RNG:int}")]
        //assigns RNG from Database
        public async Task<ActionResult<Device>> AssignRNG(int id, int RNG)
        {
            // Device dev = new Device();
            //find device with proper id
            string random = "";
            random = Convert.ToString(RNG);


            var dev = await _dbContext.Device.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (dev == null) { return BadRequest("hi"); }

            else
            {
                if (random.Length != 6)
                {
                    return BadRequest("here");
                }
                //for (int i = 0; i < 6; i++)
                //{
                //    if (!char.IsDigit(random.ElementAt(i)))
                //    {
                //        return BadRequest("bad");
                //    }
                //}

                dev.RandomNum = random;
                _dbContext.SaveChanges();
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

        [HttpPatch("update")]
        
        public async Task<ActionResult<Device>> Update([FromBody] DataInfo data)
        {
          

            var dev = await _dbContext.Device.Where(x => x.Id == data.JId).FirstOrDefaultAsync();
            if (dev == null)
            {
                return BadRequest("no device found");
            }
            dev.Humidity = data.JHumidity;
            dev.Temp = data.JTemperature;
            dev.WindSpeed = data.JWindSpd;
            dev.SoilMoisture = data.JSM;
            dev.DateTime = DateTime.Now;
            _dbContext.SaveChanges();
            return Ok(dev);
        }

    }

    public class DataInfo
    {
        [JsonProperty("ID")]
        public int JId { get; set; }
        [JsonProperty("Temp")]
        public double JTemperature { get; set; }
        [JsonProperty("Humidity")]
        public double JHumidity { get; set; }
        [JsonProperty("WindSpeed")]
        public double JWindSpd { get; set; }
        
        [JsonProperty("SoilMoisture")]
        public double JSM { get; set; }
    }
}
