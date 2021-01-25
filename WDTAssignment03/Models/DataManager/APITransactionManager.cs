using System.Collections.Generic;
using System.Linq;
using Admin.Data;
using NWBAWebAPI.Models.Repository;
using Admin.Models;

namespace NWBAWebAPI.Models.DataManager
{
    public class APITransactionManager : IDataRepository<Transaction, int>
    {
        private readonly NWBAContext _context;

        public APITransactionManager(NWBAContext context)
        {
            _context = context;
            _context.ChangeTracker.LazyLoadingEnabled = false;
        }

        public Transaction Get(int id)
        {
            return _context.Transaction.Find(id);
        }

        public IEnumerable<Transaction> GetAll()
        {
            return _context.Transaction.ToList();
        }

        public int Add(Transaction transaction)
        {
            _context.Transaction.Add(transaction);
            _context.SaveChanges();

            return transaction.TransactionId;
        }

        public int Delete(int id)
        {
            _context.Transaction.Remove(_context.Transaction.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, Transaction transaction)
        {
            _context.Update(transaction);
            _context.SaveChanges();

            return id;
        }
    }
}
