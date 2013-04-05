#region Using directives

using System;
using System.IO;
using System.Net;
using nget.core.Utils;

#endregion

namespace nget.core.Web
{
    public class HttpRetryService : IHttpRetryService
    {
        readonly IDelayService delayService;
        public const int MaxAttempts = 5;
        public const int MillisecondsOfDelayBackoffPerFailedAttempt = 250;

        public HttpRetryService(IDelayService delayService)
        {
            this.delayService = delayService;
        }

        public TResult WithRetry<TResult>(Func<TResult> httpAction)
        {
            var attemptsRemaining = MaxAttempts;
            var result = default(TResult);

            while (true)
            {
                if (attemptsRemaining-- <= 0)
                    break;
                try
                {
                    return httpAction();
                }
                catch (Exception exception)
                {
                    if (ShouldRetry(exception, attemptsRemaining))
                        continue;
                    throw;
                }
            }

            return result;
        }

        public void WithRetry(Action httpAction)
        {
            var attemptsRemaining = MaxAttempts;

            while (true)
            {
                if (attemptsRemaining-- <= 0)
                    break;
                try
                {
                    httpAction();
                    return;
                }
                catch (Exception exception)
                {
                    if (ShouldRetry(exception, attemptsRemaining))
                        continue;
                    throw;
                }
            }
        }

        bool ShouldRetry(Exception exception, int attemptsRemaining)
        {
            if (attemptsRemaining == 0)
                return false;

            var shouldContinue = ShouldRetryDueTo(exception);

            if (shouldContinue)
            {
                var attempts = MaxAttempts - attemptsRemaining;
                var delayInMs = MillisecondsOfDelayBackoffPerFailedAttempt*attempts;
                delayService.Delay(delayInMs);
                return true;
            }

            return false;
        }

        static bool ShouldRetryDueTo(Exception exception)
        {
            var webException = exception as WebException;
            if (null != webException)
            {
                if (webException.Status == WebExceptionStatus.ConnectionClosed)
                    return true;

                // retry some HTTP 500 errors
                var errorResponse = webException.Response as HttpWebResponse;
                if (null != errorResponse)
                {
                    var statusCode = errorResponse.StatusCode;

                    return (HttpStatusCode.InternalServerError == statusCode ||
                            HttpStatusCode.ServiceUnavailable == statusCode ||
                            HttpStatusCode.GatewayTimeout == statusCode);
                }
            }

            return exception is IOException;
        }
    }
}