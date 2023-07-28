using Microsoft.AspNetCore.Http;

namespace BlobStorage;

public interface IMinioService
{
    Task<string?> CreateReadOnlyBucketAsync(string bucket, string id, IFormFile file, CancellationToken cancellationToken);
    Task<string?> CreateBucketAsync(string bucket, string id, IFormFile file, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(string bucket, string id, CancellationToken cancellationToken = default);
}
