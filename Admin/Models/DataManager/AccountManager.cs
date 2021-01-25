using System.Collections.Generic;
using System.Linq;
using Admin.Data;
using Admin.Models;

namespace NWBAWebAPI.Models.DataManager
{
    public class AccountManager : IDataRepository<Account, int>
    {
        private readonly NWBAContext _context;

        public AccountManager(NWBAContext context)
        {
            _context = context;
        }

        public Account Get(int id)
        {
            return _context.Account.Find(id);
        }

        public IEnumerable<Account> GetAll()
        {
            return _context.Account.ToList();
        }

        public int Add(Account account)
        {
            _context.Account.Add(account);
            _context.SaveChanges();

            return account.AccountNumber;
        }

        public int Delete(int id)
        {
            _context.Account.Remove(_context.Account.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, Account account)
        {
            _context.Update(account);
            _context.SaveChanges();

            return id;
        }
    }
}
