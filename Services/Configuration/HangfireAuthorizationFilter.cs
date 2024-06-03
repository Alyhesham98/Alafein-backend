using Hangfire.Dashboard;
using System.Diagnostics.CodeAnalysis;

namespace Services.Configuration
{
    internal sealed class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            return true;
        }
    }
}
