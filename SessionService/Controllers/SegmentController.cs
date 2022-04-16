using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SessionService.Core;
using SessionService.DatabaseObject;
using SessionService.Entities;
using SessionService.Interfaces;
using SessionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionService.Controllers
{
    public class SegmentController : Controller
    {


        bool success;
        string message = string.Empty;
        string getAllKey = "GetAllSegment";
        private readonly ISegmentRepository _segmentRepository;
        private readonly IDistributedCache _distributed;
        private readonly IMapper mapper;

        public SegmentController(ISegmentRepository segmentRepository,IMapper _mapper, IDistributedCache distributed)
        {
            _segmentRepository = segmentRepository;
            mapper = _mapper;
            _distributed = distributed;
        }



        [HttpPost("/addsegment")]
        public async Task<Result<object>> AddSegment([FromBody]string segName)
        {

            try
            {
          
                Segment segment = new Segment() { Code=segName };
                await _segmentRepository.Add(segment);

                message = "Segment Eklendi";
                success = true;


                //Redisten eski segment listesini siliyoruz.
                if (await _distributed.GetAsync(getAllKey) != null)
                {
                    await _distributed.RemoveAsync(getAllKey);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                message = e.Message;
                success = false;

            }
            return new Result<object>
            {
                data = null,
                Success = success,
                Message = message
            };
        }

        [HttpPost("/getallsegments")]

        public async Task<Result<object>> GetAllSegments()
        {

            List<Segment> segments = new List<Segment>();

            try
            {
                var getAllSegmentCache = await _distributed.GetAsync(getAllKey);
                string cacheString;
                

                //Rediste var ise segment listesini redisten getir yok ise eğer repodan çekip redise de kaydını at.
                if (getAllSegmentCache != null)
                {
                    cacheString = Encoding.UTF8.GetString(getAllSegmentCache);
                    segments = JsonConvert.DeserializeObject<List<Segment>>(cacheString);
                    Console.WriteLine("Redisten geldi");
                }
                else
                {
                    segments = await _segmentRepository.GetAll();
                    cacheString = JsonConvert.SerializeObject(segments);
                    getAllSegmentCache = Encoding.UTF8.GetBytes(cacheString);

                    var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1)).SetAbsoluteExpiration(DateTime.Now.AddMonths(1));

                    await _distributed.SetAsync(getAllKey,getAllSegmentCache,options);

                    Console.WriteLine("Repodan Geldi");

                }

                success = true;
                message = "Başarılı";

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                message = e.Message.ToString();
                success = false;
            }


            return new Result<object>
            {
                data=segments,
                Success = success,
                Message=message
            };
        }

        [HttpPost("/updatesegment")]

        public async Task<Result<object>> UpdateSegment([FromBody] UpdateSegmentDto segment)
        {
            var getByIdSegmentCache = await _distributed.GetAsync("Segment get by id: " + segment.Id);

         

            try
            {
                

                await _segmentRepository.Update(segment.Id,mapper.Map(segment,new Segment()));
                success = true;
                message = "Başarılı";

                //Redisteki segment verisini de güncelliyoruz.
                if (getByIdSegmentCache != null)
                {
                    string _cacheKey = "Segment get by id: " + segment.Id;
                    Console.WriteLine("Rediste Güncellendi");
                    var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1)).SetAbsoluteExpiration(DateTime.Now.AddMonths(1));
                    await _distributed.SetAsync(_cacheKey, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mapper.Map(segment, new Segment()))), options);


                }

                //Redisten eski segment listesini siliyoruz.
                if (await _distributed.GetAsync(getAllKey) != null)
                {
                    await _distributed.RemoveAsync(getAllKey);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                success = false;
                message = e.Message.ToString();
            }


            return new Result<object>
            {
                data = null,
                Success = success,
                Message = message
            };
        }

        [HttpPost("/deletesegment")]

        public async Task<Result<object>> DeleteSegment([FromBody]int id)
        {
            var getByIdSegmentCache = await _distributed.GetAsync("Segment get by id: " + id.ToString());    
            try
            {

                await _segmentRepository.Delete(id);

                //Redisteki segment verisinide siliyoruz.
                if (getByIdSegmentCache != null)
                {
                    string _cacheKey = "Segment get by id: " + id.ToString();
                    Console.WriteLine("Redisten Silindi");
                    await _distributed.RemoveAsync(_cacheKey);
                }
                
                
                //Redisten eski segment listesini siliyoruz.
                if (await _distributed.GetAsync(getAllKey) != null)
                {
                    await _distributed.RemoveAsync(getAllKey);
                }

                success = true;
                message = "Başarılı";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                message = e.Message.ToString();
                success=false;

            }

            return new Result<object> 
            {
              data=null,
              Success = success,
              Message=message
            };
        }

        [HttpPost("/getbyidsegment")]

        public async Task<Result<object>> GetByIdSegment([FromBody]int id)
        {
            Segment segment;

            var getByIdSegmentCache = await _distributed.GetAsync("Segment get by id: " + id.ToString());
            string cacheJsonItem;

            //Rediste varsa ilgili id'nin keyi ile tanımlı olan segment datasını çekiyoruz yok ise repodan çekip redise de kaydediyoruz.
            if (getByIdSegmentCache != null)
            {
                Console.WriteLine("Redisten geldi");
                cacheJsonItem = Encoding.UTF8.GetString(getByIdSegmentCache);
                segment = JsonConvert.DeserializeObject<Segment>(cacheJsonItem);
            }
            else
            {
                segment = await _segmentRepository.GetById(id);

                cacheJsonItem = JsonConvert.SerializeObject(segment);

                getByIdSegmentCache = Encoding.UTF8.GetBytes(cacheJsonItem);

                var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1)).SetAbsoluteExpiration(DateTime.Now.AddMonths(1));

                await _distributed.SetAsync("Segment get by id: " + id.ToString(), getByIdSegmentCache, options);

                Console.WriteLine("Repodan Geldi");
            }

            try
            {
                 message = "Başarılı";
                 success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                message = e.Message.ToString();
                success = false;
            }


            return new Result<object>
            {
                data = segment,
                Success = success,
                Message = message
            };
        }

        [HttpPost("/getbycodesegment")]

        public async Task<Result<object>> GetByCodeSegment([FromBody]string code)
        {


            Segment segment;
            var getByIdSegmentCache = await _distributed.GetAsync("Segment get by id: " + code);
            string cacheJsonItem;

            //Rediste ilgili code ile tanımlı olan segment datasını çekiyoruz. Yok ise repodan çekip redise kaydını atıyoruz.
            if (getByIdSegmentCache != null)
            {
                Console.WriteLine("Redisten geldi");
                cacheJsonItem = Encoding.UTF8.GetString(getByIdSegmentCache);
                segment = JsonConvert.DeserializeObject<Segment>(cacheJsonItem);
            }
            else
            {
                segment = await _segmentRepository.GetByName(code);

                cacheJsonItem = JsonConvert.SerializeObject(segment);

                getByIdSegmentCache = Encoding.UTF8.GetBytes(cacheJsonItem);

                var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1)).SetAbsoluteExpiration(DateTime.Now.AddMonths(1));

                await _distributed.SetAsync("Segment get by id: " + code, getByIdSegmentCache, options);

                Console.WriteLine("Repodan Geldi");
            }

            try
            {
                
                message = "Başarılı";
                success=true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                message=e.Message.ToString();
                success = false;
            }

            return new Result<object>
            {
                data = segment,
                Message = message,
                Success = success
            };
        }
    }

}
