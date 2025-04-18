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
                await DisplayAlert("Ошибка", "Все поля должны быть заполнены.", "OK");
                return;
            }

            if (password != confirmPassword)
            {
                await DisplayAlert("Ошибка", "Пароли не совпадают.", "OK");
                return;
            }

            var database = await TaxDatabaseService.LoadDatabaseAsync();

            // Проверяем, существует ли уже пользователь с таким логином
            if (database.Users.Any(u => u.Username == username))
            {
                await DisplayAlert("Ошибка", "Пользователь с таким логином уже существует.", "OK");
                return;
            }

            // Добавляем нового пользователя
            var newUser = new User
            {
                Id = database.Users.Max(u => u.Id) + 1,
                Username = username,
                Password = password, // В реальной системе стоит использовать хеширование пароля
                Role = "user"
            };

            database.Users.Add(newUser);
            await TaxDatabaseService.SaveDatabaseAsync(database);

            await DisplayAlert("Успех", "Регистрация прошла успешно!", "OK");
            await Navigation.PushAsync(new Login()); // Переходим к странице логина
        }

        private async void OnNavigateToLogin(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Login());
        }
    }
}


