using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AppVentasWeb.Helper
{
    public class BlobHelper_Nuevo : IBlobHelper
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobHelper_Nuevo(IConfiguration configuration)
        {
            try
            {
                string keys = configuration["Blob:ConnectionString"];
                _blobServiceClient = new BlobServiceClient(keys);
            }
            catch { }
      
        }

        public async Task DeleteBlobAsync(Guid id, string containerName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient($"{id}");
            await blobClient.DeleteAsync();
        }

        public async Task<Guid> UploadBlobAsync(IFormFile file, string containerName)
        {
            using Stream stream = file.OpenReadStream();
            return await UploadBlobAsync(stream, containerName);
        }

        public async Task<Guid> UploadBlobAsync(byte[] file, string containerName)
        {
            using MemoryStream stream = new MemoryStream(file);
            return await UploadBlobAsync(stream, containerName);
        }

        public async Task<Guid> UploadBlobAsync(string image, string containerName)
        {
            using Stream stream = File.OpenRead(image);
            return await UploadBlobAsync(stream, containerName);
        }

        private async Task<Guid> UploadBlobAsync(Stream stream, string containerName)
        {
            Guid name = Guid.NewGuid();
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient($"{name}");
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = "application/octet-stream" });
            return name;
        }
    }
}