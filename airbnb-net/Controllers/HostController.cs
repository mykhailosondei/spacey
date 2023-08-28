using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationDataAccess.DataAccess;
using ApplicationDataAccess.Models;
using ApplicationLogic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostController : ControllerBase
    {
        private readonly HostService _hostService;

        public HostController(HostService hostService)
        {
            _hostService = hostService;
        }
        // GET: api/Host
        [HttpGet]
        public async Task<List<HostModel>> Get()
        {
            return await _hostService.GetAllHosts();
        }

        // GET: api/Host/5
        [HttpGet("{id}")]
        public async Task<HostModel> Get(string id)
        {
            return await _hostService.GetHost(id);
        }

        // POST: api/Host
        [HttpPost]
        public async void Post([FromBody] HostModel model)
        {
            await _hostService.CreateHostAsync(model);
        }

        // PUT: api/Host/5
        [HttpPut("{id}")]
        public async void Put(string id, [FromBody] HostModel model)
        {
            await _hostService.UpdateHostAsync(id, model);
        }

        // DELETE: api/Host/5
        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
            await _hostService.DeleteHostAsync(id);
        }
    }
}
