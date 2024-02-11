using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudioWebApi.Models;
using StudioWebApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudioWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository customerRepository;
        public CustomerController(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        [Route("GetAllCustomers")]
        [HttpPost]
        public IActionResult GetAllCustomers(Customer customer)
        {
            var customers = customerRepository.GetAllCustomers(customer);
            return Ok(customers);
        }

        [Route("GetCustomerById")]
        [HttpPost]
        public IActionResult GetCustomerById(Customer customer)
        {
            var customerData = customerRepository.GetCustomerById(customer);

            if (customerData == null)
                return NotFound();

            return Ok(customerData);
        }

        [Route("AddCustomer")]
        [HttpPost]
        public IActionResult AddCustomer([FromBody] Customer customer)
        {
            customerRepository.AddCustomer(customer);
            var customerData = customerRepository.GetCustomerById(customer);
            return Ok(customerData);
        }

        [Route("UpdateCustomer")]
        [HttpPost]
        public IActionResult UpdateCustomer([FromBody] Customer customer)
        {
            var existingCustomer = customerRepository.GetCustomerById(customer);

            if (existingCustomer == null)
                return NotFound();

            var result=customerRepository.UpdateCustomer(customer);

            return Ok(result);
        }

        [Route("DeleteCustomer")]
        [HttpPost]
        public IActionResult DeleteCustomer(Customer customer)
        {
            var existingCustomer = customerRepository.GetCustomerById(customer);

            if (existingCustomer == null)
                return NotFound();

            var result= customerRepository.DeleteCustomer(customer);

            return Ok(result);
        }
    }
}
