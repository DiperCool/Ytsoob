using System.Net;
using BuildingBlocks.Core.Exception.Types;
using Ytsoob.Services.Identity.Shared.Models;

namespace Ytsoob.Services.Identity.Identity.Exceptions;

public class RefreshTokenNotFoundException : AppException
{
    public RefreshTokenNotFoundException(RefreshToken? refreshToken)
        : base("Refresh token not found.", HttpStatusCode.NotFound) { }
}
