using System.Collections.Generic;
using System.Linq;
using Admin.Data;
using NWBAWebAPI.Models.Repository;
using Admin.Models;

namespace NWBAWebAPI.Models.DataManager
{
    public class APICustomerManager : IDataRepository<Customer, int>
    {
        private readonly NWBAContext _context;

        public APICustomerManager(NWBAContext context)
        {
            _context = context;
            _context.ChangeTracker.LazyLoadingEnabled = false;
        }

        public Customer Get(int id)
        {
            return _context.Customer.Find(id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customer.ToList();
        }

        public int Add(Customer customer)
        {
            _context.Customer.Add(customer);
            _context.SaveChanges();

            return customer.CustomerID;
        }

        public int Delete(int id)
        {
            _context.Customer.Remove(_context.Customer.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, Customer customer)
        {
            _context.Update(customer);
            _context.SaveChanges();

            return id;
        }
    }
}
