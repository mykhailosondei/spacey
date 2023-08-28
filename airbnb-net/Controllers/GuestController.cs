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
    public class GuestController : ControllerBase
    {
        private readonly GuestService _guestService;

        public GuestController(GuestService guestService)
        {
            _guestService = guestService;
        }
        // GET: api/Guest
        [HttpGet]
        public async Task<List<GuestModel>> GetAllGuests()
        {
            var result = await _guestService.GetAllGuestsAsync();
            return result;
        }

        // GET: api/Guest/5
        [HttpGet("{id}")]
        public async Task<GuestModel> GetGuest(string id)
        {
            return await _guestService.GetGuestAsync(id);
        }

        // POST: api/Guest
        [HttpPost]
        public async void PostGuest([FromBody] GuestModel guest)
        {
            await _guestService.CreateGuestAsync(guest);
        }

        // PUT: api/Guest/5
        [HttpPut("{id}")]
        public async void PutGuest(string id, [FromBody] GuestModel guest)
        {
            await _guestService.UpdateGuestAsync(id, guest);
        }

        // DELETE: api/Guest/5
        [HttpDelete("{id}")]
        public async void DeleteGuest(string id)
        {
            await _guestService.DeleteGuestAsync(id);
        }
    }
}
