namespace Infrastructure.Interfaces;
public interface IBlobStorageProvider
{
    public Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);
    public Task<Stream> GetFileStreamAsync(string blobUrl);
}
