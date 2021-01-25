using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Admin.Data;
using Admin.Attributes;

namespace WDTAssignment03.Controllers
{
    [AuthorizeCustomer]
    public class TransactionController : Controller
    {
        private readonly NWBAContext _context;

        public TransactionController(NWBAContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var nWBAContext = _context.Transaction.Include(t => t.Account).Include(t => t.DestinationAccount);
            return View(await nWBAContext.ToListAsync());
        }
        private bool TransactionExists(int id)
        {
            return _context.Transaction.Any(e => e.TransactionId == id);
        }
    }
}
