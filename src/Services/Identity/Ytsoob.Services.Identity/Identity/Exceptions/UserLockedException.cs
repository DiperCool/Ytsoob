using System.Net;
using BuildingBlocks.Core.Exception.Types;

namespace Ytsoob.Services.Identity.Identity.Exceptions;

public class UserLockedException : AppException
{
    public UserLockedException(string userId)
        : base($"userId '{userId}' has been locked.", HttpStatusCode.Forbidden) { }
}
