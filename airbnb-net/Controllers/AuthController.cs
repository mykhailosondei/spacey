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
using ApplicationLogic.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : InternalControllerBase
    {
        private readonly JwtFactory _jwtFactory;
        private readonly IUserCommandAccess _userCommandAccess;
        private readonly IUserQueryRepository _userQueryRepository;
        private readonly AuthService _authService;
        
        public AuthController(ILogger<InternalControllerBase> logger, IMapper mapper, JwtFactory jwtFactory, IUserCommandAccess userCommandAccess, IUserQueryRepository userQueryRepository, AuthService authService) : base(logger, mapper)
        {
            _jwtFactory = jwtFactory;
            _userCommandAccess = userCommandAccess;
            _userQueryRepository = userQueryRepository;
            _authService = authService;
        }
        
        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<AuthUser> Login([FromBody] LoginUserDTO loginUserDTO)
        {
            return await _authService.Authorize(loginUserDTO);
        }
        
        // POST: api/Auth/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<AuthUser> Register([FromBody] RegisterUserDTO registerUserDTO)
        {
            return await _authService.Register(registerUserDTO);
        }
    }
}
