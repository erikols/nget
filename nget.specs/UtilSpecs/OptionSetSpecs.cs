using System.Linq;
using Machine.Specifications;
using PS.Utilities.Specs;
using nget.core.Utils;

namespace nget.specs.UtilSpecs
{
    [Subject(typeof (OptionSet))]
    public class When_parsing_an_empty_command_line : With_an_automocked<OptionSet>
    {
        Because of = () => result = ClassUnderTest.Parse(new string[] {});

        It should_indicate_a_successful_parse = () => result.ShouldBeTrue();
        It should_do_nothing = () => 0.ShouldEqual(0);
        static bool result;
    }

    [Subject(typeof(OptionSet))]
    public class When_parsing_with_a_required_flag : With_an_automocked<OptionSet>
    {
        Establish context = () => ClassUnderTest.AddOption(option: "arg1", required: true, isFlag: true);

        Because of = () => result = ClassUnderTest.Parse(new[] { "-arg1" });

        It should_indicate_a_successful_parse = () => result.ShouldBeTrue();
        It should_return_the_option_value = () => ClassUnderTest.IsFlagSet("arg1").ShouldBeTrue();
        static bool result;
    }

    [Subject(typeof(OptionSet))]
    public class When_parsing_with_a_required_flag_missing : With_an_automocked<OptionSet>
    {
        Establish context = () => ClassUnderTest.AddOption(option: "arg1", required: true, isFlag: true);

        Because of = () => result = ClassUnderTest.Parse(new[] { "-arg2" });

        It should_NOT_indicate_a_successful_parse = () => result.ShouldBeFalse();
        It should_NOT_indicate_the_flag_is_set = () => ClassUnderTest.IsFlagSet("arg1").ShouldBeFalse();
        It should_contain_an_error = () => ClassUnderTest.Errors.ShouldContain(x => x.Contains("arg2"));
        static bool result;
    }

    [Subject(typeof(OptionSet))]
    public class When_parsing_with_a_positional_argument : With_an_automocked<OptionSet>
    {
        Establish context = () => ClassUnderTest.AddOption(option: "arg1", required: false, isFlag: true);

        Because of = () => result = ClassUnderTest.Parse(new[] { "some_arg" });

        It should_indicate_a_successful_parse = () => result.ShouldBeTrue();
        It should_return_the_position_arg = () => ClassUnderTest.PositionalArgs.First().ShouldEqual("some_arg");
        static bool result;
    }

    [Subject(typeof(OptionSet))]
    public class When_parsing_with_an_arg_with_params : With_an_automocked<OptionSet>
    {
        Establish context = () => ClassUnderTest.AddOption(option: "arg1", required: false, isFlag: true, parms: new[] {"s", "s"});

        Because of = () => result = ClassUnderTest.Parse(new[] { "-arg1", "param1", "param2" });

        It should_indicate_a_successful_parse = () => result.ShouldBeTrue();
        //It should_return_the_position_arg = () => ClassUnderTest.Params("args");
        static bool result;
    }
}