using System.Collections.Generic;
using System.Linq;
using Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Admin.Controllers
{
    public class ChartController : Controller
    {
        // Linq query to fetch the data from the transaction table
        public async System.Threading.Tasks.Task<IActionResult> Data()
        {
            var transactionAPI = await NWBAWebAPI.InitializeClient().GetAsync("api/transaction");
            string transactionrstr = transactionAPI.Content.ReadAsStringAsync().Result;
            List<TransactionDto> transactions = JsonConvert.DeserializeObject<List<TransactionDto>>(transactionrstr);
            var result = transactions
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