namespace FAST.Societies.Desktop.Utilities;
public static class Logger
{
    public static void Info(string message) => System.Diagnostics.Debug.WriteLine($"INFO: {message}");
}
