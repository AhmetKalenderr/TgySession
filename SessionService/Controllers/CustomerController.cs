using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SessionService.Core;
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
        bool success;
        string message = String.Empty;

        public CustomerController(TgyDatabaseContext tgyDatabaseContext)
        {
            _databaseContext = tgyDatabaseContext;
        }

        [HttpPost("/addcustomer")]

        public Result<object> AddCustomer([FromBody]CustomerAddBody customer)
        {
            try
            {
                Customer addedCustomer = new Customer
                {
                    Name = customer.Name,
                    Surname = customer.Surname,
                    SegmentId = customer.SegmentId,
                };

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

            var customer = _databaseContext.Customers.Select(c => new
            {
                c.Name,
                c.Surname,
                c.segment 
            });

            return new Result<object>
            {
                data = _databaseContext.Customers.Include(c=> c.segment),
                Message = "Başarılı",
                Success = true,
            };
        }

        [HttpPost("/updatecustomer")]

        public Result<object> UpdateCustomer([FromBody]Customer customer)
        {

            try
            {
                Customer updatedCustomer = _databaseContext.Customers.Single(c => c.Id == customer.Id);

                updatedCustomer.Name = customer.Name;
                updatedCustomer.Surname = customer.Surname;
                updatedCustomer.SegmentId = customer.SegmentId;

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
                customer = _databaseContext.Customers.Include(customer => customer.segment).Single(customer => customer.Id == id);
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
                customers = _databaseContext.Customers.Include(_databaseContext => _databaseContext.segment).Where(_databaseContext => _databaseContext.Name.StartsWith(name)).ToList();
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
