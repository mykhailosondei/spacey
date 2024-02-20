using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ApplicationLogic.CloudStorage;

public class AzureCloudStorage : ICloudStorage
{
    private readonly AzureCloudStorageOptions _options;
    
    public AzureCloudStorage(IOptions<AzureCloudStorageOptions> options)
    {
        _options = options.Value;
    }
    
    public async Task<string> UploadImageAsync(IFormFile image)
    {
        if(image == null) throw new ArgumentNullException(nameof(image));
        if(image.ContentType.ToLower() != "image/jpg" && image.ContentType.ToLower() != "image/jpeg" && image.ContentType.ToLower() != "image/png")
            throw new ArgumentException("Invalid image type");
        var blobServiceClient = new BlobServiceClient(_options.ConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient("images");
        var blobName = Guid.NewGuid() + "." + image.FileName.Split(".").Last();
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(image.OpenReadStream(), true);
        return blobClient.Uri.ToString();
    }
    
    public async Task DeleteImageAsync(string imageUrl)
    {
        if(imageUrl == null) throw new ArgumentNullException(nameof(imageUrl));
        var blobServiceClient = new BlobServiceClient(_options.ConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient("images");
        var blobName = imageUrl.Split("/").Last();
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync();
    }
}