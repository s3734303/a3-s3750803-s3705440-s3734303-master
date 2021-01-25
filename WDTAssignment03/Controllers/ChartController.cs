using System.Linq;
using System.Collections.Generic;
using Admin.Data;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    public class ChartController : Controller
    {
        private NWBAContext _context;

        public ChartController(NWBAContext context)
        {
            this._context = context;
        }

        public IActionResult Data()
        {
            var result = _context.Transaction
                           .GroupBy(x => new { group = x.AccountNumber })
                           .Select(group => new
                           {
                               AccountNumber = group.Key.group,
                               count = group.Count()
                           }
                          ).OrderByDescending(o => o.count).ToList();

            var labels = result.Select(x => x.AccountNumber).ToArray();
            var values = result.Select(x => x.count).ToArray();

            List<object> list1 = new List<object>
            {
                labels,
                values
            };

            return Json(list1);
        }
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}