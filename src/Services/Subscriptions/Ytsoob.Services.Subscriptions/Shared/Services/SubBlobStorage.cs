using BlobStorage;
using BlobStorage.Policies;
using Ytsoob.Services.Subscriptions.Subscriptions.Contracts;

namespace Ytsoob.Services.Subscriptions.Shared.Services;

public class SubBlobStorage : ISubBlobStorage
{
    private IMinioService _minioService;
    private const string Bucket = "subscriptions";

    public SubBlobStorage(IMinioService minioService)
    {
        _minioService = minioService;
    }

    public async Task<string?> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        await _minioService.CreateBucketIfNotExistsAsync(Bucket, new ReadonlyBucketPolicy(), cancellationToken);
        return await _minioService.AddItemAsync(Bucket, file, cancellationToken);
    }

    public async Task RemoveFileAsync(string file, CancellationToken cancellationToken = default)
    {
        await _minioService.DeleteItemAsync(Bucket, file, cancellationToken);
    }
}
