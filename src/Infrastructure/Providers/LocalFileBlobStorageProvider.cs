using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Providers;

public class LocalFileBlobStorageProvider : IBlobStorageProvider
{
    private readonly string _storagePath;
    private readonly string _publicBaseUrl;

    public LocalFileBlobStorageProvider(IConfiguration configuration)
    {
        _storagePath = configuration["BlobStorage:LocalPath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "blob-storage");
        _publicBaseUrl = configuration["AzureBlobStorage:PublicBlobBaseUrl"] ?? "http://localhost:5001/blob-storage";
        Directory.CreateDirectory(_storagePath);
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        var safeName = $"{Guid.NewGuid()}-{Path.GetFileName(fileName)}";
        var path = Path.Combine(_storagePath, safeName);

        await using (var output = File.Create(path))
        {
            await fileStream.CopyToAsync(output);
        }

        return $"{_publicBaseUrl.TrimEnd('/')}/{safeName}";
    }

    public Task<Stream> GetFileStreamAsync(string blobUrl)
    {
        var fileName = Path.GetFileName(new Uri(blobUrl).AbsolutePath);
        var path = Path.Combine(_storagePath, fileName);

        if (!File.Exists(path))
            throw new FileNotFoundException("File not found in local blob storage", blobUrl);

        return Task.FromResult<Stream>(File.OpenRead(path));
    }
}
