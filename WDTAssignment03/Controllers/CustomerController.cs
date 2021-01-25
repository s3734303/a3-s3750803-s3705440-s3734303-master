using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Admin.Data;
using Admin.Models;
using WDTAssignment03.Utilities;

using X.PagedList;
using Newtonsoft.Json;
using Admin.Attributes;

namespace WDTAssignment03.Controllers
{
    [AuthorizeCustomer]
    public class CustomerController : Controller
    {
        private readonly NWBAContext _context;
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;


        public CustomerController(NWBAContext context)
        {
            
            _context = context;
        }


        // GET: Customers
        public async Task<IActionResult> Index()
        {
            if (!HttpContext.Session.GetInt32(nameof(CustomerID)).HasValue)
                return RedirectToAction("Index", "Home");
            var customer = await _context.Customer.FindAsync(CustomerID);
            return View(customer);
        }


        //Deposit
        public async Task<IActionResult> Deposit(int id) => View(await _context.Account.FindAsync(id));
        [HttpPost]
        public async Task<IActionResult> Deposit(int id, decimal amount)
        {
            if (!HttpContext.Session.GetInt32(nameof(CustomerID)).HasValue)
                return RedirectToAction("Index", "Home");
            var account = await _context.Account.FindAsync(id);

            if (amount <= 0)
                ModelState.AddModelError(nameof(amount), "Amount must be positive.");
            if (amount.HasMoreThanTwoDecimalPlaces())
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            if (!ModelState.IsValid)
            {
                ViewBag.Amount = amount;
                return View(account);
            }

            account.Balance += amount;
            account.Transactions.Add(
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    Amount = amount,
                    ModifyDate = DateTime.UtcNow
                });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Withdraw
        public async Task<IActionResult> Withdraw(int id) => View(await _context.Account.FindAsync(id));
        [HttpPost]
        public async Task<IActionResult> Withdraw(int id, decimal amount)
        {
            if (!HttpContext.Session.GetInt32(nameof(CustomerID)).HasValue)
                return RedirectToAction("Index", "Home");
            Account account = await _context.Account.FindAsync(id);
            TransactionType transactionType = TransactionType.Withdraw;
            bool balanceCheck = MiscellaneousExtensionUtilities.BalanceCheck(account, amount, transactionType);
            if (amount <= 0)
                ModelState.AddModelError(nameof(amount), "Negative Amount");
            if (amount.HasMoreThanTwoDecimalPlaces())
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            if (!balanceCheck)
                ModelState.AddModelError(nameof(amount), "Insufficient fund.");
            if (!ModelState.IsValid)
            {
                ViewBag.Amount = amount;
                return View(account);
            }
            account.Balance -= amount;
            account.Transactions.Add(
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    Amount = amount,
                    ModifyDate = DateTime.UtcNow
                });
            FeeCharge(account, transactionType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Transfer Funds
        public async Task<IActionResult> TransferAccount(int id) => View(await _context.Account.FindAsync(id));
        [HttpPost, ActionName("TransferAccount")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TransferAccount(int id, int targetId, decimal amount, String comment)
        {
            if (!HttpContext.Session.GetInt32(nameof(CustomerID)).HasValue)
                return RedirectToAction("Index", "Home");
            var account = await _context.Account.FindAsync(id);
            var target = await _context.Account.FindAsync(targetId);
            TransactionType transactionType = TransactionType.Transfer;
            bool balanceCheck = MiscellaneousExtensionUtilities.BalanceCheck(account, amount, transactionType);
            if (!balanceCheck)
                ModelState.AddModelError(nameof(amount), "Insufficient fund.");
            if (id == targetId)
                ModelState.AddModelError(nameof(targetId), "Cannot transfer to the same account");
            if (!_context.Account.Contains(target))
                ModelState.AddModelError(nameof(targetId), "Account not found");
            if (!ModelState.IsValid)
            {
                ViewBag.Amount = amount;
                return View(account);
            }
            account.Balance -= amount;
            target.Balance += amount;
            account.Transactions.Add(new Transaction
            {
                Account = account,
                Amount = amount,
                DestinationAccount = target,
                Comment = comment,
                TransactionType = transactionType,
                ModifyDate = DateTime.UtcNow
            });
            FeeCharge(account, transactionType);
            target.Transactions.Add(new Transaction
            {
                Account = target,
                Amount = amount,
                Comment = string.Format("Transfered from{0},AccountNumber", id),
                TransactionType = TransactionType.Deposit,
                ModifyDate = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!HttpContext.Session.GetInt32(nameof(CustomerID)).HasValue)
                return RedirectToAction("Index", "Home");
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (!HttpContext.Session.GetInt32(nameof(CustomerID)).HasValue)
                return RedirectToAction("Index", "Home");
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerID,CustomerName,TFN,CustAddress,CustCity,CustState,PostCode,PhoneNum")] Customer customer)
        {
            if (!HttpContext.Session.GetInt32(nameof(CustomerID)).HasValue)
                return RedirectToAction("Index", "Home");
            if (id != customer.CustomerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.CustomerID == id);
        }

        private void FeeCharge(Account account, TransactionType transactionType)
        {
            var counter = account.Transactions.Count(n => n.TransactionType == TransactionType.Transfer)
                + account.Transactions.Count(n => n.TransactionType == TransactionType.Withdraw);
            if (counter >= 4)
            {

                String comment = null;
                Decimal fee = 0;
                switch (transactionType)
                {
                    case (TransactionType.Transfer):
                        comment = "Transfer Fee";
                        fee = Convert.ToDecimal(0.2);
                        break;
                    case (TransactionType.Withdraw):
                        comment = "Withdraw Fee";
                        fee = Convert.ToDecimal(0.1);
                        break;
                }
                account.Balance -= fee;
                account.Transactions.Add(new Transaction
                {
                    TransactionType = TransactionType.ServiceCharge,
                    Amount = fee,
                    Comment = comment,
                    ModifyDate = DateTime.UtcNow
                }
                );
            }
        }

        public string AccountSessionKey = "_AccountSessionKey";
        [ActionName("IndexToViewTransaction")]
        public async Task<IActionResult> IndexToViewTransaction(int id)
        {
            var account = await _context.Account.FindAsync(id);
            if (account == null)
                return NotFound();

            // Store a complex object in the session via JSON serialisation.
            var accountJson = JsonConvert.SerializeObject(account, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            HttpContext.Session.SetString(AccountSessionKey, accountJson);

            return RedirectToAction(nameof(ViewTransaction));
        }

        public async Task<IActionResult> ViewTransaction(int? page = 1)
        {
            if (!HttpContext.Session.GetInt32(nameof(CustomerID)).HasValue)
                return RedirectToAction("Index", "Home");
            var accountJson = HttpContext.Session.GetString(AccountSessionKey);
            if (accountJson == null)
                return RedirectToAction(nameof(Index));

            //Retrieve complex objects from the session 
            var account = JsonConvert.DeserializeObject<Transaction>(accountJson);
            ViewBag.Account = account;

            //Page the orders, maximum of 4 transactions per page
            const int pageSize = 4;
            var pagedList = await _context.Transaction.Where(x => x.AccountNumber == account.AccountNumber).
                ToPagedListAsync(page, pageSize);

            return View(pagedList);
        }
    }
}
