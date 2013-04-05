using System;

namespace nget.core.Utils
{
    public class UrlExpander
    {
        public string ExpandUrl(string inputUrl)
        {
            return Uri.IsWellFormedUriString(inputUrl, UriKind.Absolute)
                       ? inputUrl
                       : string.Format("http://{0}", inputUrl);
        }
    }
}