using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using airbnb_net.Controllers.Abstract;
using ApplicationCommon.DTOs.Host;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.DataQueryAccess;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Host = ApplicationDAL.Entities.Host;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostController : InternalControllerBase
    {
        private readonly IHostQueryRepository _hostQueryRepository;
        private readonly IHostCommandAccess _hostCommandAccess;
        
        
        public HostController(IHostQueryRepository hostQueryRepository, IHostCommandAccess hostCommandAccess, ILogger<InternalControllerBase> logger, IMapper mapper) 
            : base(logger, mapper)
        {
            _hostQueryRepository = hostQueryRepository;
            _hostCommandAccess = hostCommandAccess;
        }
        
        // GET: api/Host/5
        [HttpGet("{id:guid}")]
        public async Task<HostDTO> Get(Guid id)
        {
            return _mapper.Map<HostDTO>(await _hostQueryRepository.GetHostById(id));
        }
        
        // POST: api/Host
        [HttpPost]
        public async Task<Guid> Post([FromBody] HostCreateDTO hostCreate)
        {
            var hostDTO = _mapper.Map<HostDTO>(hostCreate);
            var host = _mapper.Map<Host>(hostDTO);
            return await _hostCommandAccess.AddHost(host);
        }
        
        // PUT: api/Host/5
        [HttpPut("{id:guid}")]
        public async Task Put(Guid id, [FromBody] HostUpdateDTO hostUpdate)
        {
            var hostDTO = _mapper.Map<HostDTO>(hostUpdate);
            var host = _mapper.Map<Host>(hostDTO);
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
