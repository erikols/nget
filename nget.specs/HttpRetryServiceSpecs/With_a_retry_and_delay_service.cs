using System;
using Machine.Specifications;
using Moq;
using PS.Utilities.Specs;
using nget.core.Utils;
using nget.core.Web;

namespace nget.specs.HttpRetryServiceSpecs
{
    public class With_a_retry_and_delay_service : With_an_automocked<HttpRetryService>
    {
        Establish context = () => { mockDelayService = GetTestDouble<IDelayService>(); };

        protected static Mock<IDelayService> mockDelayService;
        protected const string done = "done";
        protected static string result;
        protected static Func<string> actor;
    }
}