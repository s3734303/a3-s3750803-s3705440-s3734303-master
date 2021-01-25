using System;
using System.Text;
using Admin.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;


namespace Admin.Controllers
{
    public class AdminController : Controller
    {
        // Returns the customer information in a tabular format
        public async Task<IActionResult> Index()
        {
            var response = await NWBAWebAPI.InitializeClient().GetAsync("api/customer");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            // Storing the response details recieved from web api.
            var result = response.Content.ReadAsStringAsync().Result;

            // Deserializing the response recieved from web api and storing into a list.
            var customers = JsonConvert.DeserializeObject<List<CustomerDto>>(result);

            return View(customers);
        }


        // Returns the list of transactions in a tabular format
        [ActionName("ViewTransaction")]
        public async Task<IActionResult> ViewTransactions()
        {
            var response = await NWBAWebAPI.InitializeClient().GetAsync("api/transaction");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            // Storing the response details recieved from web api.
            var result = response.Content.ReadAsStringAsync().Result;

            // Deserializing the response recieved from web api and storing into a list.
            var transaction = JsonConvert.DeserializeObject<List<TransactionDto>>(result);

            return View(transaction);
        }


        // Returns the login status of customers
        [ActionName("CheckLogin")]
        public async Task<IActionResult> CheckLogin()
        {
            if (!HttpContext.Session.GetInt32("admin").HasValue)
                return RedirectToAction("Index", "Home");

            var loginAPI = await NWBAWebAPI.InitializeClient().GetAsync("api/login");
            string loginstr = loginAPI.Content.ReadAsStringAsync().Result;
            List<LoginDto> logins = JsonConvert.DeserializeObject<List<LoginDto>>(loginstr);
            return View(logins);
        }


        // Displays the UI for editing the customer profile
        // GET: Customer/Edit/1
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var response = await NWBAWebAPI.InitializeClient().GetAsync($"api/customer/{id}");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            var result = response.Content.ReadAsStringAsync().Result;
            var customer = JsonConvert.DeserializeObject<CustomerDto>(result);

            return View(customer);
        }

        // POST: Customer/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CustomerDto customer)
        {
            if (id != customer.CustomerID)
                return NotFound();

            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                var response = NWBAWebAPI.InitializeClient().PutAsync("api/customer", content).Result;

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");
            }

            return View(customer);
        }


        // Returns all the scheduled bill payments
        [ActionName("CheckBillPay")]
        public async Task<IActionResult> CheckBillPay()
        {
            if (!HttpContext.Session.GetInt32("admin").HasValue)
                return RedirectToAction("Index", "Home");
            var billPayAPI = await NWBAWebAPI.InitializeClient().GetAsync("api/billpay");
            if (!billPayAPI.IsSuccessStatusCode)
                throw new Exception();
            string billPaystr = billPayAPI.Content.ReadAsStringAsync().Result;

            List<BillPayDto> billPays = JsonConvert.DeserializeObject<List<BillPayDto>>(billPaystr);
            return View(billPays);
        }


        // Option to toggle between blocking and unblocking the scheduled payments
        [HttpPost,ActionName("CheckBillPay")]
        public async Task<IActionResult> ActiveChange(int id)
        {
            if (!HttpContext.Session.GetInt32("admin").HasValue)
                return RedirectToAction("Index", "Home");
            var billPayAPI = await NWBAWebAPI.InitializeClient().GetAsync("api/billpay");
            if (!billPayAPI.IsSuccessStatusCode)
                throw new Exception();
            string billPaystr = billPayAPI.Content.ReadAsStringAsync().Result;
            List<BillPayDto> billPays = JsonConvert.DeserializeObject<List<BillPayDto>>(billPaystr);
            BillPayDto billPay = billPays.Find(n => n.BillPayId.Equals(id));
            if (billPay.Active)
                billPay.Active= false;
            else
                billPay.Active = true;
            var result =  await NWBAWebAPI.InitializeClient().PutAsJsonAsync("api/billpay", billPay);
            return RedirectToAction(nameof(CheckBillPay));
        }


        // Lock the user account
        [ActionName("blocklogin"),HttpPost]
        public async Task<IActionResult> ManualLockdown(String id)
        {
            if (!HttpContext.Session.GetInt32("admin").HasValue)
                return RedirectToAction("Index", "Home");
            var loginAPI = await NWBAWebAPI.InitializeClient().GetAsync("api/login");
            string loginstr = loginAPI.Content.ReadAsStringAsync().Result;
            List<LoginDto> logins = JsonConvert.DeserializeObject<List<LoginDto>>(loginstr);
            LoginDto login = logins.Find(n => n.UserID.Equals(id));

            if (DateTime.Compare(login.Timer, DateTime.UtcNow) > 0)
                login.Timer = DateTime.UtcNow;
            else
                login.Timer = DateTime.UtcNow.AddMinutes(1);

            await NWBAWebAPI.InitializeClient().PutAsJsonAsync("api/login", login);
            return RedirectToAction(nameof(CheckLogin));
        }
    }
}