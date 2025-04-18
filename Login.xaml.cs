using TaxCalculator.Models;

namespace TaxCalculator
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var username = usernameEntry.Text;
            var password = passwordEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("������", "����������, ������� ����� � ������.", "OK");
                return;
            }

            var database = await TaxDatabaseService.LoadDatabaseAsync();
            var user = database.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                await DisplayAlert("������", "�������� ����� ��� ������.", "OK");
                return;
            }

            // ����������� �������
            await DisplayAlert("�����", "�� ������� �����!", "OK");
            // ��������������� �� �������� �������
            await Navigation.PushAsync(new ProfilePage());
        }

        private async void OnNavigateToRegister(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Register());
        }
    }
}
