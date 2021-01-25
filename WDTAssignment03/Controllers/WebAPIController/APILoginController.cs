using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Admin.Models;
using NWBAWebAPI.Models.DataManager;

namespace NWBAWebAPI.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class APILoginController : Controller
    {
        private readonly APILoginManager _repo;

        public APILoginController(APILoginManager repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IEnumerable<Login> Get()
        {
            return _repo.GetAll();
        }

        [HttpGet("{id}")]
        public Login Get(string id)
        {
            return _repo.Get(id);
        }

        [HttpPost]
        public void Post([FromBody] Login customer)
        {
            _repo.Add(customer);
        }

        [HttpPut]
        public void Put([FromBody] Login customer)
        {
            _repo.Update(customer.UserID, customer);
        }

        [HttpDelete("{id}")]
        public string Delete(string id)
        {
            return _repo.Delete(id);
        }
    }
}
