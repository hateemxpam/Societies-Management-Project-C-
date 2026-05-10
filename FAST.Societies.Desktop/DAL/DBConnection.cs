namespace FAST.Societies.Desktop.DAL;

public static class DBConnection
{
    public static string GetConnectionString()
    {
        return "Server=localhost\\SQLEXPRESS;Database=FASTSocietiesDB;Trusted_Connection=True;TrustServerCertificate=True;";
    }
}
