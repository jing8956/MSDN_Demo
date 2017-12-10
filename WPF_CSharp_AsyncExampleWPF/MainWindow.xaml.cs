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

// Add the following using directives, and add a reference for System.Net.Http. 
using System.Net.Http;
using System.Net;
using System.IO;

namespace WPF_CSharp_AsyncExampleWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void startButton_Click(object sender, RoutedEventArgs e)
        {
            // Disable the button until the operation is complete.
            startButton.IsEnabled = false;

            resultsTextBox.Clear();

            //One-step async call.
            //await SumPageSizesAsync();

            //Two-step async call. 
            //Task sumTask = SumPageSizesAsync();
            //await sumTask;

            #region 如何：使用 Async 和 Await 并行发起多个 Web 请求
            await CreateMultipleTasksAsync();
            #endregion

            resultsTextBox.Text += "\r\nControl returned to startButton_Click.\r\n";

            // Reenable the button in case you want to run the operation again.
            startButton.IsEnabled = true;
        }

        private List<string> SetUpURLList()
        {
            var urls = new List<string>
            {
                "http://msdn.microsoft.com/library/windows/apps/br211380.aspx",
                "http://msdn.microsoft.com",
                "http://msdn.microsoft.com/en-us/library/hh290136.aspx",
                "http://msdn.microsoft.com/en-us/library/ee256749.aspx",
                "http://msdn.microsoft.com/en-us/library/hh290138.aspx",
                "http://msdn.microsoft.com/en-us/library/hh290140.aspx",
                "http://msdn.microsoft.com/en-us/library/dd470362.aspx",
                "http://msdn.microsoft.com/en-us/library/aa578028.aspx",
                "http://msdn.microsoft.com/en-us/library/ms404677.aspx",
                "http://msdn.microsoft.com/en-us/library/ff730837.aspx"
            };
            return urls;
        }

        private void DisplayResults(string url, byte[] content)
        {
            // Display the length of each website. The string format  
            // is designed to be used with a monospaced font, such as 
            // Lucida Console or Global Monospace. 
            var bytes = content.Length;
            // Strip off the "http://".
            var displayURL = url.Replace("http://", "");
            resultsTextBox.Text += string.Format("\n{0,-58} {1,8}", displayURL, bytes);
        }

        private async Task SumPageSizesAsync()
        {
            // Make a list of web addresses.
            List<string> urlList = SetUpURLList();

            #region 演练：使用 Async 和 Await 访问 Web
            //var total = 0;

            //foreach (var url in urlList)
            //{
            //    byte[] urlContents = await GetURLContentsAsync(url);

            //    // The previous line abbreviates the following two assignment statements. 

            //    // GetURLContentsAsync returns a Task<T>. At completion, the task 
            //    // produces a byte array. 
            //    //Task<byte[]> getContentsTask = GetURLContentsAsync(url); 
            //    //byte[] urlContents = await getContentsTask;

            //    DisplayResults(url, urlContents);

            //    // Update the total.          
            //    total += urlContents.Length;
            //}
            #endregion

            #region 如何：使用 Task.WhenAll 扩展异步演练
            // Create a query. 
            IEnumerable<Task<int>> downloadTasksQuery =
                from url in urlList select ProcessURLAsync(url);

            // Use ToArray to execute the query and start the download tasks.
            Task<int>[] downloadTasks = downloadTasksQuery.ToArray();

            // You can do other work here before awaiting. 

            // Await the completion of all the running tasks. 
            int[] lengths = await Task.WhenAll(downloadTasks);

            //The previous line is equivalent to the following two statements. 
            //Task<int[]> whenAllTask = Task.WhenAll(downloadTasks); 
            //int[] lengths = await whenAllTask; 

            int total = lengths.Sum();
            #endregion

            // Display the total count for all of the websites.
            resultsTextBox.Text +=
                string.Format("\r\n\r\nTotal bytes returned:  {0}\r\n", total);
        }

        #region 如何：使用 Task.WhenAll 扩展异步演练
        // The actions from the foreach loop are moved to this async method. 
        private async Task<int> ProcessURLAsync(string url)
        {
            var byteArray = await GetURLContentsAsync(url);
            DisplayResults(url, byteArray);
            return byteArray.Length;
        }
        #endregion

        private async Task<byte[]> GetURLContentsAsync(string url)
        {
            //The downloaded resource ends up in the variable named content. 
            var content = new MemoryStream();

            //Initialize an HttpWebRequest for the current URL. 
            var webReq = (HttpWebRequest)WebRequest.Create(url);

            //Send the request to the Internet resource and wait for 
            //the response.                 
            using (WebResponse response = await webReq.GetResponseAsync())

            //The previous statement abbreviates the following two statements. 

            //Task<WebResponse> responseTask = webReq.GetResponseAsync(); 
            //using (WebResponse response = await responseTask)
            {
                // Get the data stream that is associated with the specified url. 
                using (Stream responseStream = response.GetResponseStream())
                {
                    // Read the bytes in responseStream and copy them to content. 
                    await responseStream.CopyToAsync(content);

                    //The previous statement abbreviates the following two statements. 

                    //CopyToAsync returns a Task, not a Task<T>. 
                    //Task copyTask = responseStream.CopyToAsync(content); 

                    //When copyTask is completed, content contains a copy of 
                    //responseStream. 
                    //await copyTask;
                }
            }
            // Return the result as a byte array. 
            return content.ToArray();
        }

        #region 下面的代码包含使用 HttpClient 解决方案，GetByteArrayAsync的完整示例。
        //private async Task SumPageSizesAsync()
        //{
        //    // Make a list of web addresses.
        //    List<string> urlList = SetUpURLList();

        //    // Declare an HttpClient object and increase the buffer size. The 
        //    // default buffer size is 65,536.
        //    HttpClient client =
        //        new HttpClient() { MaxResponseContentBufferSize = 1000000 };

        //    #region 演练：使用 Async 和 Await 访问 Web
        //    //var total = 0;

        //    //foreach (var url in urlList)
        //    //{
        //    //    // GetByteArrayAsync returns a task. At completion, the task 
        //    //    // produces a byte array. 
        //    //    byte[] urlContents = await client.GetByteArrayAsync(url);

        //    //    // The following two lines can replace the previous assignment statement. 
        //    //    //Task<byte[]> getContentsTask = client.GetByteArrayAsync(url); 
        //    //    //byte[] urlContents = await getContentsTask;

        //    //    DisplayResults(url, urlContents);

        //    //    // Update the total.
        //    //    total += urlContents.Length;
        //    //}
        //    #endregion

        //    #region 如何：使用 Task.WhenAll 扩展异步演练
        //    //// Create a query.
        //    //IEnumerable<Task<int>> downloadTasksQuery =
        //    //    from url in urlList select ProcessURL(url, client);

        //    //// Use ToArray to execute the query and start the download tasks.
        //    //Task<int>[] downloadTasks = downloadTasksQuery.ToArray();

        //    //// You can do other work here before awaiting. 

        //    //// Await the completion of all the running tasks. 
        //    //int[] lengths = await Task.WhenAll(downloadTasks);

        //    ////The previous line is equivalent to the following two statements. 
        //    ////Task<int[]> whenAllTask = Task.WhenAll(downloadTasks); 
        //    ////int[] lengths = await whenAllTask; 

        //    //int total = lengths.Sum();
        //    #endregion

        //    // Display the total count for all of the websites.
        //    resultsTextBox.Text +=
        //        string.Format("\r\n\r\nTotal bytes returned:  {0}\r\n", total);
        //}

        #region 如何：使用 Task.WhenAll 扩展异步演练
        // The actions from the foreach loop are moved to this async method.
        async Task<int> ProcessURL(string url, HttpClient client)
        {
            byte[] byteArray = await client.GetByteArrayAsync(url);
            DisplayResults(url, byteArray);
            return byteArray.Length;
        }
        #endregion
        #endregion

        #region 如何：使用 Async 和 Await 并行发起多个 Web 请求
        private async Task CreateMultipleTasksAsync()
        {
            // Declare an HttpClient object, and increase the buffer size. The 
            // default buffer size is 65,536.
            HttpClient client =
                new HttpClient() { MaxResponseContentBufferSize = 1000000 };

            // Create and start the tasks. As each task finishes, DisplayResults  
            // displays its length.
            Task<int> download1 =
                ProcessURLAsync("http://msdn.microsoft.com", client);
            Task<int> download2 =
                ProcessURLAsync("http://msdn.microsoft.com/en-us/library/hh156528(VS.110).aspx", client);
            Task<int> download3 =
                ProcessURLAsync("http://msdn.microsoft.com/en-us/library/67w7t67f.aspx", client);

            // Await each task. 
            int length1 = await download1;
            int length2 = await download2;
            int length3 = await download3;

            int total = length1 + length2 + length3;

            // Display the total count for the downloaded websites.
            resultsTextBox.Text +=
                string.Format("\r\n\r\nTotal bytes returned:  {0}\r\n", total);
        }

        async Task<int> ProcessURLAsync(string url, HttpClient client)
        {
            
            var byteArray = await client.GetByteArrayAsync(url);
            DisplayResults(url, byteArray);
            return byteArray.Length;
        }
        #endregion
    }
}
