using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        // Fetch Data Button Click
        private async void FetchButton_Click(object sender, RoutedEventArgs e)
        {
            string url = UrlTextBox.Text;

            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("Please enter a valid URL.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                ContentTextBox.Text = "Fetching data, please wait...";
                string content = await FetchDataAsync(url);
                ContentTextBox.Text = content;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Failed to fetch data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ContentTextBox.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ContentTextBox.Text = string.Empty;
            }
        }

        // Fetch Data from URL
        private async Task<string> FetchDataAsync(string url)
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // Clear Button Click
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            UrlTextBox.Text = string.Empty;
            ContentTextBox.Text = string.Empty;
        }

        // Close Button Click
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}