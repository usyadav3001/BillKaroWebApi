using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudioWebApi.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ICustomerRepository _customerRepository;
        //public ICustomerRepository customerRepository
        //{
        //    get
        //    {
        //        if (_customerRepository is null)
        //            _customerRepository = new customerRepository();
        //        return _customerRepository;
        //    }
        //}

        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
