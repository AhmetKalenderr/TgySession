using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SessionService.Core;
using SessionService.DatabaseObject;
using SessionService.Entities;
using SessionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionService.Controllers
{
    public class SegmentController : Controller
    {
        private readonly TgyDatabaseContext _databaseContext;

        LogUnitDto<object> logUnit = new LogUnitDto<object>();

        bool success;
        string message = string.Empty;

        public SegmentController(TgyDatabaseContext tgyDatabaseContext) {
            _databaseContext = tgyDatabaseContext;
            logUnit.TableName = "Segment";
        }


        [HttpPost("/addsegment")]
        public async Task<Result<object>> AddSegment([FromBody]string segName)
        {
            logUnit.ActionTime = DateTime.Parse(DateTime.Now.ToLocalTime().ToString("dd'/'MM'/'yyyy HH:mm:ss"));
            logUnit.ActionType = ActionType.ADD;

            try
            {
                Segment segment = new Segment() { Code=segName };                

                await _databaseContext.Segments.AddAsync(segment);
                await _databaseContext.SaveChangesAsync();

                logUnit.Object = segment;

                message = "Segment Eklendi";
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                message = e.Message;
                success = false;

                logUnit.Object = null;
            }
            finally
            {
                logUnit.Message = message;
                logUnit.Success = success;
                await _databaseContext.Logs.AddAsync(new Log() { Data = JsonConvert.SerializeObject(logUnit) });
                await _databaseContext.SaveChangesAsync();
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
            logUnit.ActionTime = DateTime.Parse(DateTime.Now.ToLocalTime().ToString("dd'/'MM'/'yyyy HH:mm:ss"));
            logUnit.ActionType = ActionType.GET;
            try
            {
                segments =  await _databaseContext.Segments.Select(s => new Segment { Code = s.Code, Id = s.Id }).ToListAsync();
                success = true;
                message = "Başarılı";

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                message = e.Message.ToString();
                success = false;
            }
            finally
            {
                logUnit.Object = null;
                logUnit.Message = message;
                logUnit.Success = success;
                await _databaseContext.Logs.AddAsync(new Log() { Data = JsonConvert.SerializeObject(logUnit) });
                await _databaseContext.SaveChangesAsync();
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
            logUnit.ActionTime = DateTime.Parse(DateTime.Now.ToLocalTime().ToString("dd'/'MM'/'yyyy HH:mm:ss"));
            logUnit.ActionType = ActionType.UPDATE;

            try
            {
                Segment updatedSegment = await _databaseContext.Segments.SingleAsync(s => s.Id == segment.Id);

                updatedSegment.Code = segment.Code;

                await _databaseContext.SaveChangesAsync();

                logUnit.Object = updatedSegment;
                success = true;
                message = "Başarılı";

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                success = false;
                message = e.Message.ToString();
                logUnit.Object= null;
            }
            finally
            {
                logUnit.Message = message;
                logUnit.Success = success;
                await _databaseContext.Logs.AddAsync(new Log() { Data = JsonConvert.SerializeObject(logUnit) });
                await _databaseContext.SaveChangesAsync();
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
            logUnit.ActionTime = DateTime.Parse(DateTime.Now.ToLocalTime().ToString("dd'/'MM'/'yyyy HH:mm:ss"));
            logUnit.ActionType = ActionType.DELETE;

            try
            {
                Segment deletedSegment = await _databaseContext.Segments.SingleAsync(s => s.Id ==id);

                _databaseContext.Segments.Remove(deletedSegment);
                await _databaseContext.SaveChangesAsync();

                logUnit.Object = deletedSegment;
                success = true;
                message = "Başarılı";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                message = e.Message.ToString();
                success=false;

                logUnit.Object = null;
            }
            finally
            {
                logUnit.Message = message;
                logUnit.Success = success;
                await _databaseContext.Logs.AddAsync(new Log() { Data = JsonConvert.SerializeObject(logUnit) });
                await _databaseContext.SaveChangesAsync();
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
            logUnit.ActionTime = DateTime.Parse(DateTime.Now.ToLocalTime().ToString("dd'/'MM'/'yyyy HH:mm:ss"));
            logUnit.ActionType =ActionType.GET;

            Segment segment = new Segment();
            try
            {
                 segment = await _databaseContext.Segments.SingleAsync(s=> s.Id == id);
                 message = "Başarılı";
                 success = true;
                logUnit.Object = segment;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                message = e.Message.ToString();
                success = false;
                logUnit.Object = null;
            }
            finally
            {
                logUnit.Message = message;
                logUnit.Success = success;
                await _databaseContext.Logs.AddAsync(new Log() { Data = JsonConvert.SerializeObject(logUnit) });
                await _databaseContext.SaveChangesAsync();
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
            logUnit.ActionTime = DateTime.Parse(DateTime.Now.ToLocalTime().ToString("dd'/'MM'/'yyyy HH:mm:ss"));
            logUnit.ActionType =ActionType.GET;

            List<Segment> segment = new List<Segment>();
            try
            {
                segment = await _databaseContext.Segments.Where(c => c.Code.Contains(code)).ToListAsync();
                message = "Başarılı";
                success=true;
                logUnit.Object= segment;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                message=e.Message.ToString();
                success = false;
                logUnit.Object = null;
            }
            finally
            {
                logUnit.Message = message;
                logUnit.Success = success;
                await _databaseContext.Logs.AddAsync(new Log() { Data = JsonConvert.SerializeObject(logUnit) });
                await _databaseContext.SaveChangesAsync();
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
