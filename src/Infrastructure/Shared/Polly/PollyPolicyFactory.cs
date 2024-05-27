using Polly.Retry;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Timeout;
using Polly.Wrap;
using Microsoft.Extensions.Logging;
using System;

namespace Designly.Shared.Polly
{
    public static class PollyPolicyFactory
    {
        private const int defaultRetryCount = 5;
        private const int defaultTimeout = 30;

        public static AsyncRetryPolicy RetryWithJitterAsync(ILogger logger, int retryCount = defaultRetryCount)
        {
            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: retryCount);

            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(delay, onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    logger.LogWarning("Retry {RetryCount} after {TimeSpan} due to {Exception}.", retryCount, timeSpan, exception.Message);
                });

            return retryPolicy;
        }

        public static AsyncRetryPolicy NetworkRetryAsync(ILogger logger, int retryCount, TimeSpan initialDelay)
        {
            return Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(
                    retryCount,
                    retryAttempt => initialDelay * Math.Pow(2, retryAttempt),
                    onRetry: (exception, timeSpan, retryCount, context) => 
                    {
                        logger.LogWarning("Network retry {RetryCount} after {TimeSpan} due to {Exception}.", retryCount, timeSpan, exception.Message);
                    }
                );
        }

        public static AsyncTimeoutPolicy TimeoutAsync(ILogger logger, int timeoutInSeconds = defaultTimeout)
        {
            var policy = Policy.TimeoutAsync(timeoutInSeconds, TimeoutStrategy.Pessimistic,
               onTimeoutAsync: async (context, timespan, task, exception) =>
               {
                   logger.LogError("Operation timed out after {Timeout} seconds. Context: {Context}. Exception: {Exception}", timespan.TotalSeconds, context, exception?.Message);
                   await Task.CompletedTask;
               });

            return policy;
        }

        public static AsyncPolicyWrap WrappedAsyncPolicies(ILogger logger)
        {
            var wrappedPolicy = Policy.WrapAsync(
                RetryWithJitterAsync(logger),
                TimeoutAsync(logger));

            return wrappedPolicy;
        }

        public static AsyncPolicyWrap WrappedNetworkRetries(ILogger logger)
        {
            var wrappedPolicy = Policy.WrapAsync(
                NetworkRetryAsync(logger, defaultRetryCount, TimeSpan.FromSeconds(1)),
                TimeoutAsync(logger));
            return wrappedPolicy;
        }
    }
}
