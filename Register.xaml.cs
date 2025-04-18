using TaxCalculator.Models;

namespace TaxCalculator
{
    public partial class Register : ContentPage
    {
        public Register()
        {
            InitializeComponent();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var username = usernameEntry.Text;
            var password = passwordEntry.Text;
            var confirmPassword = confirmPasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                await DisplayAlert("������", "��� ���� ������ ���� ���������.", "OK");
                return;
            }

            if (password != confirmPassword)
            {
                await DisplayAlert("������", "������ �� ���������.", "OK");
                return;
            }

            var database = await TaxDatabaseService.LoadDatabaseAsync();

            // ���������, ���������� �� ��� ������������ � ����� �������
            if (database.Users.Any(u => u.Username == username))
            {
                await DisplayAlert("������", "������������ � ����� ������� ��� ����������.", "OK");
                return;
            }

            // ��������� ������ ������������
            var newUser = new User
            {
                Id = database.Users.Max(u => u.Id) + 1,
                Username = username,
                Password = password, // � �������� ������� ����� ������������ ����������� ������
                Role = "user"
            };

            database.Users.Add(newUser);
            await TaxDatabaseService.SaveDatabaseAsync(database);

            await DisplayAlert("�����", "����������� ������ �������!", "OK");
            await Navigation.PushAsync(new Login()); // ��������� � �������� ������
        }

        private async void OnNavigateToLogin(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Login());
        }
    }
}


