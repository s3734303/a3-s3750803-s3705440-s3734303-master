using System.Collections.Generic;
using System.Linq;
using Admin.Data;
using NWBAWebAPI.Models.Repository;
using Admin.Models;

namespace NWBAWebAPI.Models.DataManager
{
    public class APILoginManager : IDataRepository<Login, string>
    {
        private readonly NWBAContext _context;

        public APILoginManager(NWBAContext context)
        {
            _context = context;
            _context.ChangeTracker.LazyLoadingEnabled = false;
        }

        public Login Get(string id)
        {
            return _context.Login.Find(id);
        }

        public IEnumerable<Login> GetAll()
        {
            return _context.Login.ToList();
        }

        public string Add(Login login)
        {
            _context.Login.Add(login);
            _context.SaveChanges();

            return login.UserID;
        }

        public string Delete(string id)
        {
            _context.Login.Remove(_context.Login.Find(id));
            _context.SaveChanges();

            return id;
        }

        public string Update(string id, Login login)
        {
            _context.Update(login);
            _context.SaveChanges();

            return id;
        }
    }
}
