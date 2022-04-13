using Microsoft.EntityFrameworkCore;
using SessionService.Entities;
using SessionService.Interfaces;
using SessionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionService.Repository
{
    public class LogRepository : ILogRepository
    {
        TgyDatabaseContext databaseContext;
        public LogRepository(TgyDatabaseContext context)
        {
            databaseContext = context;
        }

        public async Task<List<Log>> GetAll()
        {
            return await databaseContext.Logs.ToListAsync();
        }

        public async Task<List<Log>> GetByType(string type)
        {
            return await databaseContext.Logs.Where(l =>l.ActionType.ToLower()== type.ToLower()).ToListAsync();
        }
    }
}
