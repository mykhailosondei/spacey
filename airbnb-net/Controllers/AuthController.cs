using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using airbnb_net.Controllers.Abstract;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.DataQueryAccess;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Jwt;
using ApplicationLogic.RoleLogic;
using ApplicationLogic.Services;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : InternalControllerBase
    {
        private readonly AuthService _authService;
        private readonly IUserIdGetter _userIdGetter;
        private readonly IRoleGetter _roleGetter;
        
        public AuthController(ILogger<InternalControllerBase> logger, IMapper mapper, JwtFactory jwtFactory, IUserCommandAccess userCommandAccess, IUserQueryRepository userQueryRepository, AuthService authService, IUserIdGetter userIdGetter, IRoleGetter roleGetter) : base(logger, mapper)
        {
            _authService = authService;
            _userIdGetter = userIdGetter;
            _roleGetter = roleGetter;
        }
        
        // POST: api/Auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<AuthUser> Login([FromBody] LoginUserDTO loginUserDTO)
        {
            return await _authService.Authorize(loginUserDTO);
        }

        [HttpPost("switch-role-to-host/{toHost:bool}")]
        [Authorize(Roles = "User, Host")]
        public async Task<AuthUser> SwitchRole(bool toHost)
        {
            var userId = _userIdGetter.UserId;
            return await _authService.SwitchRole(userId, toHost);
        }
        
        [HttpGet("is-in-role/{role}")]
        public bool IsInRole(string role)
        {
            return _roleGetter.IsInRole(role);
        }
        
        // POST: api/Auth/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<AuthUser> Register([FromBody] RegisterUserDTO registerUserDTO)
        {
            return await _authService.Register(registerUserDTO);
        }
        
        //POST: api/Auth/isEmailTaken
        [HttpPost("isEmailTaken")]
        [AllowAnonymous]
        public async Task<bool> IsEmailTaken([FromBody] string email)
        {
            return await _authService.IsEmailTaken(email);
        }
    }
}
