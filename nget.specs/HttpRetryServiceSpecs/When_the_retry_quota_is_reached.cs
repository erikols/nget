using System;
using System.Net;
using Machine.Specifications;
using Moq;
using nget.core.Web;
using It = Machine.Specifications.It;

namespace nget.specs.HttpRetryServiceSpecs
{
    [Subject(typeof (HttpRetryService))]
    public class When_the_retry_quota_is_reached : With_a_retry_and_delay_service
    {
        Establish context = () =>
            {
                actor = () =>
                    {
                        timesInvoked++;
                        throw new WebException("Test Simulated Error",
                                               WebExceptionStatus.ConnectionClosed);
                    };
            };

        Because of = () => exception = Catch.Exception(() => ClassUnderTest.WithRetry(actor));

        It Should_rethrow_the_final_exception = () => exception.ShouldBeOfType<WebException>();

        It Should_call_the_delay_service_each_time_but_the_last =
            () =>
            mockDelayService.Verify(x => x.Delay(Moq.It.IsAny<int>()), Times.Exactly(HttpRetryService.MaxAttempts - 1));

        It Should_call_the_DelayService_once_with_one_level_of_backoff_delay =
            () =>
            mockDelayService.Verify(x => x.Delay(HttpRetryService.MillisecondsOfDelayBackoffPerFailedAttempt),
                                    Times.Once());

        It Should_call_the_DelayService_once_with_two_levels_of_backoff_delay =
            () =>
            mockDelayService.Verify(x => x.Delay(2*HttpRetryService.MillisecondsOfDelayBackoffPerFailedAttempt),
                                    Times.Once());

        It Should_call_the_DelayService_once_with_three_levels_of_backoff_delay =
            () =>
            mockDelayService.Verify(x => x.Delay(3*HttpRetryService.MillisecondsOfDelayBackoffPerFailedAttempt),
                                    Times.Once());

        It Should_call_the_DelayService_once_with_four_levels_of_backoff_delay =
            () =>
            mockDelayService.Verify(x => x.Delay(4*HttpRetryService.MillisecondsOfDelayBackoffPerFailedAttempt),
                                    Times.Once());

        It Should_invoke_the_action_MaxAttempts_time = () => timesInvoked.ShouldEqual(HttpRetryService.MaxAttempts);

        static Exception exception;
        static int timesInvoked;
    }
}