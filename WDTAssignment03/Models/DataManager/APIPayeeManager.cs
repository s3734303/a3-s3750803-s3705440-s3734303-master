using System.Collections.Generic;
using System.Linq;
using Admin.Data;
using NWBAWebAPI.Models.Repository;
using Admin.Models;

namespace NWBAWebAPI.Models.DataManager
{
    public class APIPayeeManager : IDataRepository<Payee, int>
    {
        private readonly NWBAContext _context;

        public APIPayeeManager(NWBAContext context)
        {
            _context = context;
            _context.ChangeTracker.LazyLoadingEnabled = false;
        }

        public Payee Get(int id)
        {
            return _context.Payee.Find(id);
        }

        public IEnumerable<Payee> GetAll()
        {
            return _context.Payee.ToList();
        }

        public int Add(Payee payee)
        {
            _context.Payee.Add(payee);
            _context.SaveChanges();

            return payee.PayeeId;
        }

        public int Delete(int id)
        {
            _context.Payee.Remove(_context.Payee.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, Payee payee)
        {
            _context.Update(payee);
            _context.SaveChanges();

            return id;
        }
    }
}
