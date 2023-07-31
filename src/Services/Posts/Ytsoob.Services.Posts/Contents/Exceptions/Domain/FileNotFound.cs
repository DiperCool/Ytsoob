using System.Net;
using BuildingBlocks.Core.Domain.Exceptions;

namespace Ytsoob.Services.Posts.Posts.Exception;

public class FileNotFound : DomainException
{
    public FileNotFound(string url, HttpStatusCode statusCode = HttpStatusCode.NotFound) : base($"File with url {url} not found", statusCode)
    {
    }
}
