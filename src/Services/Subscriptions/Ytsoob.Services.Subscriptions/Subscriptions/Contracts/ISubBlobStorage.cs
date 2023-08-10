namespace Ytsoob.Services.Subscriptions.Subscriptions.Contracts;

public interface ISubBlobStorage
{
    public Task<string?> UploadFile(IFormFile formFile);
    public Task RemoveFile(string file);
}
