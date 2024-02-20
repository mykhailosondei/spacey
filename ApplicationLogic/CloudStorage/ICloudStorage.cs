using Microsoft.AspNetCore.Http;

namespace ApplicationLogic.CloudStorage;

public interface ICloudStorage
{
    Task<string> UploadImageAsync(IFormFile image);
    Task DeleteImageAsync(string imageUrl);
}