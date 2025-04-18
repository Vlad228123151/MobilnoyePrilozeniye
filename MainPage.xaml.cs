namespace TaxCalculator
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnReadMoreClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Новость", "Полный текст новости. Здесь может быть больше информации об изменениях в законодательстве.", "OK");
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            // Переход на страницу входа
            await Navigation.PushAsync(new Login());
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            // Переход на страницу регистрации
            await Navigation.PushAsync(new Register());
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            // Переход на главную страницу
            await Navigation.PopAsync();
        }
    }
}
