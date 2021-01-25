using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Admin.Models;
using NWBAWebAPI.Models.DataManager;

namespace NWBAWebAPI.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    public class APITransactionController : Controller
    {
        private readonly APITransactionManager _repo;

        public APITransactionController(APITransactionManager repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IEnumerable<Transaction> Get()
        {
            return _repo.GetAll();
        }

        [HttpGet("{id}")]
        public Transaction Get(int id)
        {
            return _repo.Get(id);
        }

        [HttpPost]
        public void Post([FromBody] Transaction transaction)
        {
            _repo.Add(transaction);
        }

        [HttpPut]
        public void Put([FromBody] Transaction transaction)
        {
            _repo.Update(transaction.TransactionId, transaction);
        }

        [HttpDelete("{id}")]
        public long Delete(int id)
        {
            return _repo.Delete(id);
        }
    }
}
