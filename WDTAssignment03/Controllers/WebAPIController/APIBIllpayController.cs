using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Admin.Models;
using NWBAWebAPI.Models.DataManager;

namespace NWBAWebAPI.Controllers
{
    [ApiController]
    [Route("api/billpay")]
    public class BillPayController : Controller
    {
        private readonly APIBillPayManager _repo;

        public BillPayController(APIBillPayManager repo)
        {
            _repo = repo;
        }

        // GET: api/accounts
        [HttpGet]
        public IEnumerable<BillPay> Get()
        {
            return _repo.GetAll();
        }

        // GET api/accounts/1
        [HttpGet("{id}")]
        public BillPay Get(int id)
        {
            return _repo.Get(id);
        }

        // POST api/accounts
        [HttpPost]
        public void Post([FromBody] BillPay billpay)
        {
            _repo.Add(billpay);
        }

        // PUT api/accounts
        [HttpPut]
        public void Put([FromBody] BillPay billpay)
        {
            _repo.Update(billpay.BillPayId, billpay);
        }

        // DELETE api/accounts/1
        [HttpDelete("{id}")]
        public long Delete(int id)
        {
            return _repo.Delete(id);
        }
    }
}
