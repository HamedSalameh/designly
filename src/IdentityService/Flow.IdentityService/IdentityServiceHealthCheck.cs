using Flow.SharedKernel.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Flow.IdentityService
{
    internal class IdentityServiceHealthCheck : IHealthCheck
    {
        private readonly IIdentityService _identityService;

        public IdentityServiceHealthCheck(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (_identityService is null)
            {
                return Task.FromResult( new HealthCheckResult(context.Registration.FailureStatus, "IdentityService wsa not injected") );
            }

            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}
