using TaxCalculator.Models;

namespace TaxCalculator
{
    public partial class ProfilePage : ContentPage
    {
        private User _currentUser;
        private TaxDatabase _db;
        private List<CheckBox> deductionCheckboxes = new();

        public ProfilePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Загрузка БД
            _db = await TaxDatabaseService.LoadDatabaseAsync();

            // Получение текущего пользователя
            var currentUserLogin = "admin"; // Заменить на значение из авторизации
            _currentUser = _db.Users.FirstOrDefault(u => u.Username == currentUserLogin);

            if (_currentUser != null)
            {
                usernameLabel.Text = $"Логин: {_currentUser.Username}";
                roleLabel.Text = $"Роль: {_currentUser.Role}";
            }

            // Вывод всех доступных вычетов
            DeductionsLayout.Children.Clear();
            deductionCheckboxes.Clear();

            if (_db.Deductions != null)
            {
                deductionsLabel.Text = $"Дедукции: {string.Join(", ", _db.Deductions.Select(d => d.Name))}";
                foreach (var deduction in _db.Deductions)
                {
                    var checkBox = new CheckBox
                    {
                        BindingContext = deduction,
                        VerticalOptions = LayoutOptions.Center
                    };
                    deductionCheckboxes.Add(checkBox);

                    var label = new Label
                    {
                        Text = $"{deduction.Name} ({deduction.Cost} ₽)", // Добавил символ ₽ для валюты
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Colors.Black  // Установил цвет текста в черный
                    };

                    var stack = new HorizontalStackLayout
                    {
                        Children = { checkBox, label },
                        Margin = new Thickness(0, 5)
                    };

                    DeductionsLayout.Children.Add(stack);
                }
            }

            // Последний результат
            if (_db.TaxResults != null)
            {
                var taxResult = _db.TaxResults.FirstOrDefault(r => r.UserId == _currentUser?.Id);
                if (taxResult != null)
                {
                    taxResultLabel.Text = $"Последний налог: {taxResult.Tax:N2} ₽";
                }
            }
        }

        private async void OnCalculateClicked(object sender, EventArgs e)
        {
            if (!double.TryParse(IncomeEntry.Text, out var income))
            {
                await DisplayAlert("Ошибка", "Введите корректный доход", "Ок");
                return;
            }

            var userType = UserTypePicker.SelectedItem?.ToString() ?? "Физическое лицо";

            var selectedDeductions = deductionCheckboxes
                .Where(cb => cb.IsChecked)
                .Select(cb => (Deduction)cb.BindingContext)
                .ToList();

            var totalDeductions = selectedDeductions.Sum(d => d.Cost);
            var incomeAfterDeductions = income - totalDeductions;

            if (incomeAfterDeductions <= 0)
            {
                await DisplayAlert("Внимание",
                    $"Сумма вычетов ({totalDeductions:N2}) превышает или равна доходу ({income:N2}). Налог будет рассчитан с нуля.",
                    "Ок");

                incomeAfterDeductions = 0;
            }

            var taxRate = await TaxDatabaseService.GetTaxRateAsync(userType);
            var tax = incomeAfterDeductions * (taxRate / 100.0);
            var resultAfterTax = incomeAfterDeductions - tax;

            if (_currentUser != null)
            {
                await TaxDatabaseService.SaveTaxResultAsync(new TaxResult
                {
                    UserId = _currentUser.Id,
                    Income = income,
                    Tax = tax,
                    UserType = userType
                });

                taxResultLabel.Text = $"Последний налог: {tax:N2} ₽";  // Здесь убрали знак "?"
            }

            string formula = $"({income:N2} - {totalDeductions:N2}) × {taxRate}% = {tax:N2} ₽"; // Заменили знак на "₽"

            // Выделяем результат крупным шрифтом и цветом
            ResultLabel.FontSize = 22;
            ResultLabel.TextColor = Colors.DarkGreen;

            ResultLabel.Text = $"📊 Формула:\n{formula}\n\n💰 Доход после налога: {resultAfterTax:N2} ₽\n💸 Налог: {tax:N2} ₽"; // Здесь тоже знак "₽"
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}
