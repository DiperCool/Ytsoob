using BlobStorage.Policies;
using Microsoft.AspNetCore.Http;

namespace BlobStorage;

public interface IMinioService
{
    Task<string?> AddItemAsync(
        string bucket,
        IFormFile item,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<string?>> AddItemsAsync(
        string bucket,
        IEnumerable<IFormFile> items,
        CancellationToken cancellationToken = default
    );

    Task<string?> CreateBucketIfNotExistsAsync(
        string bucket,
        IBucketPolicy? policy,
        CancellationToken cancellationToken = default
    );

    Task<bool> DeleteItemAsync(string bucket, string item, CancellationToken cancellationToken = default);
    Task<bool> DeleteItemsAsync(string bucket, IEnumerable<string> items, CancellationToken cancellationToken = default);
}
