using Admin.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Admin.Controllers
{
    // Secure URL - Customised URLs
    [Route("/Nwba/SecureLogin")]
    public class LoginController : Controller
    {
        public IActionResult Login() => View();

        // Admin login functionality
        [HttpPost,ActionName("Login")]
        public async Task<IActionResult> Login(string adminID, string password)
        {
            
            if (!(adminID.Equals("admin") && password.Equals("admin")))
            {
                ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
                return View(new LoginDto { UserID = adminID });
            }

            HttpContext.Session.SetInt32("admin", 1);
            HttpContext.Session.SetString("admin", "admin");

            return RedirectToAction("Index", "Admin");
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
