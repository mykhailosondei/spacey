using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.DataQueryAccess;
using ApplicationDAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserQueryRepository _userQueryRepository;
        private readonly UserCommandAccess _userCommandAccess;
        
        public UserController(UserQueryRepository userQueryRepository, UserCommandAccess userCommandAccess)
        {
            _userQueryRepository = userQueryRepository;
            _userCommandAccess = userCommandAccess;
        }
        
        // GET: api/User
        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            return await _userQueryRepository.GetAllUsers();
        }

        // GET: api/User/5
        [HttpGet("{id:guid}")]
        public async Task<User> Get(Guid id)
        {
            return await _userQueryRepository.GetUserById(id);
        }

        // POST: api/User
        [HttpPost]
        public async Task<Guid> Post([FromBody] User user)
        {
            return await _userCommandAccess.AddUser(user);
        }

        // PUT: api/User/5
        [HttpPut("{id:guid}")]
        public async Task Put(Guid id, [FromBody] User user)
        {
            await _userCommandAccess.UpdateUser(id, user);
        }

        // DELETE: api/User/5
        [HttpDelete("{id:guid}")]
        public async Task Delete(Guid id)
        {
            await _userCommandAccess.DeleteUser(id);
        }
    }
}
