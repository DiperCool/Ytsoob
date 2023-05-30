using System.Globalization;
using System.Security.Claims;
using BuildingBlocks.Abstractions.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.Core.Persistence.EfCore.Interceptors;

// https://khalidabuhakmeh.com/entity-framework-core-5-interceptors
// https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors#savechanges-interception
// Ref: https://www.meziantou.net/entity-framework-core-generate-tracking-columns.htm
public class AuditInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        if (eventData.Context == null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var now = DateTime.Now;
        var userId = GetCurrentUser();
        foreach (var entry in eventData.Context.ChangeTracker.Entries<IHaveAudit>())
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.CurrentValues[nameof(IHaveAudit.LastModified)] = now;
                    entry.CurrentValues[nameof(IHaveAudit.LastModifiedBy)] = userId;
                    break;
                case EntityState.Added:
                    entry.CurrentValues[nameof(IHaveAudit.Created)] = now;
                    entry.CurrentValues[nameof(IHaveAudit.CreatedBy)] = userId;
                    break;
            }
        }

        foreach (var entry in eventData.Context.ChangeTracker.Entries<IHaveCreator>())
        {
            if (entry.State != EntityState.Added) continue;
            entry.CurrentValues[nameof(IHaveCreator.CreatedBy)] = userId;
            entry.CurrentValues[nameof(IHaveCreator.Created)] = now;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private Guid? GetCurrentUser()
    {
        var id = _httpContextAccessor.HttpContext?.User?.FindFirstValue("sub");
        return id == null ? null : Guid.Parse(id, CultureInfo.CurrentCulture);
    }
}
