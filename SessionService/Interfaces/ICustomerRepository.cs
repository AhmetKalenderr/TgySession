using SessionService.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SessionService.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer> GetById(int id);

        Task<List<Customer>> GetAll();

        //Task Add(Customer customer);

        //Task Update(int id, Customer customer);

        //Task Delete(int id);

        Task<Customer> GetByName(string name);
    }
}
