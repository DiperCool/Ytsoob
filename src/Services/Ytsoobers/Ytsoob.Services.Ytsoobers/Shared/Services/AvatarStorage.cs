using BlobStorage;
using BuildingBlocks.Core.Web.Extenions;
using Microsoft.Extensions.Options;
using Sprache;
using Ytsoob.Services.Ytsoobers.Shared.Contracts;

namespace Ytsoob.Services.Ytsoobers.Shared.Services;

public class AvatarStorage : IAvatarStorage
{
    private IMinioService _minioService;
    private MinioOptions _options;
    private IConfiguration _configuration;
    private const string BucketName = "avatars";
    public AvatarStorage(IMinioService minioService, IConfiguration configuration)
    {
        _minioService = minioService;
        _configuration = configuration;
        _options = _configuration.BindOptions<MinioOptions>();
    }

    public async Task<string> UploadAvatarAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var fileExtension = Path.GetExtension(file.FileName);
        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var fileUrl = $"http://{_options.Uri}/{BucketName}/{uniqueFileName}";
        await _minioService.CreateReadOnlyBucketAsync(BucketName, uniqueFileName, file, cancellationToken);
        return fileUrl;
    }
}
