using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Web.WebView2.Core;

namespace WebViewReproduce
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CoreWebView2Environment? _environment;

        public MainWindow()
        {
            InitializeComponent();

            InitializeWebViewAsync();
        }

        private async void InitializeWebViewAsync()
        {
            var options = new CoreWebView2EnvironmentOptions {
                AllowSingleSignOnUsingOSPrimaryAccount = true
            };

            string localData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string path = Path.Combine(localData, "WebView Test");

            CoreWebView2Environment environment = _environment = await CoreWebView2Environment
                .CreateAsync(null, path, options);

            AddNewTab(environment, new Uri("https://www.google.com"));
        }

        public void NewTabRequested(TabContent tab, object? sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            CoreWebView2Environment? environment = _environment;
            if (environment == null || !Uri.TryCreate(e.Uri, UriKind.Absolute, out Uri? uri)) {
                return;
            }

            e.Handled = true;

            AddNewTab(environment, uri);
        }

        private async void AddNewTab(CoreWebView2Environment environment, Uri uri)
        {

            WebContentViewModel vm = new(uri);
            TabContent content = new(environment, vm, this);
            TabItem item = new TabItem {
                Content = content,
                Header = uri.Host
            };

            int index = Tabs.Items.Add(item);
            if (index < 0) {
                return;
            }

            // await Task.Delay(200);

            Tabs.SelectedIndex = index;
            Tabs.UpdateLayout();
        }
    }
}
