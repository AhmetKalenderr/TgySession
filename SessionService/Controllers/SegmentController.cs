using Microsoft.AspNetCore.Mvc;
using SessionService.Core;
using SessionService.Entities;
using SessionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SessionService.Controllers
{
    public class SegmentController : Controller
    {
        private readonly TgyDatabaseContext _databaseContext;

        public SegmentController(TgyDatabaseContext tgyDatabaseContext) { _databaseContext = tgyDatabaseContext; }

        bool success;
        string message = string.Empty;

        [HttpPost("/addsegment")]
        public Result<object> AddSegment([FromBody]string segName)
        {
            try
            {
                Segment segment = new Segment() { Code=segName };                

                _databaseContext.Segments.Add(segment);
                _databaseContext.SaveChanges();
                message = "Başarılı";
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

        public Result<object> GetAllSegments()
        {

            List<Segment> segments = new List<Segment>();

            try
            {
                segments = _databaseContext.Segments.Select(s => new Segment { Code = s.Code, Id = s.Id }).ToList();
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

        public Result<object> UpdateSegment([FromBody]Segment segment)
        {
            try
            {
                Segment updatedSegment = _databaseContext.Segments.Single(s => s.Id == segment.Id);

                updatedSegment.Code = segment.Code;

                _databaseContext.SaveChanges();
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

        public Result<object> DeleteSegment([FromBody]int id)
        {
            try
            {
                Segment deletedSegment = _databaseContext.Segments.Single(s => s.Id ==id);

                _databaseContext.Segments.Remove(deletedSegment);
                _databaseContext.SaveChanges();
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

        public Result<object> GetByIdSegment([FromBody]int id)
        {
            Segment segment = new Segment();
            try
            {
                 segment = _databaseContext.Segments.Single(s=> s.Id == id);
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

        public Result<object> GetByCodeSegment([FromBody]string code)
        {
            List<Segment> segment = new List<Segment>();
            try
            {
                segment = _databaseContext.Segments.Where(c => c.Code.Contains(code)).ToList();
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
