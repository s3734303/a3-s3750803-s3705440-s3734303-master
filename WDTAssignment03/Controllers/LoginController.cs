using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Admin.Data;
using Admin.Models;
using SimpleHashing;
using Microsoft.EntityFrameworkCore;
using Admin.Attributes;
using System;

namespace WDTAssignment03.Controllers
{
    [Route("/Nwba/SecureLogin")]
    public class LoginController : Controller
    {
        [AuthorizeCustomer]
        private readonly NWBAContext _context;

        public LoginController(NWBAContext context) => _context = context;

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string UserID, string password)
        {
            var login = await _context.Login.Include(x => x.Customer).FirstOrDefaultAsync(x => x.UserID == UserID);
            if (login == null || !PBKDF2.Verify(login.Password, password))
            {

                if (login != null)
                {
                    
                    if (login.Flag < 3)
                    {
                        login.Flag += 1;
                        _context.Login.Update(login);
                        await _context.SaveChangesAsync();
                        ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
                        return View(new Login { UserID = UserID });
                    }
                    else
                    {
                        login.Flag = 0;
                        login.Timer = DateTime.UtcNow.AddMinutes(1);
                        _context.Login.Update(login);
                        await _context.SaveChangesAsync();
                        ModelState.AddModelError("LoginFailed", "Your account has been temporarily suspended. Cooldown period: 1 minute");
                        return View(new Login { UserID = UserID });
                    }
                }

                


                ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
                return View(new Login { UserID = UserID });
            }
            if (DateTime.Compare(login.Timer, DateTime.UtcNow) > 0)
            {
                ModelState.AddModelError("LoginFailed", "Your account has been temporarily suspended. Cooldown period: 1 minute");
                return View(new Login { UserID = UserID });
            }
            // Login customer.
            HttpContext.Session.SetInt32("CustomerID", login.CustomerID);
            HttpContext.Session.SetString("CustomerName", login.Customer.CustomerName);

            return RedirectToAction("Index", "Customer");
        }

        [Route("LogoutNow")]
        public IActionResult Logout()
        {
            // Logout customer.
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

    }

}
