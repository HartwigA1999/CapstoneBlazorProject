using BlazorApp1.Data;
using BlazorApp1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace BlazorApp1.Controllers
{
    public class DataInfo
    {
       [JsonPropertyName("ID")]
        public int JId { get; set; }
       [JsonPropertyName("Temp")]
        public double JTemperature { get; set; }
       [JsonPropertyName("Humidity")]
        public double JHumidity { get; set; }
        [JsonPropertyName("WindSpeed")]
        public double JWindSpd { get; set; }

        [JsonPropertyName("SoilMoisture")]
        public double JSM { get; set; }

        [JsonPropertyName("Delta")]
        public double JDelta { get; set; }
    }
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
        public async void StoreOldData(Device dev)

        {
            
            if (dev.DateTime == null)
            {
                dev.DateTime = DateTime.Now;
            }
            //make a list of exsisting old data
            List<OldData> dataList = new List<OldData>();
            //fill list from db comparing device IDs, this pulls only previous versions of the current device
            
           dataList = await _dbContext.OldData.Where(x => x.DeviceId == dev.Id).AsNoTracking().ToListAsync();
            //if list is less than 10, add to database
            if (dataList != null)
            {
                if (dataList.Count() < 10)
                {
                    //add a new device
                    OldData newDevice = new OldData();
                    newDevice.Humidity = dev.Humidity;
                    newDevice.DeviceId = dev.Id;
                    newDevice.Temp = dev.Temp;
                    newDevice.WindSpeed = dev.WindSpeed;
                    newDevice.SoilMoisture = dev.SoilMoisture;
                    newDevice.DateTime = (DateTime)dev.DateTime;
                    _dbContext.Add(newDevice);
                }
                else
                {
                    //replace oldest device
                    //search for oldest device
                    //using the .Compare() function, I will determine seniority

                    //< 0 − If date1 is earlier than date2.
                    //0 − If date1 is the same as date2.
                    //> 0 − If date1 is later than date2.
                    OldData OldestData = dataList.ElementAt(0);
                    for (int i = 1; i < 10; i++)
                    {
                        //if the next element in the list is older than the current element
                        if(DateTime.Compare((DateTime)OldestData.DateTime, (DateTime)dataList.ElementAt(i).DateTime) > 0)
                        {
                            OldestData = dataList.ElementAt(i);
                        }
                    }
                    //replace the oldest data with the most current data

                    //finds the DB id of the element that was the oldest data from a DB context
                    var newData = await _dbContext.OldData.Where(x => x.Id == OldestData.Id).FirstOrDefaultAsync();
                    if(newData == null)
                    {
                        //do nothing
                    }
                    else
                    {
                        //loads new data into oldest spot
                        newData.DeviceId = dev.Id;
                        newData.Humidity = dev.Humidity;
                        newData.Temp = dev.Temp;
                        newData.WindSpeed = dev.WindSpeed;
                        newData.SoilMoisture = dev.SoilMoisture;
                        newData.Gdelta = dev.Gdelta;
                        newData.DateTime = (DateTime)dev.DateTime;
                        _dbContext.SaveChanges();
                    }
                }
            }
            //if list is empty, make a new device
            else
            {
                OldData newDevice = new OldData();
                newDevice.Humidity = dev.Humidity;
                newDevice.DeviceId = dev.Id;
                newDevice.Temp = dev.Temp;
                newDevice.WindSpeed = dev.WindSpeed;
                newDevice.SoilMoisture = dev.SoilMoisture;
                newDevice.Gdelta = dev.Gdelta;
                newDevice.DateTime = (DateTime)dev.DateTime;
                _dbContext.Add(newDevice);
            }

            
        }

        [HttpGet("test/{id:int}")]
        public async Task<ActionResult<Device>> Test(int id)
        {
            var dev = await _dbContext.Device.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (dev == null)
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
                    return BadRequest("no device found, Device ID was " + data.JId + " " + data.JWindSpd + data.JTemperature);
                }
                dev.Humidity = data.JHumidity;
                dev.Temp = data.JTemperature;
                dev.WindSpeed = data.JWindSpd;
                dev.SoilMoisture = data.JSM;
                dev.Gdelta = data.JDelta;
                dev.DateTime = DateTime.Now;


                List<OldData> dataList = new List<OldData>();
                //fill list from db comparing device IDs, this pulls only previous versions of the current device

                dataList = await _dbContext.OldData.Where(x => x.DeviceId == dev.Id).AsNoTracking().ToListAsync();
                //if list is less than 10, add to database
                if (dataList != null)
                {
                    if (dataList.Count() < 10)
                    {
                        //add a new device
                        OldData newDevice = new OldData();
                        newDevice.Humidity = dev.Humidity;
                        newDevice.DeviceId = dev.Id;
                        newDevice.Temp = dev.Temp;
                        newDevice.WindSpeed = dev.WindSpeed;
                        newDevice.SoilMoisture = dev.SoilMoisture;
                        newDevice.Gdelta = dev.Gdelta;
                        newDevice.DateTime = (DateTime)dev.DateTime;
                        newDevice.Count = 1;
                    
                        _dbContext.Add(newDevice);
                        _dbContext.SaveChanges();
                        return Ok(dev);
                }
                    else
                    {
                        //replace oldest device
                        //search for oldest device
                        //using the .Compare() function, I will determine seniority

                        //< 0 − If date1 is earlier than date2.
                        //0 − If date1 is the same as date2.
                        //> 0 − If date1 is later than date2.
                        OldData OldestData = dataList.ElementAt(0);
                        for (int i = 1; i < dataList.Count-1; i++)
                        {
                            //if the next element in the list is older than the current element
                            if (DateTime.Compare((DateTime)OldestData.DateTime, (DateTime)dataList.ElementAt(i).DateTime) > 0)
                            {
                                OldestData = dataList.ElementAt(i);
                            }
                        }
                        //replace the oldest data with the most current data

                        //finds the DB id of the element that was the oldest data from a DB context
                        var newData = await _dbContext.OldData.Where(x => x.Id == OldestData.Id).FirstOrDefaultAsync();
                        if (newData == null)
                        {
                            //do nothing
                        }
                        else
                        {
                            //loads new data into oldest spot
                            newData.DeviceId = dev.Id;
                            newData.Humidity = dev.Humidity;
                            newData.Temp = dev.Temp;
                            newData.WindSpeed = dev.WindSpeed;
                            newData.SoilMoisture = dev.SoilMoisture;
                            newData.DateTime = (DateTime)dev.DateTime;
                            newData.Gdelta = dev.Gdelta;
                          //  newData.Count = 1;
                            _dbContext.SaveChanges();
                        
                        }
                    }
                //sort data by age
                for (int i = 0; i < dataList.Count() - 1; i++)
                {
                    OldData OldestData = dataList.ElementAt(i);
                    for (int j = i; j < dataList.Count - 1; j++)
                    {
                        //if the next element in the list is older than the current element
                        if (DateTime.Compare((DateTime)OldestData.DateTime, (DateTime)dataList.ElementAt(i).DateTime) > 0)
                        {
                            OldestData = dataList.ElementAt(i);
                        }
                    }
                    //set oldest data count to new count
                    OldestData.Count = i + 1;
                    _dbContext.SaveChanges();
                }


                _dbContext.SaveChanges();
                    return Ok(dev);
                }
            else
            {
                OldData newDevice = new OldData();
                newDevice.Humidity = dev.Humidity;
                newDevice.DeviceId = dev.Id;
                newDevice.Temp = dev.Temp;
                newDevice.WindSpeed = dev.WindSpeed;
                newDevice.SoilMoisture = dev.SoilMoisture;
                newDevice.DateTime = (DateTime)dev.DateTime;
                newDevice.Gdelta = dev.Gdelta;
                _dbContext.Add(newDevice);
                _dbContext.SaveChanges();
                return Ok(dev);
            }
            

        
            
        }

        

    }
}

