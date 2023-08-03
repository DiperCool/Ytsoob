using BlobStorage;
using BlobStorage.Policies;
using Ytsoob.Services.Ytsoobers.Shared.Contracts;

namespace Ytsoob.Services.Ytsoobers.Shared.Services;

public class AvatarStorage : IAvatarStorage
{
    private IMinioService _minioService;
    private const string BucketName = "avatars";

    public AvatarStorage(IMinioService minioService)
    {
        _minioService = minioService;
    }

    public async Task<string?> UploadAvatarAsync(IFormFile file, CancellationToken cancellationToken)
    {
        await _minioService.CreateBucketIfNotExistsAsync(BucketName, new ReadonlyBucketPolicy(), cancellationToken);
        return await _minioService.AddItemAsync(BucketName, file, cancellationToken);
    }
}
