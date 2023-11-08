using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using airbnb_net.Controllers.Abstract;
using ApplicationCommon.DTOs.User;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : InternalControllerBase
    {
        public AuthController(ILogger<InternalControllerBase> logger, IMapper mapper) : base(logger, mapper)
        {
            
        }
        
        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<AuthUser> Login([FromBody] LoginUserDTO auth)
        {
            return new AuthUser() { };
        }
        
        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<AuthUser> Register([FromBody] RegisterUserDTO auth)
        {
            return new AuthUser() { };
        }
    }
}
