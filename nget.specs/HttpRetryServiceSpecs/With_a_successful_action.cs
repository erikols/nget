using System;
using Machine.Specifications;
using Moq;
using nget.core.Web;
using It = Machine.Specifications.It;

namespace nget.specs.HttpRetryServiceSpecs
{
    [Subject(typeof (HttpRetryService))]
    public class With_a_successful_action : With_a_retry_and_delay_service
    {
        Establish context = () => { action = () => { timesInvoked++; }; };

        Because of = () => ClassUnderTest.WithRetry(action);

        It Should_only_invoke_the_action_once = () => timesInvoked.ShouldEqual(1);

        It Should_not_call_the_DelayService =
            () => mockDelayService.Verify(x => x.Delay(Moq.It.IsAny<int>()), Times.Never());

        static int timesInvoked;
        static Action action;
    }
}