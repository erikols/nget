using System;

namespace nget.core.Web
{
    public interface IHttpRetryService
    {
        TResult WithRetry<TResult>(Func<TResult> httpAction);
        void WithRetry(Action httpAction);
    }
}