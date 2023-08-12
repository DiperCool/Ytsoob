using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Payment.Shared.Data;

namespace Ytsoob.Services.Payment.Shared.Extensions.WebApplicationExtensions;

public static partial class WebApplicationExtensions
{
    public static async Task ApplyDatabaseMigrations(this WebApplication app)
    {
        var configuration = app.Services.GetRequiredService<IConfiguration>();
        if (configuration.GetValue<bool>("PostgresOptions:UseInMemory") == false)
        {
            using var serviceScope = app.Services.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<PaymentDbContext>();

            app.Logger.LogInformation("Updating catalog database...");

            await dbContext.Database.MigrateAsync();

            app.Logger.LogInformation("Updated catalog database");
        }
    }
}
