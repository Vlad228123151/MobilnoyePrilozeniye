using System.Text.Json;
using TaxCalculator.Models;

public static class TaxDatabaseService
{
    private static readonly string DbFilePath = Path.Combine(FileSystem.AppDataDirectory, "tax_database.json");

    public static async Task<TaxDatabase> LoadDatabaseAsync()
    {
        if (!File.Exists(DbFilePath))
        {
            var defaultDatabase = new TaxDatabase
            {
                Users = new List<User>
                {
                    new User { Id = 1, Username = "admin", Password = "adminpassword", Role = "admin" }
                },
                Deductions = new List<Deduction>
                {
                    new Deduction { Id = 1, Name = "Образование", Description = "Расходы на обучение", Cost = 15000 }
                },
                TaxRates = new List<TaxRate>
                {
                    new TaxRate { Id = 1, UserType = "Физическое лицо", Rate = 13 },
                    new TaxRate { Id = 2, UserType = "Юридическое лицо", Rate = 20 }
                },
                TaxResults = new List<TaxResult>
                {
                    new TaxResult { Id = 1, UserId = 1, Income = 100000, Tax = 13000, UserType = "Физическое лицо" }
                }
            };

            await SaveDatabaseAsync(defaultDatabase);
            return defaultDatabase;
        }

        var content = await File.ReadAllTextAsync(DbFilePath);
        return JsonSerializer.Deserialize<TaxDatabase>(content) ?? new TaxDatabase();
    }

    public static async Task SaveDatabaseAsync(TaxDatabase db)
    {
        var json = JsonSerializer.Serialize(db, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(DbFilePath, json);
    }

    public static async Task<User?> GetUserByUsernameAsync(string username)
    {
        var db = await LoadDatabaseAsync();
        return db.Users.FirstOrDefault(u => u.Username == username);
    }

    // ✅ Получение налоговой ставки по типу пользователя
    public static async Task<double> GetTaxRateAsync(string userType)
    {
        var db = await LoadDatabaseAsync();
        var rate = db.TaxRates.FirstOrDefault(r => r.UserType == userType);
        return rate?.Rate ?? 13.0; // По умолчанию 13%, если не найдено
    }

    // ✅ Сохранение результата налогообложения
    public static async Task SaveTaxResultAsync(TaxResult result)
    {
        var db = await LoadDatabaseAsync();

        // Удалить старый результат пользователя, если он есть
        db.TaxResults.RemoveAll(r => r.UserId == result.UserId);

        // Добавить новый
        result.Id = db.TaxResults.Count > 0 ? db.TaxResults.Max(r => r.Id) + 1 : 1;
        db.TaxResults.Add(result);

        await SaveDatabaseAsync(db);
    }
}
