using Azure.Storage.Blobs;
using Infrastructure.Interfaces;

namespace Infrastructure.Providers;
public class AzureBlobStorageProvider : IBlobStorageProvider
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName = "attachments";

    public AzureBlobStorageProvider(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync();
        await containerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

        var blobClient = containerClient.GetBlobClient(fileName);

        var blobHttpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders
        {
            ContentType = contentType
        };

        await blobClient.UploadAsync(fileStream, new Azure.Storage.Blobs.Models.BlobUploadOptions
        {
            HttpHeaders = blobHttpHeaders
        });

        return blobClient.Uri.ToString();
    }

    public async Task<Stream> GetFileStreamAsync(string blobUrl)
    {
        var blobClient = new BlobClient(new Uri(blobUrl));

        if (!await blobClient.ExistsAsync())
            throw new FileNotFoundException("Файл не знайдено у хмарному сховищі", blobUrl);

        var response = await blobClient.DownloadStreamingAsync();
        return response.Value.Content;
    }

}
