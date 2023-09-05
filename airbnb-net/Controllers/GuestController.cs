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
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }
        // GET: api/User
        [HttpGet]
        public async Task<List<UserModel>> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();
            return result;
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<UserModel> GetUser(string id)
        {
            return await _userService.GetUserAsync(id);
        }

        // POST: api/User
        [HttpPost]
        public async void PostUser([FromBody] UserModel user)
        {
            await _userService.CreateUserAsync(user);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async void PutUser(string id, [FromBody] UserModel user)
        {
            await _userService.UpdateUserAsync(id, user);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async void DeleteUser(string id)
        {
            await _userService.DeleteUserAsync(id);
        }
    }
}
