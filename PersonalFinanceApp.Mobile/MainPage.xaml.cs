using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PersonalFinanceApp.Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Username and password are required.", "OK");
                return;
            }

            SetInputsEnabled(false);

            LoadingIndicator.IsRunning = true;
            LoadingIndicator.IsVisible = true;

            var loginData = new { Username = username, Password = password };
            var jsonData = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var apiUrl = "https://localhost:7253/api/Account/login";
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.PostAsync(apiUrl, content);
                    response.EnsureSuccessStatusCode();

                    var responseContent = await response.Content.ReadAsStringAsync();

                    //var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
                    //await DisplayAlert("Success", $"Login successful!\nToken: {tokenResponse.Token}", "OK");
                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
                    await Navigation.PushAsync(new HomePage(tokenResponse.Token));
                }
                catch (HttpRequestException)
                {
                    await DisplayAlert("Error", "Failed to login. Please try again later.", "OK");
                }
                finally
                {
                    SetInputsEnabled(true);

                    LoadingIndicator.IsRunning = false;
                    LoadingIndicator.IsVisible = false;
                }
            }
        }

        private class TokenResponse
        {
            public string Token { get; set; }
        }

        private void SetInputsEnabled(bool isEnabled)
        {
            UsernameEntry.IsEnabled = isEnabled;
            PasswordEntry.IsEnabled = isEnabled;
        }
    }

}
