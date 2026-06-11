using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Providers;

public class AzureBlobStorageProvider : IBlobStorageProvider
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IConfiguration _configuration;
    private readonly string _containerName = "attachments";

    public AzureBlobStorageProvider(BlobServiceClient blobServiceClient, IConfiguration configuration)
    {
        _blobServiceClient = blobServiceClient;
        _configuration = configuration;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        if (UsesAzurite())
        {
            await containerClient.CreateIfNotExistsAsync();
        }
        else
        {
            await containerClient.CreateIfNotExistsAsync();
            await containerClient.SetAccessPolicyAsync(PublicAccessType.Blob);
        }

        var blobClient = containerClient.GetBlobClient(fileName);

        var blobHttpHeaders = new BlobHttpHeaders
        {
            ContentType = contentType
        };

        await blobClient.UploadAsync(fileStream, new BlobUploadOptions
        {
            HttpHeaders = blobHttpHeaders
        });

        return ToPublicBlobUrl(blobClient.Uri.ToString());
    }

    public async Task<Stream> GetFileStreamAsync(string blobUrl)
    {
        var blobName = GetBlobNameFromUrl(blobUrl);
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        if (!await blobClient.ExistsAsync())
            throw new FileNotFoundException("Файл не знайдено у хмарному сховищі", blobUrl);

        var response = await blobClient.DownloadStreamingAsync();
        return response.Value.Content;
    }

    private bool UsesAzurite()
    {
        var connectionString = _configuration.GetConnectionString("AzureBlobStorage");
        return string.Equals(connectionString, "UseDevelopmentStorage=true", StringComparison.OrdinalIgnoreCase)
            || connectionString?.Contains("devstoreaccount1", StringComparison.OrdinalIgnoreCase) == true;
    }

    private string ToPublicBlobUrl(string blobUri)
    {
        var publicBase = _configuration["AzureBlobStorage:PublicBlobBaseUrl"];
        if (string.IsNullOrWhiteSpace(publicBase))
            return blobUri;

        var internalBase = _blobServiceClient.Uri.ToString().TrimEnd('/');
        return blobUri.Replace(internalBase, publicBase.TrimEnd('/'), StringComparison.OrdinalIgnoreCase);
    }

    private string GetBlobNameFromUrl(string blobUrl)
    {
        var uri = new Uri(blobUrl);
        var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var containerIndex = Array.FindIndex(
            segments,
            segment => segment.Equals(_containerName, StringComparison.OrdinalIgnoreCase));

        if (containerIndex < 0 || containerIndex >= segments.Length - 1)
            throw new ArgumentException("Invalid blob URL", nameof(blobUrl));

        return string.Join('/', segments.Skip(containerIndex + 1));
    }
}
