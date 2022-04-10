using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SessionService.Core;
using SessionService.DatabaseObject;
using SessionService.Entities;
using SessionService.Interfaces;
using SessionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionService.Controllers
{
    public class SegmentController : Controller
    {


        bool success;
        string message = string.Empty;
        private readonly ISegmentRepository _segmentRepository;
        private readonly IMapper mapper;

        public SegmentController(ISegmentRepository segmentRepository,IMapper _mapper)
        {
            _segmentRepository = segmentRepository;
            mapper = _mapper;
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
                segments = await _segmentRepository.GetAll();
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


            try
            {
                await _segmentRepository.Update(segment.Id,mapper.Map(segment,new Segment()));
                success = true;
                message = "Başarılı";

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


            try
            {
                await _segmentRepository.Delete(id);

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


            Segment segment = new Segment();
            try
            {
                 segment = await _segmentRepository.GetById(id);
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


            Segment segment = new Segment();
            try
            {
                segment = await _segmentRepository.GetByName(code);
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
