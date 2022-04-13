using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SessionService.Core;
using SessionService.DatabaseObject;
using SessionService.Entities;
using SessionService.Interfaces;
using SessionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionService.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper mapper;
        bool success;
        string message = String.Empty;



        public CustomerController(IMapper mapper,ICustomerRepository customerRepository)
        {
            this.mapper = mapper;
            _customerRepository = customerRepository;
        }

        [HttpPost("/addcustomer")]

        public async Task<Result<object>> AddCustomer([FromBody]AddCustomerDto customer)
        {
            try
            {
                Customer addedCustomer = mapper.Map<Customer>(customer);
                await _customerRepository.Add(addedCustomer);
                success = true;
                message = "Customer Datası Eklendi";
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
        public async Task<Result<object>> GetAllCustomer()
        {
            List<Customer> customers = new List<Customer>();
            try
            {

                customers = await _customerRepository.GetAll();
                message = "Tüm Customer Datası Çekildi";
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
                data = customers,
                Message = message,
                Success = success,
            };
        }

        [HttpPost("/updatecustomer")]

        public async Task<Result<object>> UpdateCustomer([FromBody]UpdateCustomerDto customer)
        {



            try
            {
                await _customerRepository.Update(customer.Id, mapper.Map(customer, new Customer()));
                success = true;
                message = "Data Güncellendi";


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

        public async Task<Result<object>> DeleteCustomer([FromBody]int id)
        {


            try
            {
                await _customerRepository.Delete(id);

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

        public async Task<Result<object>> GetByIdCustomer([FromBody]int id)
        {
     

            Customer customer = new Customer();
            try
            {
                customer = await _customerRepository.GetById(id);
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

        public async Task<Result<object>> GetByNameCustomer([FromBody]string name)
        {
            Customer customers = new Customer();


            try
            {
                customers = await _customerRepository.GetByName(name);
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
