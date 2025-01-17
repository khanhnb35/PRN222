using System.Net.Http;
using System.Windows;

namespace HttpClientClassDemo_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        readonly HttpClient client = new HttpClient();
        private void btnClose_Click(object sender, RoutedEventArgs e) => Close();

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtContent.Text = string.Empty;
        }

        private async void btnViewHTML_Click(object sender, RoutedEventArgs e)
        {
            string uri = txtURL.Text;

            // Call asynchronous network methods
            // in a try/catch block to handle exceptions
            try
            {
                string responseBody = await client.GetStringAsync(uri);
                txtContent.Text = responseBody.Trim();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Message: {ex.Message}");
            }
        }
    }