using ApplicationLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutocompleteController : ControllerBase
    {
        private readonly IAutocompleteService _autocompleteService;

        public AutocompleteController(IAutocompleteService autocompleteService)
        {
            _autocompleteService = autocompleteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAutocomplete(string query, int limit = 10)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest();
            }

            var result = await _autocompleteService.GetAutocomplete(query, limit);
            return Ok(result);
        }
        
    }
}
