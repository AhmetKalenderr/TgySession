using SessionService.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SessionService.Interfaces
{
    public interface ILogRepository
    {
        //Entity ve Action type 'ına göre seçilebilir şekilde de getirilebilir. (Customer ve updated olanlar , tüm customerlar, segment ve deleted olanlar vs..)
        Task<List<Log>> GetAll();
        Task<List<Log>> GetByType(string type);
    }
}
