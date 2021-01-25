using System.Collections.Generic;
using System.Linq;
using Admin.Data;
using NWBAWebAPI.Models.Repository;
using Admin.Models;

namespace NWBAWebAPI.Models.DataManager
{
    public class APIBillPayManager : IDataRepository<BillPay, int>
    {
        private readonly NWBAContext _context;

        public APIBillPayManager(NWBAContext context)
        {
            _context = context;
            _context.ChangeTracker.LazyLoadingEnabled = false;
        }

        public BillPay Get(int id)
        {
            return _context.BillPay.Find(id);
        }

        public IEnumerable<BillPay> GetAll()
        {
            return _context.BillPay.ToList();
        }

        public int Add(BillPay billpay)
        {
            _context.BillPay.Add(billpay);
            _context.SaveChanges();

            return billpay.BillPayId;
        }

        public int Delete(int id)
        {
            _context.BillPay.Remove(_context.BillPay.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, BillPay billpay)
        {
            _context.Update(billpay);
            _context.SaveChanges();

            return id;
        }
    }
}
