using System.Collections.Generic;
using System.Text;

namespace nget.core.Utils
{
    public class OptionSet
    {
        readonly Dictionary<string, OptionSetItem> options;
        readonly Dictionary<string, object> found;
        readonly List<string> errors;
        readonly List<string> positionalArgs;

        public OptionSet()
        {
            options = new Dictionary<string, OptionSetItem>();
            found = new Dictionary<string, object>();
            errors = new List<string>();
            positionalArgs = new List<string>();
        }

        public object this[string index]
        {
            get
            {
                return found.ContainsKey(index)
                           ? found[index]
                           : null;
            }
        }

        public IEnumerable<string> PositionalArgs
        {
            get { return positionalArgs; }
        }

        public IEnumerable<string> Errors
        {
            get { return errors; }
        }

        public bool Parse(string[] args)
        {
            var argc = args.Length;
            for (var i = 0; i < argc; i++)
            {
                var a = args[i];
                if (IsArg(a))
                {
                    var argName = a.Substring(1);
                    var argInfo = GetItemData(argName);
                    if (null == argInfo)
                    {
                        errors.Add("Unknown option: " + a);
                    }
                    else
                    {
                        if (argInfo.IsFlag)
                            found.Add(argName, true);
                    }
                }
                else
                {
                    positionalArgs.Add(a);
                }
            }

            return errors.Count == 0;
        }

        OptionSetItem GetItemData(string arg)
        {
            OptionSetItem optionData;
            options.TryGetValue(arg, out optionData);
            return optionData;
        }

        bool IsArg(string s)
        {
            return s.StartsWith("-");
        }

        public void AddOption(string option, bool required, bool isFlag, string[] parms = null)
        {
            options.Add(option,
                        new OptionSetItem
                            {
                                Option = option,
                                Required = required,
                                IsFlag = isFlag,
                                Params = parms
                            });
        }

        public bool IsFlagSet(string flag)
        {
            return found.ContainsKey(flag);
        }

        public string Usage()
        {
            var sb = new StringBuilder();
            sb.Append("Usage:\n");

            foreach (var opt in options)
            {
                var option = opt.Value;
                if (option.IsFlag)
                    sb.AppendFormat("\t-{0} ({1})",
                                    option.Option,
                                    option.Required
                                        ? "required"
                                        : "optional");
            }

            return sb.ToString();
        }

        public IEnumerable<object> Params(string arg)
        {
            return null;
        }
    }

    internal class OptionSetItem
    {
        public string Option { get; set; }
        public bool Required { get; set; }
        public bool IsFlag { get; set; }
        public string[] Params { get; set; }
    }
}