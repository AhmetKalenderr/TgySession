using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SessionService.Core;
using SessionService.DatabaseObject;
using SessionService.Entities;
using SessionService.Interfaces;
using SessionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionService.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDistributedCache _distributed;
        private readonly IMapper mapper;
        bool success;
        string message = String.Empty;
        string getAllKey = "GetAllCustomer";




        public CustomerController(IMapper mapper,ICustomerRepository customerRepository,IDistributedCache distributed)
        {
            this.mapper = mapper;
            _customerRepository = customerRepository;
            _distributed = distributed;
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

                //Redisten eski customer listesini siliyoruz.
                if (await _distributed.GetAsync(getAllKey) != null)
                {
                    await _distributed.RemoveAsync(getAllKey);
                }
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
            List<Customer> customers;
            
            var getAllCustomerCache = await _distributed.GetAsync(getAllKey);

            string cacheJsonItem;

            try
            {

                //Rediste var ise redisten getir yoksa repodan getir ve redise kaydet.
            if (getAllCustomerCache != null)
            {
                cacheJsonItem = Encoding.UTF8.GetString(getAllCustomerCache);
                customers = JsonConvert.DeserializeObject<List<Customer>>(cacheJsonItem);
                Console.WriteLine("Redisten Getirildi");
            }
            else
            {
                customers = await _customerRepository.GetAll();
                cacheJsonItem = JsonConvert.SerializeObject(customers);
                getAllCustomerCache = Encoding.UTF8.GetBytes(cacheJsonItem);

                var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1)).SetAbsoluteExpiration(DateTime.Now.AddMonths(1));
                await _distributed.SetAsync(getAllKey, getAllCustomerCache, options);
                Console.WriteLine("Repodan Getirildi");

            }

           
                message = "Tüm Customer Datası Çekildi";
                success = true;


            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                customers = new List<Customer>();
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
            var getByIdCustomerCache = await _distributed.GetAsync("Customer get by id:" + customer.Id.ToString());

            try
            {
                await _customerRepository.Update(customer.Id, mapper.Map(customer, new Customer()));
                success = true;
                message = "Data Güncellendi";

                //Redisten eski customer listesini siliyoruz.
                if (await _distributed.GetAsync(getAllKey) != null)
                {
                    await _distributed.RemoveAsync(getAllKey);
                }

                //Redisteki customer nesnesini de güncelliyoruz.
                if (getByIdCustomerCache != null)
                {
                    Console.WriteLine("Rediste Güncellendi");
                    var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1)).SetAbsoluteExpiration(DateTime.Now.AddMonths(1));
                    await _distributed.SetAsync("Customer get by id:" + customer.Id, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mapper.Map(customer, new Customer()))), options);
                }


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

            var getByIdCustomerCache = await _distributed.GetAsync("Customer get by id:" + id.ToString());



            try
            {
                await _customerRepository.Delete(id);


                //Redisteki customer nesnesini de temizliyoruz.
                if (getByIdCustomerCache != null)
                {
                    Console.WriteLine("Redisten Silindi");
                    await _distributed.RemoveAsync("Customer get by id:" + id.ToString());
                }


                //Redisten eski customer listesini siliyoruz.
                if (await _distributed.GetAsync(getAllKey) != null)
                {
                    await _distributed.RemoveAsync(getAllKey);
                }

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

            string cacheJsonItem;

            var getByIdCustomerCache = await _distributed.GetAsync("Customer get by id:"+id.ToString());
            
            try
            {
                //İlgili id ile kayıtlı rediste customer datası var sa getir yoksa repodan getir redise kayıt at.
                if (getByIdCustomerCache != null)
            {
                Console.WriteLine("Redisten geldi");
                cacheJsonItem = Encoding.UTF8.GetString(getByIdCustomerCache);
                customer = JsonConvert.DeserializeObject<Customer>(cacheJsonItem);
            }
            else
            {
                customer = await _customerRepository.GetById(id);

                cacheJsonItem = JsonConvert.SerializeObject(customer);

                getByIdCustomerCache = Encoding.UTF8.GetBytes(cacheJsonItem);

                var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1)).SetAbsoluteExpiration(DateTime.Now.AddMonths(1));

                await _distributed.SetAsync("Customer get by id:" + id.ToString(), getByIdCustomerCache, options);

                Console.WriteLine("Repodan Geldi");
            }


           
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

            string cacheJsonItem;

            var getByNameCustomerCache = await _distributed.GetAsync("Customer get by name:" + name);

            try
            {

                if (getByNameCustomerCache != null)
            {
                Console.WriteLine("Redisten geldi");
                cacheJsonItem = Encoding.UTF8.GetString(getByNameCustomerCache);
                customers = JsonConvert.DeserializeObject<Customer>(cacheJsonItem);
            }
            else
            {
                customers =  await _customerRepository.GetByName(name);

                cacheJsonItem = JsonConvert.SerializeObject(customers);

                getByNameCustomerCache = Encoding.UTF8.GetBytes(cacheJsonItem);

                var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1)).SetAbsoluteExpiration(DateTime.Now.AddMonths(1));

                await _distributed.SetAsync("Customer get by name:" + name, getByNameCustomerCache, options);

                Console.WriteLine("Repodan Geldi");
            }

          
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
