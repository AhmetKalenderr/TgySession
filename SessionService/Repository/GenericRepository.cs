using Microsoft.EntityFrameworkCore;
using SessionService.Interfaces;
using SessionService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SessionService.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        TgyDatabaseContext dbContext;
        public GenericRepository(TgyDatabaseContext context)
        {
            dbContext = context;
        }
        public async Task<T> GetByName(string name)
        {
            //bu metod entitylerin repoları içerisinde belirtilmeli. Şuan için çalışmıyor.
            return await dbContext.Set<T>().FirstOrDefaultAsync(t => t.Equals(name));
        }


        public async Task Add(T data)
        {
            dbContext.Set<T>().Add(data);
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            dbContext.Set<T>().Remove(dbContext.Set<T>().Find(id));
            await dbContext.SaveChangesAsync();
        }

        public Task<List<T>> GetAll()
        {
            return dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }



        public async Task Update(int id, T data)
        {
            dbContext.Entry(data).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }
    }
}
