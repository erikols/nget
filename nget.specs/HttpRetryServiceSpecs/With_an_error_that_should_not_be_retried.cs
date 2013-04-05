#region Using directives

using System;
using Machine.Specifications;
using Moq;
using nget.core.Web;
using It = Machine.Specifications.It;

#endregion

namespace nget.specs.HttpRetryServiceSpecs
{
    [Subject(typeof (HttpRetryService))]
    public class With_an_error_that_should_not_be_retried : With_a_retry_and_delay_service
    {
        Establish context = () =>
            {
                actor = () =>
                    {
                        timesInvoked++;
                        throw new InvalidOperationException("Test Simulated Error");
                    };
            };

        Because of = () => exception = Catch.Exception(() => result = ClassUnderTest.WithRetry(actor));

        It Should_rethrow_the_exception = () => exception.ShouldBeOfType<InvalidOperationException>();

        It Should_NOT_call_the_DelayService =
            () => mockDelayService.Verify(x => x.Delay(Moq.It.IsAny<int>()), Times.Never());

        It Should_only_invoke_the_func_once = () => timesInvoked.ShouldEqual(1);

        static Exception exception;
        static int timesInvoked;
    }
}