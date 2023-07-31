namespace Ytsoob.Services.Ytsoobers.Shared.Contracts;

public interface IAvatarStorage
{
    public Task<string?> UploadAvatarAsync(IFormFile file, CancellationToken cancellationToken);
}
