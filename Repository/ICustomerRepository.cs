using StudioWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudioWebApi.Repository
{
    public interface ICustomerRepository
    {
        public Result GetAllCustomers(Customer customer);

        public Result GetCustomerById(Customer customer);

        public string AddCustomer(Customer customer);

        public Result UpdateCustomer(Customer customer);

        public Result DeleteCustomer(Customer customer);

    }
}
