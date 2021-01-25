using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Admin.Models;
using NWBAWebAPI.Models.DataManager;

namespace NWBAWebAPI.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly APIAccountManager _repo;

        public AccountController(APIAccountManager repo)
        {
            _repo = repo;
        }

        // GET: api/accounts
        [HttpGet]
        public IEnumerable<Account> Get()
        {
            return _repo.GetAll();
        }

        // GET api/accounts/1
        [HttpGet("{id}")]
        public Account Get(int id)
        {
            return _repo.Get(id);
        }

        // POST api/accounts
        [HttpPost]
        public void Post([FromBody] Account account)
        {
            _repo.Add(account);
        }

        // PUT api/accounts
        [HttpPut]
        public void Put([FromBody] Account account)
        {
            _repo.Update(account.CustomerID, account);
        }

        // DELETE api/accounts/1
        [HttpDelete("{id}")]
        public long Delete(int id)
        {
            return _repo.Delete(id);
        }
    }
}
