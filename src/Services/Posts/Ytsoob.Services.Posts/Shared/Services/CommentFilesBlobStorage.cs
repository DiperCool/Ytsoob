using BlobStorage;
using BlobStorage.Policies;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Shared.Services;

public class CommentFilesBlobStorage : ICommentFilesBlobStorage
{
    private IMinioService _minioService;
    private const string Bucket = "comments";

    public CommentFilesBlobStorage(IMinioService minioService)
    {
        _minioService = minioService;
    }

    public async Task<IEnumerable<string?>> UploadFilesAsync(
        IEnumerable<IFormFile> files,
        CancellationToken cancellationToken = default
    )
    {
        await _minioService.CreateBucketIfNotExistsAsync(Bucket, new ReadonlyBucketPolicy(), cancellationToken);
        return await _minioService.AddItemsAsync(Bucket, files, cancellationToken);
    }

    public async Task RemoveFilesAsync(IEnumerable<string> files, CancellationToken cancellationToken = default)
    {
        await _minioService.DeleteItemsAsync(Bucket, files, cancellationToken);
    }
}
