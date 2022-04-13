using SessionService.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SessionService.Interfaces
{
    public interface ILogRepository
    {
        Task<List<Log>> GetAll();
        Task<List<Log>> GetByType(string type);
    }
}
