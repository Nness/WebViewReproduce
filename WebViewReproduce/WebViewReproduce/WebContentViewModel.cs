using System;

namespace WebViewReproduce
{
    public class WebContentViewModel
    {
        public Uri Source { get; }

        public string Title => Source.Host;

        public WebContentViewModel(Uri source)
        {
            Source = source;
        }
    }
}