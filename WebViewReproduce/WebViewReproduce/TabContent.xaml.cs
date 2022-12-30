using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Web.WebView2.Core;

namespace WebViewReproduce
{
    /// <summary>
    /// Interaction logic for TabContent.xaml
    /// </summary>
    public partial class TabContent : UserControl
    {
        private readonly MainWindow _parent;

        public TabContent(CoreWebView2Environment environment, WebContentViewModel viewModel, MainWindow parent)
        {
            _parent = parent;
            DataContext = viewModel;

            InitializeComponent();

            InitializeWebViewAsync(environment);
        }

        private async void InitializeWebViewAsync(CoreWebView2Environment environment)
        {
            WebContent.CoreWebView2InitializationCompleted += WebViewInitializationCompleted;

            await WebContent.EnsureCoreWebView2Async(environment);
        }

        private void WebViewInitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            CoreWebView2? webView = WebContent.CoreWebView2;
            if (webView == null) {
                return;
            }

            webView.NewWindowRequested += NewWindowRequested;
            webView.ProcessFailed += ProcessFailed;

            if (DataContext is WebContentViewModel model) {
                webView.Navigate(model.Source.ToString());
            }
        }

        private void ProcessFailed(object? sender, CoreWebView2ProcessFailedEventArgs e)
        {
            Console.WriteLine(e.Reason);
        }

        private void NewWindowRequested(object? sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            _parent.NewTabRequested(this, sender, e);
        }
    }
}
