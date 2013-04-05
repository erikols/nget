using System.Net;
using Machine.Specifications;
using Moq;
using nget.core.Web;
using It = Machine.Specifications.It;

namespace nget.specs.HttpRetryServiceSpecs
{
    [Subject(typeof (HttpRetryService))]
    public class With_a_single_retriable_error : With_a_retry_and_delay_service
    {
        Establish context = () =>
            {
                actor = () =>
                    {
                        if (timesInvoked++ == 0)
                            throw new WebException("Test Simulated Error",
                                                   WebExceptionStatus.ConnectionClosed);

                        return done;
                    };
            };

        Because of = () => result = ClassUnderTest.WithRetry(actor);

        It Should_return_the_result_returned_by_the_Func = () => result.ShouldEqual(done);

        It Should_retry_the_func_once_after_the_failure = () => timesInvoked.ShouldEqual(2);

        It Should_call_the_DelayService_once_with_one_level_of_backoff_delay =
            () =>
            mockDelayService.Verify(x => x.Delay(HttpRetryService.MillisecondsOfDelayBackoffPerFailedAttempt),
                                    Times.Once());

        static int timesInvoked;
    }
}