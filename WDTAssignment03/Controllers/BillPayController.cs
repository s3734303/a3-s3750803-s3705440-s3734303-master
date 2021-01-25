using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Admin.Data;
using Admin.Models;
using Admin.Attributes;

namespace WDTAssignment03.Controllers
{
    [AuthorizeCustomer]
    public class BillPayController : Controller
    {

        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        private readonly NWBAContext _context;

        public BillPayController(NWBAContext context)
        {
            
            _context = context;
        }

        // GET: BillPay
        public async Task<IActionResult> Index()
        {
            return View(await _context.BillPay.ToListAsync());
        }

        // GET: BillPay/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var billPay = await _context.BillPay
                .FirstOrDefaultAsync(m => m.BillPayId == id);
            if (billPay == null)
            {
                return NotFound();
            }

            return View(billPay);
        }

        // GET: BillPay/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}
        // POST: BillPay/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        public async Task<IActionResult> Create(int id) => View(await _context.Customer.FindAsync(id));
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, int payeeId, decimal amount, DateTime scheduleDate, PeriodType periodType)
        {
            Customer customer = await _context.Customer.FindAsync(CustomerID);
            var account = await _context.Account.FindAsync(id);
            if (!Enum.IsDefined(typeof(PeriodType), periodType))
                ModelState.AddModelError(nameof(periodType), "Illigal Action");
            var payee = await _context.Payee.FindAsync(payeeId);
            if (!_context.Payee.Contains(payee))
                ModelState.AddModelError(nameof(payeeId), "Payee no found");
            if (!customer.Accounts.Contains(account))
                ModelState.AddModelError(nameof(periodType), "Illigal Action");
            if (DateTime.Compare(scheduleDate, DateTime.Now) < 0)
                ModelState.AddModelError(nameof(scheduleDate), "Invalid Time");
            if (!ModelState.IsValid)
            {
                return View();
            }
            _context.Add(new BillPay
            {
                Account = account,
                PayeeId = payeeId,
                Amount = amount,
                ScheduleDate = scheduleDate,
                Period = periodType,
                ModifyDate = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: BillPay/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billPay = await _context.BillPay.FindAsync(id);
            if (billPay == null)
            {
                return NotFound();
            }
            return View(billPay);
        }



        // POST: BillPay/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int billPayId, decimal amount, DateTime scheduleDate, PeriodType periodType)
        {
            BillPay billPay = await _context.BillPay.FindAsync(billPayId);
            if (amount <= 0)
                ModelState.AddModelError((nameof(amount)), "Invalid Amount");
            if (DateTime.Compare(scheduleDate, DateTime.Now) < 0)
                ModelState.AddModelError(nameof(scheduleDate), "Invalid Time");
            if (!Enum.IsDefined(typeof(PeriodType), periodType))
                ModelState.AddModelError(nameof(periodType), "Illigal Action");

            if (ModelState.IsValid)
            {
                billPay.ModifyDate = DateTime.UtcNow;
                billPay.Amount = amount;
                billPay.Period = periodType;
                billPay.ScheduleDate = scheduleDate;
                try
                {
                    _context.Update(billPay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                }

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: BillPay/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billPay = await _context.BillPay
                .FirstOrDefaultAsync(m => m.BillPayId == id);
            if (billPay == null)
            {
                return NotFound();
            }

            return View(billPay);
        }

        // POST: BillPay/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var billPay = await _context.BillPay.FindAsync(id);
            _context.BillPay.Remove(billPay);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillPayExists(int id)
        {
            return _context.BillPay.Any(e => e.BillPayId == id);
        }


    }
}
