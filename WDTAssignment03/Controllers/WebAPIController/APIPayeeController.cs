using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Admin.Models;
using NWBAWebAPI.Models.DataManager;

namespace NWBAWebAPI.Controllers
{
    [ApiController]
    [Route("api/payee")]
    public class APIPayeeController : Controller
    {
        private readonly APIPayeeManager _repo;

        public APIPayeeController(APIPayeeManager repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IEnumerable<Payee> Get()
        {
            return _repo.GetAll();
        }

        [HttpGet("{id}")]
        public Payee Get(int id)
        {
            return _repo.Get(id);
        }

        [HttpPost]
        public void Post([FromBody] Payee payee)
        {
            _repo.Add(payee);
        }

        [HttpPut]
        public void Put([FromBody] Payee payee)
        {
            _repo.Update(payee.PayeeId, payee);
        }

        [HttpDelete("{id}")]
        public long Delete(int id)
        {
            return _repo.Delete(id);
        }
    }
}
