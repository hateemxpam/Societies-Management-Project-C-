using System.Text.Json;

namespace FAST.Societies.Desktop.Data;

public sealed class AppConfig
{
    public string ConnectionString { get; set; } =
        "Server=DESKTOP-UML3T13\\SQLEXPRESS;Database=FASTSocietiesDB;Trusted_Connection=True;TrustServerCertificate=True;";

    public static AppConfig Load()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
        if (!File.Exists(path))
        {
            var cfg = new AppConfig();
            File.WriteAllText(path, JsonSerializer.Serialize(cfg, new JsonSerializerOptions { WriteIndented = true }));
            return cfg;
        }

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
    }
}
