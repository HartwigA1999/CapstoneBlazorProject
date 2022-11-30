﻿using BlazorApp1.Data;
using BlazorApp1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

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

        [HttpPatch("update/{id:int}")]
        
        public async Task<ActionResult<Device>> Update([FromBody] string input, int id)
        {

            var dev = await _dbContext.Device.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (dev == null)
            {
                return BadRequest("no device found");
            }
            string[] words = input.Split("+");


            //load temp
            if (words.Length == 4)
            {
                dev.Temp = Convert.ToDouble(words.ElementAt(0));
                dev.Humidity = Convert.ToDouble(words.ElementAt(1));
                dev.WindSpeed = Convert.ToDouble(words.ElementAt(2));
                dev.SoilMoisture = Convert.ToDouble(words.ElementAt(3));
                dev.DateTime = DateTime.Now;

                _dbContext.SaveChanges();

                return Ok(dev);
            }
            else
            {
                return BadRequest("improper data format");
            }
        }

    }
}
