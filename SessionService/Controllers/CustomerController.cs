using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SessionService.Core;
using SessionService.DatabaseObject;
using SessionService.Entities;
using SessionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SessionService.Controllers
{
    public class CustomerController : Controller
    {
        private readonly TgyDatabaseContext _databaseContext;
        private readonly IMapper mapper;
        bool success;
        string message = String.Empty;

        public CustomerController(TgyDatabaseContext tgyDatabaseContext,IMapper mapper)
        {
            _databaseContext = tgyDatabaseContext;
            this.mapper = mapper;

        }

        [HttpPost("/addcustomer")]

        public Result<object> AddCustomer([FromBody]AddCustomerDto customer)
        {
            try
            {
                Customer addedCustomer = mapper.Map<Customer>(customer);

                _databaseContext.Customers.Add(addedCustomer);
                _databaseContext.SaveChanges();

                success = true;
                message = "Başarılı";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                success = false;
                message = e.Message;      
            }


            return new Result<object>
            {
                data = null,
                Message = message,
                Success = success
            };
        }


        [HttpPost("/getallcustomers")]
        public Result<object> GetAllCustomer()
        {

            return new Result<object>
            {
                data = _databaseContext.Customers.Include(c=> c.segment).ToList(),
                Message = "Başarılı",
                Success = true,
            };
        }

        [HttpPost("/updatecustomer")]

        public Result<object> UpdateCustomer([FromBody]UpdateCustomerDto customer)
        {

            try
            {
                Customer updatedCustomer = _databaseContext.Customers.Single(c => c.Id == customer.Id);



                //Sorulacak..
                #region Trading

                mapper.Map(customer,updatedCustomer);


                //updatedCustomer = mapper.Map<Customer>(customer);

                Console.WriteLine($"{updatedCustomer.Id} {updatedCustomer.Name} : {updatedCustomer.Surname} : {updatedCustomer.SegmentId}");

                //_databaseContext.Customers.Attach(updatedCustomer);

                //if (updatedCustomer != null)
                //{
                //    _databaseContext.Set<Customer>().Attach(updatedCustomer);

                //    _databaseContext.Entry<Customer>(updatedCustomer).State = EntityState.Modified;
                //}
                #endregion


                //updatedCustomer.Name = customer.Name;
                //updatedCustomer.Surname = customer.Surname;
                //updatedCustomer.SegmentId = customer.SegmentId;

                _databaseContext.SaveChanges();
                success = true;
                message = "Başarılı";
            }
            catch (Exception e)
            {
                success = false;
                message = e.Message;
                Console.WriteLine(e.Message);

            }

            return new Result<object>
            {
                data = null,
                Success = success,
                Message = message,
            };
        }

        [HttpPost("/deletecustomer")]

        public Result<object> DeleteCustomer([FromBody]int id)
        {
            try
            {
                Customer deletedCustomers = _databaseContext.Customers.Single(c => c.Id == id);

                _databaseContext.Customers.Remove(deletedCustomers);

                _databaseContext.SaveChanges();

                success = true;
                message = "Başarılı";

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                success = false;
                message = e.Message;
            }

            return new Result<object>
            {
                data = null,
                Success = success,
                Message = message,
            };
        }

        [HttpPost("/getbyidcustomer")]

        public Result<object> GetByIdCustomer([FromBody]int id)
        {
            Customer customer = new Customer();
            try
            {
                customer = _databaseContext.Customers.Include(customer => customer.segment).AsNoTracking().Single(customer => customer.Id == id);
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
                data= customer,
                Success = success,
                Message = message
            };
        }

        [HttpPost("getbynamecustomers")]

        public Result<object> GetByNameCustomer([FromBody]string name)
        {
            List<Customer> customers = new List<Customer>();

           

            try
            {
                customers = _databaseContext.Customers.Include(_databaseContext => _databaseContext.segment).Where(_databaseContext => _databaseContext.Name.Equals(name)).AsNoTracking().ToList();
                message = "Başarılı";
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                success =false;
                message = e.Message;
            }


            return new Result<object>
            {
                data = customers,
                Message = message,
                Success = success
            };
        }
    }
}
