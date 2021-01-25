using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Admin.Data;
using Admin.Attributes;

namespace WDTAssignment03
{
    [AuthorizeCustomer]
    public class AccountController : Controller
    {
        private readonly NWBAContext _context;

        public AccountController(NWBAContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            var nWBAContext = _context.Account.Include(a => a.Customer);
            return View(await nWBAContext.ToListAsync());
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .Include(a => a.Customer)
                .FirstOrDefaultAsync(m => m.AccountNumber == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }
        private bool AccountExists(int id)
        {
            return _context.Account.Any(e => e.AccountNumber == id);
        }
    }
}
