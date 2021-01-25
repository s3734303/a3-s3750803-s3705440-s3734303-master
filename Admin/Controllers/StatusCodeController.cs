using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    public class StatusCodeController : Controller
    {
        [HttpGet("/StatusCode/{errorCode}")]
        public IActionResult Index(int errorCode)
        {
            return View(errorCode);
        }
    }
}