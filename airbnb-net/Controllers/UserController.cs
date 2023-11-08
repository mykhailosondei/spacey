using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using airbnb_net.Controllers.Abstract;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.DataQueryAccess;
using ApplicationDAL.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : InternalControllerBase
    {
        private readonly UserQueryRepository _userQueryRepository;
        private readonly UserCommandAccess _userCommandAccess;
        
        public UserController(UserQueryRepository userQueryRepository, UserCommandAccess userCommandAccess, ILogger<InternalControllerBase> logger, IMapper mapper)
        : base(logger, mapper)
        {
            _userQueryRepository = userQueryRepository;
            _userCommandAccess = userCommandAccess;
        }
        
        // GET: api/User
        [HttpGet]
        public async Task<IEnumerable<UserDTO>> Get()
        {
            return _mapper.Map<IEnumerable<UserDTO>>(await _userQueryRepository.GetAllUsers());
        }

        // GET: api/User/5
        [HttpGet("{id:guid}")]
        public async Task<UserDTO> Get(Guid id)
        {
            return _mapper.Map<UserDTO>(await _userQueryRepository.GetUserById(id));
        }

        // PUT: api/User/5
        [HttpPut("{id:guid}")]
        public async Task Put(Guid id, [FromBody] UserUpdateDTO userUpdate)
        {
            var userDTO = _mapper.Map<User>(userUpdate);
            var user = _mapper.Map<User>(userDTO);
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
