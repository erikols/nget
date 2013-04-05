using System;
using System.Net;
using Machine.Specifications;
using Moq;
using nget.core.Web;
using It = Machine.Specifications.It;

namespace nget.specs.HttpRetryServiceSpecs
{
    [Subject(typeof (HttpRetryService))]
    public class With_a_single_retriable_error_and_an_action : With_a_retry_and_delay_service
    {
        Establish context = () =>
            {
                action = () =>
                    {
                        if (timesInvoked++ == 0)
                            throw new WebException("Test Simulated Error",
                                                   WebExceptionStatus.ConnectionClosed);
                    };
            };

        Because of = () => ClassUnderTest.WithRetry(action);

        It Should_retry_the_action_once_after_the_failure = () => timesInvoked.ShouldEqual(2);

        It Should_call_the_DelayService_once_with_one_level_of_backoff_delay =
            () =>
            mockDelayService.Verify(x => x.Delay(HttpRetryService.MillisecondsOfDelayBackoffPerFailedAttempt),
                                    Times.Once());

        static int timesInvoked;
        static Action action;
    }
}