namespace BuildingBlocks.Security.Jwt;

public interface ICurrentUserService
{
    string? UserId { get; }
    Guid UserIdGuid { get; }
    string Role { get; }
    string? JwtToken { get; }
    bool IsAuthenticated { get; }
}
