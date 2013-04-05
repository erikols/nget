using Machine.Specifications;
using Moq;
using nget.core.Web;
using It = Machine.Specifications.It;

namespace nget.specs.HttpRetryServiceSpecs
{
    [Subject(typeof (HttpRetryService))]
    public class With_a_successful_func : With_a_retry_and_delay_service
    {
        Establish context = () =>
            {
                actor = () =>
                    {
                        timesInvoked++;
                        return done;
                    };
            };

        Because of = () => result = ClassUnderTest.WithRetry(actor);

        It Should_return_the_result_returned_by_the_Func = () => result.ShouldEqual(done);

        It Should_only_invoke_the_action_once = () => timesInvoked.ShouldEqual(1);

        It Should_not_call_the_DelayService =
            () => mockDelayService.Verify(x => x.Delay(Moq.It.IsAny<int>()), Times.Never());

        static int timesInvoked;
    }
}