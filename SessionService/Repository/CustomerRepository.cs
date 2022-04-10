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
    public class CustomerRepository : ICustomerRepository
    {
        protected readonly TgyDatabaseContext _context;
        public CustomerRepository(TgyDatabaseContext context)
        {
            _context = context; 
        }

        public async Task Add(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _context.Customers.Remove(_context.Customers.Find(id));
            await _context.SaveChangesAsync();
        }

        public async Task<List<Customer>> GetAll()
        {
            return await _context.Customers.Include(c => c.segment).ToListAsync();
        }

        public async Task<Customer> GetById(int id)
        {
            return await _context.Customers.Include(c => c.segment).SingleAsync(c => c.Id == id);
        }

        public async Task<Customer> GetByName(string name)
        {
           return await _context.Customers.Include(c => c.segment).FirstOrDefaultAsync(c=> c.Name == name);
        }

        public async Task Update(int id, Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
