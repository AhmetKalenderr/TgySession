using Microsoft.AspNetCore.Mvc;
using SessionService.Entities;
using SessionService.Interfaces;
using SessionService.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SessionService.Controllers
{
    public class LogController : Controller
    {

        private readonly ILogRepository logRepository;

        public LogController(ILogRepository repo)
        {
            logRepository = repo;
        }

        [HttpPost("/getall")]
        public async Task<List<Log>> GetAll()
        {
            return await logRepository.GetAll();
        }

        [HttpPost("/gettype")]
        public async Task<List<Log>> GetType(string type)
        {
            return await logRepository.GetByType(type);
        }
    }
}
