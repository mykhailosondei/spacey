
using ApplicationLogic.CloudStorage;
using Microsoft.AspNetCore.Mvc;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {

        private readonly ICloudStorage _cloudStorage;

        public ImageUploadController(ICloudStorage cloudStorage)
        {
            _cloudStorage = cloudStorage;
        }

        [HttpPost("upload")]
        public async Task<string> UploadImageAsync([FromForm] IFormFile image)
        {
            return await _cloudStorage.UploadImageAsync(image);
        }
        
        [HttpDelete("delete")]
        public async Task DeleteImageAsync([FromBody] string imageUrl)
        {
            await _cloudStorage.DeleteImageAsync(imageUrl);
        }
    }
}
