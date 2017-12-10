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

// Add the following using directive, and add a reference for System.Net.Http.
using System.Net.Http;
using System.Threading;

namespace WPF_CSharp_CancelAndRestart
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        // *** Declare a System.Threading.CancellationTokenSource.
        CancellationTokenSource cts;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            //This line is commented out to make the results clearer in the output.
            //ResultsTextBox.Text = "";

            // *** If a download process is already underway, cancel it.
            if (cts != null)
            {
                cts.Cancel();
            }

            // *** Now set cts to a new value that you can use to cancel the current process
            // if the button is chosen again.
            CancellationTokenSource newCTS = new CancellationTokenSource();
            cts = newCTS;

            try
            {
                // ***Send cts.Token to carry the message if there is a cancellation request.
                await AccessTheWebAsync(cts.Token);
            }
            // *** Catch cancellations separately.
            catch (OperationCanceledException)
            {
                ResultsTextBox.Text += "\r\nDownloads canceled.\r\n";
            }
            catch (Exception)
            {
                ResultsTextBox.Text += "\r\nDownloads failed.";
            }

            // *** When the process is complete, signal that another process can begin.
            if (cts == newCTS)
                cts = null;
        }

        // *** Provide a parameter for the CancellationToken from StartButton_Click.
        private async Task AccessTheWebAsync(CancellationToken ct)
        {
            // Declare an HttpClient object.
            HttpClient client = new HttpClient();

            // Make a list of web addresses.
            List<string> urlList = SetUpURLList();

            var total = 0;
            var position = 0;

            foreach (var url in urlList)
            {
                // *** Use the HttpClient.GetAsync method because it accepts a 
                // cancellation token.
                HttpResponseMessage response = await client.GetAsync(url, ct);

                // *** Retrieve the website contents from the HttpResponseMessage.
                byte[] urlContents = await response.Content.ReadAsByteArrayAsync();

                // *** Check for cancellations before displaying information about the 
                // latest site. 
                ct.ThrowIfCancellationRequested();

                DisplayResults(url, urlContents, ++position);

                // Update the total.
                total += urlContents.Length;
            }

            // Display the total count for all of the websites.
            ResultsTextBox.Text +=
                string.Format("\r\n\r\nTOTAL bytes returned:  {0}\r\n", total);
        }

        private List<string> SetUpURLList()
        {
            List<string> urls = new List<string>
            {
                "http://msdn.microsoft.com/en-us/library/hh191443.aspx",
                "http://msdn.microsoft.com/en-us/library/aa578028.aspx",
                "http://msdn.microsoft.com/en-us/library/jj155761.aspx",
                "http://msdn.microsoft.com/en-us/library/hh290140.aspx",
                "http://msdn.microsoft.com/en-us/library/hh524395.aspx",
                "http://msdn.microsoft.com/en-us/library/ms404677.aspx",
                "http://msdn.microsoft.com",
                "http://msdn.microsoft.com/en-us/library/ff730837.aspx"
            };
            return urls;
        }

        private void DisplayResults(string url, byte[] content, int pos)
        {
            // Display the length of each website. The string format is designed
            // to be used with a monospaced font, such as Lucida Console or 
            // Global Monospace.

            // Strip off the "http://".
            var displayURL = url.Replace("http://", "");
            // Display position in the URL list, the URL, and the number of bytes.
            ResultsTextBox.Text += string.Format("\n{0}. {1,-58} {2,8}", pos, displayURL, content.Length);
        }
    }
}
