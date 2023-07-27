using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Ytsoobers.Profiles.ValueObjects;
using Ytsoob.Services.Ytsoobers.Shared.Contracts;
using Ytsoob.Services.Ytsoobers.Ytsoobers.Exceptions;
using Ytsoob.Services.Ytsoobers.Ytsoobers.Models;

namespace Ytsoob.Services.Ytsoobers.Profiles.Features.UpdatingProfile.v1.UpdateProfile;

public class UpdateProfile : ITxUpdateCommand<Unit>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
}

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

    public UpdateProfileHandler(IYtsoobersDbContext ytsoobersDbContext, ICurrentUserService currentUserService, ILogger<UpdateProfileHandler> logger)
    {
        _ytsoobersDbContext = ytsoobersDbContext;
        _currentUserService = currentUserService;
        _logger = logger;
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

        ytsoober.UpdateProfile(FirstName.Of(request.FirstName), LastName.Of(request.LastName), request.Avatar);
        _ytsoobersDbContext.Ytsoobers.Update(ytsoober);
        await _ytsoobersDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
