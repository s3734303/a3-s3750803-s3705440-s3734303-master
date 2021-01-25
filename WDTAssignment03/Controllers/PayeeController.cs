using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Admin.Data;
using Admin.Models;
using Admin.Attributes;

namespace WDTAssignment03.Controllers
{
    [AuthorizeCustomer]
    public class PayeeController : Controller
    {
        private readonly NWBAContext _context;

        public PayeeController(NWBAContext context)
        {
            _context = context;
        }

        // GET: Payees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Payee.ToListAsync());
        }

        private bool PayeeExists(int id)
        {
            return _context.Payee.Any(e => e.PayeeId == id);
        }
    }
}
