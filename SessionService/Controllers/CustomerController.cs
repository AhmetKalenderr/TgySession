using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SessionService.Core;
using SessionService.DatabaseObject;
using SessionService.Entities;
using SessionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionService.Controllers
{
    public class CustomerController : Controller
    {
        private readonly TgyDatabaseContext _databaseContext;
        private readonly IMapper mapper;
        bool success;
        string message = String.Empty;

        LogUnitDto<object> logUnit = new LogUnitDto<object>();



        public CustomerController(TgyDatabaseContext tgyDatabaseContext,IMapper mapper)
        {
            _databaseContext = tgyDatabaseContext;
            this.mapper = mapper;
            logUnit.TableName = "Customer";

        }

        [HttpPost("/addcustomer")]

        public async Task<Result<object>> AddCustomer([FromBody]AddCustomerDto customer)
        {

            logUnit.ActionType =ActionType.ADD;
            logUnit.ActionTime = DateTime.Parse(DateTime.Now.ToLocalTime().ToString("dd'/'MM'/'yyyy HH:mm:ss"));


            try
            {
                Customer addedCustomer = mapper.Map<Customer>(customer);

                _databaseContext.Customers.Add(addedCustomer);

                addedCustomer.segment = await _databaseContext.Segments.SingleAsync(s => s.Id == addedCustomer.SegmentId);

                logUnit.Object = addedCustomer;
                
                success = true;
                message = "Customer Datası Eklendi";

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                success = false;
                message = e.Message;

                logUnit.Object = null;


            }finally
            {
                logUnit.Message = message;
                logUnit.Success = success;
                await _databaseContext.Logs.AddAsync(new Log() { Data = JsonConvert.SerializeObject(logUnit) });
                await _databaseContext.SaveChangesAsync();
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
            logUnit.ActionTime = DateTime.Parse(DateTime.Now.ToLocalTime().ToString("dd'/'MM'/'yyyy HH:mm:ss"));
            logUnit.ActionType = ActionType.GET;
            logUnit.Object = null;

            try
            {

                customers =  await _databaseContext.Customers.Include(c => c.segment).ToListAsync();
                message = "Tüm Customer Datası Çekildi";
                success = true;


            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                message = e.Message;
                success = false;
            }finally
            {
                logUnit.Success=success;
                logUnit.Message = message;

                await _databaseContext.Logs.AddAsync(new Log() { Data = JsonConvert.SerializeObject(logUnit) });
                await _databaseContext.SaveChangesAsync();
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

            logUnit.ActionTime = DateTime.Parse(DateTime.Now.ToLocalTime().ToString("dd'/'MM'/'yyyy HH:mm:ss"));
            logUnit.ActionType = ActionType.UPDATE;

            try
            {
                Customer updatedCustomer = await _databaseContext.Customers.SingleAsync(c => c.Id == customer.Id);




                mapper.Map(customer,updatedCustomer);




                await _databaseContext.SaveChangesAsync();
                success = true;
                message = "Data Güncellendi";
                updatedCustomer.segment = _databaseContext.Segments.Single(s => s.Id == updatedCustomer.SegmentId);

                logUnit.Object = updatedCustomer;

            }
            catch (Exception e)
            {
                success = false;
                message = e.Message;
                Console.WriteLine(e.Message);

                logUnit.Object = null;

                await _databaseContext.Logs.AddAsync(new Log() { Data = JsonConvert.SerializeObject(logUnit) });
                await _databaseContext.SaveChangesAsync();

            }
            finally
            {

                logUnit.Success = success;
                logUnit.Message = message;

                await _databaseContext.Logs.AddAsync(new Log() { Data = JsonConvert.SerializeObject(logUnit) });
                await _databaseContext.SaveChangesAsync();
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
            logUnit.ActionTime = DateTime.Parse(DateTime.Now.ToLocalTime().ToString("dd'/'MM'/'yyyy HH:mm:ss"));
            logUnit.ActionType = ActionType.DELETE;

            try
            {
                Customer deletedCustomers = await _databaseContext.Customers.SingleAsync(c => c.Id == id);

                _databaseContext.Customers.Remove(deletedCustomers);

                await _databaseContext.SaveChangesAsync();

                success = true;
                message = "Başarılı";

                logUnit.Object = deletedCustomers;





            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                success = false;
                message = e.Message;

                logUnit.Object = null;

            }
            finally
            {
                logUnit.Success = success;
                logUnit.Message = message;

                await _databaseContext.Logs.AddAsync(new Log() { Data = JsonConvert.SerializeObject(logUnit) });
                await _databaseContext.SaveChangesAsync();
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
            logUnit.ActionTime = DateTime.Parse(DateTime.Now.ToLocalTime().ToString("dd'/'MM'/'yyyy HH:mm:ss"));
            logUnit.ActionType = ActionType.GET;

            Customer customer = new Customer();
            try
            {
                customer = await _databaseContext.Customers.Include(customer => customer.segment).AsNoTracking().SingleAsync(customer => customer.Id == id);
                message = "Başarılı";
                success = true;
                logUnit.Object= customer;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                message = e.Message;
                success = false;
                logUnit.Object = null;
            }
            finally
            {
                logUnit.Success = success;
                logUnit.Message = message;

                await _databaseContext.Logs.AddAsync(new Log() { Data = JsonConvert.SerializeObject(logUnit) });
                await _databaseContext.SaveChangesAsync();
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
            List<Customer> customers = new List<Customer>();

            logUnit.ActionTime = DateTime.Parse(DateTime.Now.ToLocalTime().ToString("dd'/'MM'/'yyyy HH:mm:ss"));
            logUnit.ActionType = ActionType.GET;
            try
            {
                customers = await _databaseContext.Customers.Include(_databaseContext => _databaseContext.segment).Where(_databaseContext => _databaseContext.Name.Equals(name)).AsNoTracking().ToListAsync();
                message = "Başarılı";
                success = true;

                logUnit.Object = customers;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                success =false;
                message = e.Message;

                logUnit.Object = null;
            }
            finally
            {
                logUnit.Success = success;
                logUnit.Message = message;

                await _databaseContext.Logs.AddAsync(new Log() { Data = JsonConvert.SerializeObject(logUnit) });
                await _databaseContext.SaveChangesAsync();
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
