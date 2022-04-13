using System.Collections.Generic;
using System.Threading.Tasks;

namespace SessionService.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        //Task<T> GetByName(string name);
        Task<T> GetById(int id);

        Task<List<T>> GetAll();

        Task Add(T data);

        Task Update(int id, T data);

        Task Delete(int id);
    }
}
