using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Ytsoobers.Profiles.ValueObjects;
using Ytsoob.Services.Ytsoobers.Shared.Contracts;
using Ytsoob.Services.Ytsoobers.Ytsoobers.Exceptions;
using Ytsoob.Services.Ytsoobers.Ytsoobers.Models;

namespace Ytsoob.Services.Ytsoobers.Profiles.Features.UpdatingProfile.v1.UpdateProfile;

public record UpdateProfile(string FirstName, string LastName, IFormFile? Avatar) : ITxUpdateCommand<Unit>;

public class UpdateProfileValidator : AbstractValidator<UpdateProfile>
{
    public UpdateProfileValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(15);
        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(15);
    }
}

public class UpdateProfileHandler : ICommandHandler<UpdateProfile, Unit>
{
    private IYtsoobersDbContext _ytsoobersDbContext;
    private ICurrentUserService _currentUserService;
    private ILogger<UpdateProfileHandler> _logger;
    private IAvatarStorage _avatarStorage;
    public UpdateProfileHandler(IYtsoobersDbContext ytsoobersDbContext, ICurrentUserService currentUserService, ILogger<UpdateProfileHandler> logger, IAvatarStorage avatarStorage)
    {
        _ytsoobersDbContext = ytsoobersDbContext;
        _currentUserService = currentUserService;
        _logger = logger;
        _avatarStorage = avatarStorage;
    }

    public async Task<Unit> Handle(UpdateProfile request, CancellationToken cancellationToken)
    {
        Ytsoober? ytsoober = await _ytsoobersDbContext.Ytsoobers
            .Include(x => x.Profile)
            .FirstOrDefaultAsync(x => x.Id == _currentUserService.YtsooberId, cancellationToken: cancellationToken);
        if (ytsoober == null)
        {
            _logger.LogCritical("Ytsoober with Id = {YtsooberId} not found", _currentUserService.YtsooberId);
            throw new YtsooberNotFoundException(_currentUserService.YtsooberId);
        }

        string? avatar = request.Avatar != null ? await _avatarStorage.UploadAvatarAsync(request.Avatar, cancellationToken) : null;
        ytsoober.UpdateProfile(FirstName.Of(request.FirstName), LastName.Of(request.LastName), avatar);
        _ytsoobersDbContext.Ytsoobers.Update(ytsoober);
        await _ytsoobersDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
