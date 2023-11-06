using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.DataQueryAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Host = ApplicationDAL.Entities.Host;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostController : ControllerBase
    {
        private readonly HostQueryRepository _hostQueryRepository;
        private readonly HostCommandAccess _hostCommandAccess;
        
        public HostController(HostQueryRepository hostQueryRepository, HostCommandAccess hostCommandAccess)
        {
            _hostQueryRepository = hostQueryRepository;
            _hostCommandAccess = hostCommandAccess;
        }
        
        // GET: api/Host/5
        [HttpGet("{id:guid}")]
        public async Task<Host> Get(Guid id)
        {
            return await _hostQueryRepository.GetHostById(id);
        }
        
        // POST: api/Host
        [HttpPost]
        public async Task<Guid> Post([FromBody] Host host)
        {
            return await _hostCommandAccess.AddHost(host);
        }
        
        // PUT: api/Host/5
        [HttpPut("{id:guid}")]
        public async Task Put(Guid id, [FromBody] Host host)
        {
            await _hostCommandAccess.UpdateHost(id, host);
        }
        
        // DELETE: api/Host/5
        [HttpDelete("{id:guid}")]
        public async Task Delete(Guid id)
        {
            await _hostCommandAccess.DeleteHost(id);
        }
    }
}
