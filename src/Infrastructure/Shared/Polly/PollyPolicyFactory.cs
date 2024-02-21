using Polly.Retry;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Timeout;
using Polly.Wrap;

namespace Designly.Shared.Polly
{
    public static class PollyPolicyFactory
    {
        private const int defaultRetryCount = 5;
        private const int defaultTimeout = 30;

        public static AsyncRetryPolicy RetryWithJitterAsync(int retryCount = defaultRetryCount)
        {
            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: retryCount);

            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(delay);

            return retryPolicy;
        }

        public static AsyncRetryPolicy NetworkRetryAsync(int retryCount, TimeSpan initialDelay)
        {
            return Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(
                    retryCount,
                    retryAttempt => initialDelay * Math.Pow(2, retryAttempt),
                    (exception, timeSpan, retryCount, context) =>
                    {
                    }
                );
        }

        public static AsyncTimeoutPolicy TimeoutAsync(int timeoutInSeconds = defaultTimeout)
        {
            var policy = Policy.TimeoutAsync(timeoutInSeconds, TimeoutStrategy.Pessimistic,
              onTimeoutAsync: (context, timespan, _, _) =>
              {
                  // TODO: Log here?
                  return Task.CompletedTask;
              });

            return policy;
        }

        public static AsyncPolicyWrap WrappedAsyncPolicies()
        {
            var wrappedPolicy = Policy.WrapAsync(
                RetryWithJitterAsync(),
                TimeoutAsync()
                );

            return wrappedPolicy;
        }

        public static AsyncPolicyWrap WrappedNetworkRetries()
        {
            var wrappedPolicy = Policy.WrapAsync(
                NetworkRetryAsync(defaultRetryCount, TimeSpan.FromSeconds(1)),
                TimeoutAsync()
                                              );
            return wrappedPolicy;
        }
    }
}
